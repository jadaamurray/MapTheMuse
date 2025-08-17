import { Box, Typography, Link, Stack } from "@mui/material";

export default function Footer() {
  return (
    <Box
      component="footer"
      sx={{
        bgcolor: "primary.main",
        color: "white",
        py: 2,
        px: 2,
        mt: "auto", // pushes footer to bottom if using flex column layout
      }}
    >
      <Stack
        direction={{ xs: "column", sm: "row" }}
        spacing={2}
        justifyContent="space-between"
        alignItems="center"
        maxWidth="lg"
        mx="auto"
      >
        <Typography variant="body2">
          © {new Date().getFullYear()} Map The Muse. All rights reserved.
        </Typography>

        <Stack direction="row" spacing={3}>
          <Link href="/privacy" color="inherit" underline="hover">
            Privacy Policy
          </Link>
          <Link href="/terms" color="inherit" underline="hover">
            Terms of Service
          </Link>
        </Stack>
      </Stack>
    </Box>
  );
}
