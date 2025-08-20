import {
  Box, Stack, Avatar, Typography, Button, Divider,
} from "@mui/material";
import ShareIcon from "@mui/icons-material/Share";
import EditIcon from "@mui/icons-material/Edit";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import LanguageIcon from "@mui/icons-material/Language";

export default function ProfileHeader({ user, actions }) {
  const fullName = (() => {
    const f = user?.firstName ?? "";
    const l = user?.lastName ?? "";
    const name = `${f} ${l}`.trim();
    return name || user?.userName || "Your profile";
  })();

  const initials = fullName
    .split(" ")
    .filter(Boolean)
    .map((p) => p[0])
    .join("")
    .slice(0, 2)
    .toUpperCase();

  const username = user?.userName;
  const avatarUrl = user?.profilePictureUrl || "";
  const country = user?.country;
  const preferredLanguage = user?.preferredLanguage;

  return (
    <Box sx={{ maxWidth: 960, mx: "auto", px: 5, py: 10 }}>
      <Stack alignItems="center" spacing={4}>
        <Avatar src={avatarUrl || undefined} alt={`${fullName}'s avatar`} sx={{ width: 96, height: 96 }}>
          {initials}
        </Avatar>

        <Typography variant="h4" sx={{ fontWeight: 800, textAlign: "center" }}>
          {fullName}
        </Typography>

        {username && (
          <Typography variant="body2" sx={{ color: "text.secondary" }}>
            @{username}
          </Typography>
        )}

        <Stack direction="row" alignItems="center" spacing={2} sx={{ color: "text.secondary", mt: 0.5 }}>
          {country && (
            <Stack direction="row" spacing={0.75} alignItems="center">
              <LocationOnIcon fontSize="small" />
              <span>{country}</span>
            </Stack>
          )}
          {country && preferredLanguage && <Divider flexItem orientation="vertical" />}
          {preferredLanguage && (
            <Stack direction="row" spacing={0.75} alignItems="center">
              <LanguageIcon fontSize="small" />
              <span>{preferredLanguage}</span>
            </Stack>
          )}
        </Stack>

        {/* Default actions if none passed in */}
        <Stack direction="row" spacing={1.2} sx={{ mt: 1 }}>
          {actions ?? (
            <>
              <Button variant="outlined" startIcon={<ShareIcon />} sx={{ borderRadius: 3 }}>
                Share
              </Button>
              <Button
                variant="contained"
                startIcon={<EditIcon />}
                sx={{ borderRadius: 3 }}
                href="/profile/edit"
              >
                Edit profile
              </Button>
            </>
          )}
        </Stack>
      </Stack>
    </Box>
  );
}