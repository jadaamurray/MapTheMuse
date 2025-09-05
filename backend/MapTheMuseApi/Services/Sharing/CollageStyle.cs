public record CollageStyle(
    int Width = 1080,
    int Height = 1080,
    int Cols = 3,
    int Padding = 32,
    int Gap = 12,
    float CornerRadius = 18f,
    uint Background = 0xFFF7F6F3,      // ARGB
    uint Placeholder = 0xFFE6E4E0,
    bool AddBorder = false,
    uint BorderColour = 0xFFFFFFFF,
    float BorderWidth = 2f,
    bool AddDropShadow = true,
    float ShadowBlur = 10f,
    float ShadowDx = 0f,
    float ShadowDy = 4f,
    bool AddBottomGradient = true,
    byte GradientOpacity = 170,         // 0–255
    float TitleSize = 44f,
    uint TitleColour = 0xFFFFFFFF,
    string? WatermarkPath = null,       // later add logo
    float WatermarkScale = 0.18f,       // % of height
    int PreferredColsWhenFew = 0        // e.g. 2 for small sets
);
