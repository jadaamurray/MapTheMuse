// Services/Media/MediaService.cs
using MapTheMuseApi.Dtos;
using MapTheMuseApi.Models;
using Microsoft.EntityFrameworkCore;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _repo;
    private readonly IMediaSpineService _spine;

    public MediaService(IMediaRepository repo, IMediaSpineService spine)
    { _repo = repo; _spine = spine; }

    public async Task<List<MediaListDto>> GetAllAsync(int? skip = null, int? take = null)
    {
        IQueryable<Media> q = _repo.Query().OrderByDescending(m => m.LastSyncedUtc);

        if (skip is int s) q = q.Skip(s);
        if (take is int t) q = q.Take(t);

        return await q.Select(m => new MediaListDto
        {
            Id = m.Id,
            ExternalId = m.ExternalId,
            Source = m.Source,
            Title = m.Title,
            ShortDescription = string.IsNullOrEmpty(m.Description)
                ? null
                : (m.Description!.Length <= 100 ? m.Description : m.Description!.Substring(0, 97) + "…"),
            Creator = m.Creator,
            Type = m.Type,
            ReleaseDate = m.ReleaseDate
        }).ToListAsync();
    }

    public async Task<MediaDetailDto?> GetByIdAsync(int id)
    {
        return await _repo.Query()
            .Where(x => x.Id == id)
            .Select(m => new MediaDetailDto
            {
                Id = m.Id,
                ExternalId = m.ExternalId,
                Source = m.Source,
                Title = m.Title,
                Description = m.Description,
                Creator = m.Creator,
                Type = m.Type,
                ReleaseDate = m.ReleaseDate,
                PosterPath = m.PosterPath,
                LastSyncedUtc = m.LastSyncedUtc
            })
            .SingleOrDefaultAsync();
    }

    public async Task<MediaDetailDto> CreateAsync(MediaCreateUpdateDto dto)
    {
        // use Spine to dedupe by source/externalId
        Media entity;
        if (!string.IsNullOrWhiteSpace(dto.Source) &&
            !string.IsNullOrWhiteSpace(dto.ExternalId))
        {
            entity = await _spine.EnsureAsync(dto.Type, dto.ExternalId!, dto.Source!, dto.Title, dto.PosterPath);
            // fill remaining fields below
            entity.Description = dto.Description ?? entity.Description;
            entity.Creator = dto.Creator ?? entity.Creator;
            entity.ReleaseDate = dto.ReleaseDate ?? entity.ReleaseDate;
            await _repo.UpdateAsync(entity);
        }
        else
        {
            entity = new Media
            {
                Source = dto.Source,
                ExternalId = dto.ExternalId,
                Type = dto.Type,
                Title = dto.Title,
                PosterPath = dto.PosterPath,
                Description = dto.Description,
                Creator = dto.Creator,
                ReleaseDate = dto.ReleaseDate,
                LastSyncedUtc = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
        }

        return await GetByIdAsync(entity.Id) ?? new MediaDetailDto { Id = entity.Id, Title = entity.Title };
    }

    public async Task<bool> UpdateAsync(int id, MediaCreateUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;

        entity.Title = dto.Title ?? entity.Title;
        entity.Description = dto.Description ?? entity.Description;
        entity.Creator = dto.Creator ?? entity.Creator;
        entity.PosterPath = dto.PosterPath ?? entity.PosterPath;
        entity.ReleaseDate = dto.ReleaseDate ?? entity.ReleaseDate;
        entity.LastSyncedUtc = DateTime.UtcNow;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repo.ExistsAsync(id)) return false;
        await _repo.DeleteAsync(id);
        return true;
    }
}
