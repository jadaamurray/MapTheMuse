using SkiaSharp;

public record CollageItem(string ImageUrl);

public class CollageRenderer
{
    private readonly IImageFetcher _fetcher;
    private readonly SKTypeface _typeface;

    public CollageRenderer(IImageFetcher fetcher)
    {
        _fetcher = fetcher;
        // later: load brand font (place a .ttf in wwwroot/fonts or embed as resource)
        _typeface = SKTypeface.FromFamilyName("Inter") ?? SKTypeface.Default;
    }

    public async Task<byte[]?> RenderAsync(
         IReadOnlyList<CollageItem> items,
        string title,
        CollageStyle? style = null,
        CancellationToken ct = default)
    {
        style ??= new CollageStyle();
        int width = style.Width, height = style.Height;
        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(new SKColor(245, 244, 240)); // subtle off‑white

        // Grid maths
        int cols = style.Cols;
        int padding = style.Padding;
        int gap = style.Gap;
        int count = Math.Min(items.Count, cols * cols);
        int rows = (int)Math.Ceiling(count / (double)cols);
        int cell = (width - (cols - 1) * gap - 2 * padding) / cols;
        int gridH = rows * cell + (rows - 1) * gap;
        int startY = Math.Max((height - gridH) / 2 - 20, padding);

        
        // Draw tiles
        for (int i = 0; i < count; i++)
        {
            int row = i / cols, col = i % cols;
            int x = padding + col * cell;
            int y = startY + row * cell;
            var rect = new SKRect(x, y, x + cell, y + cell);

            var img = await _fetcher.FetchAsSkImageAsync(items[i].ImageUrl, ct);
            canvas.Save();
            if (img is not null)
            {
                // centre‑crop to square
                var src = img.Width >= img.Height
                    ? new SKRect((img.Width - img.Height) / 2f, 0, (img.Width + img.Height) / 2f, img.Height)
                    : new SKRect(0, (img.Height - img.Width) / 2f, img.Width, (img.Height + img.Width) / 2f);
                canvas.DrawImage(img, src, rect, new SKPaint { FilterQuality = SKFilterQuality.High });
            }
            else
            {
                using var ph = new SKPaint { Color = new SKColor(225, 225, 225) };
            }
            canvas.Restore();
        }

        // Bottom gradient for text
        using (var grad = new SKPaint { IsAntialias = true })
        {
            grad.Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, height * 0.6f), new SKPoint(0, height),
                new[] { new SKColor(0, 0, 0, 0), new SKColor(0, 0, 0, 160) },
                new float[] { 0, 1 }, SKShaderTileMode.Clamp);
            canvas.DrawRect(new SKRect(0, height * 0.55f, width, height), grad);
        }

        // Title
        using var text = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true,
            Typeface = _typeface,
            TextSize = 42
        };
        canvas.DrawText(title, padding, height - 28, text);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 90);
        return data.ToArray();
    }
}
