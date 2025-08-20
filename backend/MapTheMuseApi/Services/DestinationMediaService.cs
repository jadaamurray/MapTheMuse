using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Dtos;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public interface IDestinationMediaService
{
     // List all linked media for a destination
    Task<IReadOnlyList<DestinationMediaItemDto>> GetForDestinationAsync(
        int destinationId,
        CancellationToken ct = default);

    // Link by external triple (Source, Type, ExternalId) + optional cached fields
    Task<int> LinkAsync(
        int destinationId,
        CreateDestinationMediaLinkDto dto,
        string? createdById,
        CancellationToken ct = default);

    // Unlink by link id
    Task UnlinkAsync(int linkId, CancellationToken ct = default);

    // update context note
    Task UpdateContextNoteAsync(int linkId, string? contextNote, CancellationToken ct = default);

    // reorder links for a destination
    Task ReorderAsync(
        int destinationId,
        IReadOnlyList<(int linkId, int? orderIndex)> newOrder,
        CancellationToken ct = default);
}

public class DestinationMediaService : IDestinationMediaService
    {
        private readonly MapTheMuseContext _db;
        private readonly TmdbClient _tmdb;
        private readonly IMediaSpineService _mediaSpine;

        public DestinationMediaService(
            MapTheMuseContext db,
            TmdbClient tmdb,
            IMediaSpineService mediaSpine)
        {
            _db = db;
            _tmdb = tmdb;
            _mediaSpine = mediaSpine;
        }

        public async Task<IReadOnlyList<DestinationMediaItemDto>> GetForDestinationAsync(
            int destinationId,
            CancellationToken ct = default)
        {
            var links = await _db.DestinationMediaLinks
                .AsNoTracking()
                .Include(l => l.Media)
                .Where(l => l.DestinationId == destinationId)
                .OrderBy(l => l.OrderIndex ?? int.MaxValue)
                .ThenBy(l => l.Id)
                .ToListAsync(ct);

            // Build DTOs for quicker responses.
            var tasks = links.Select(l => BuildDtoAsync(l, ct)).ToArray();
            await Task.WhenAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }

        public async Task<int> LinkAsync(
            int destinationId,
            CreateDestinationMediaLinkDto dto,
            string? createdById,
            CancellationToken ct = default)
        {
            var source = string.IsNullOrWhiteSpace(dto.Source) ? "TMDB" : dto.Source.Trim();

            // Ensure a Media row exists for (Source, Type, ExternalId)
            var media = await _mediaSpine.EnsureAsync(
                dto.Type, dto.ExternalId, source, dto.Title, dto.PosterPath);

            // Prevent duplicates per destination/media
            var exists = await _db.DestinationMediaLinks
                .AnyAsync(x => x.DestinationId == destinationId && x.MediaId == media.Id, ct);

            if (exists)
                throw new InvalidOperationException("Media already linked to this destination.");

            var link = new DestinationMediaLink
            {
                DestinationId = destinationId,
                MediaId = media.Id,
                ContextNote = dto.ContextNote,
                OrderIndex = dto.OrderIndex,
                CreatedById = createdById
            };

            _db.DestinationMediaLinks.Add(link);
            await _db.SaveChangesAsync(ct);
            return link.Id;
        }

        public async Task UnlinkAsync(int linkId, CancellationToken ct = default)
        {
            var link = await _db.DestinationMediaLinks.FindAsync([linkId], ct);
            if (link == null) return;

            _db.DestinationMediaLinks.Remove(link);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateContextNoteAsync(int linkId, string? contextNote, CancellationToken ct = default)
        {
            var link = await _db.DestinationMediaLinks.FirstOrDefaultAsync(l => l.Id == linkId, ct);
            if (link == null) return;

            link.ContextNote = string.IsNullOrWhiteSpace(contextNote) ? null : contextNote.Trim();
            await _db.SaveChangesAsync(ct);
        }

        public async Task ReorderAsync(
            int destinationId,
            IReadOnlyList<(int linkId, int? orderIndex)> newOrder,
            CancellationToken ct = default)
        {
            if (newOrder == null || newOrder.Count == 0) return;

            var ids = newOrder.Select(x => x.linkId).ToHashSet();
            var links = await _db.DestinationMediaLinks
                .Where(l => l.DestinationId == destinationId && ids.Contains(l.Id))
                .ToListAsync(ct);

            // Apply new order indices
            var byId = newOrder.ToDictionary(x => x.linkId, x => x.orderIndex);
            foreach (var l in links)
            {
                l.OrderIndex = byId[l.Id];
            }

            await _db.SaveChangesAsync(ct);
        }

        // ---------- helpers ----------

        private async Task<DestinationMediaItemDto> BuildDtoAsync(DestinationMediaLink link, CancellationToken ct)
        {
            var m = link.Media ?? throw new InvalidOperationException("Link has no Media loaded.");

            // Start from cached values (fast) and hydrate with TMDB if applicable.
            string? title = m.Title;
            string? overview = m.Description;
            string? creator = m.Creator;
            string? poster = m.PosterPath;
            int? year = m.ReleaseDate?.Year;

            var isTmdb = string.Equals(m.Source, "TMDB", StringComparison.OrdinalIgnoreCase);
            if (isTmdb)
            {
                if (m.Type == MediaType.Movie)
                {
                    var (t, y, c, p, o) = await _tmdb.GetMovieAsync(m.ExternalId, null, ct);
                    title = t ?? title;
                    year = y ?? year;
                    creator = c ?? creator;
                    poster = p ?? poster;
                    overview = o ?? overview;
                }
                else if (m.Type == MediaType.Tv)
                {
                    var (t, y, c, p, o) = await _tmdb.GetTvAsync(m.ExternalId, null, ct);
                    title = t ?? title;
                    year = y ?? year;
                    creator = c ?? creator;
                    poster = p ?? poster;
                    overview = o ?? overview;
                }
            }

            return new DestinationMediaItemDto(
                LinkId: link.Id,
                Source: m.Source,
                ExternalId: m.ExternalId,
                Type: m.Type,
                Title: title ?? m.ExternalId,
                Year: year,
                Creator: creator,
                PosterPath: poster,
                Overview: overview,
                ContextNote: link.ContextNote
            );
        }
    }
