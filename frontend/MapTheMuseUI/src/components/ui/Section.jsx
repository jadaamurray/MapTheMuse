import { Box, Stack, Typography } from "@mui/material";

export default function Section({ id, title, subtitle, action, children }) {
  return (
    <Box id={id} sx={{ scrollMarginTop: { xs: 72, md: 96 }, mb: 1 }}>
      <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 1 }}>
        <Box>
          <Typography variant="h5" sx={{ fontWeight: 800 }}>
            {title}
          </Typography>
          {subtitle && (
            <Typography variant="body2" sx={{ color: "text.secondary" }}>
              {subtitle}
            </Typography>
          )}
        </Box>
        {action}
      </Stack>
      {children}
    </Box>
  );
}
