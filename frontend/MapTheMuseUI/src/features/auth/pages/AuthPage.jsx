import {
  Box,
  Paper,
  Stack,
  Typography,
} from "@mui/material";
import React from "react";
import AuthForm from "../components/AuthForm";
import useAuth from "../hooks/useAuth";
import { useLocation, useNavigate } from "react-router-dom";

const AuthPage = () => {
  const { login, register, loading, error } = useAuth();
  const { pathname } = useLocation();
  const navigate = useNavigate();

  const mode = pathname.endsWith("/register") ? "register" : "login";
  const toggleMode = () => navigate(mode === "login" ? "/register" : "/login");

  const handleSubmit = async (formData) => {
    if (mode === "login") await login(formData);
    else await register(formData);
  };

  return (
    // Frame (kept your border/shadow vibe)
    <Box
      sx={{
        position: "relative",
        width: "100%",
        minHeight: "80vh",
        borderRadius: "25px",
        overflow: "hidden",
        bgcolor: "background.default",
        border: "6px solid",
        borderColor: "background.default",
        boxShadow: "16px 49px 45.3px rgba(12, 12, 13, 0.4)",
      }}
    >
      {/* Background image (ABSOLUTE) */}
      <Box
        component="img"
        src="/destinationPhotos/Airport.jpeg"
        alt=""
        aria-hidden
        sx={{
          position: "absolute",
          inset: 0,
          width: "100%",
          height: "100%",
          objectFit: "cover",
          objectPosition: "center",
          zIndex: 0,
        }}
      />

      {/* Gradient overlay (ABSOLUTE) */}
      <Box
        sx={{
          position: "absolute",
          inset: 0,
          background:
            "linear-gradient(0deg, rgba(132,132,132,0.35), rgba(246,246,246,0.35))",
          pointerEvents: "none",
          zIndex: 0,
        }}
      />

      {/* Foreground content (RELATIVE, centred) */}
      <Box
        sx={{
          position: "relative",
          zIndex: 1,
          height: "100%",
          display: "grid",
          placeItems: "center",
          px: { xs: 2, md: 4 },
          py: { xs: 6, md: 10 },
        }}
      >
        <Stack
          direction={{ xs: "column", md: mode === "register" ? "row" : "column" }}
          spacing={{ xs: 3, md: 6 }}
          alignItems="center"
          sx={{ width: "100%", maxWidth: 1100 }}
        >
          {/* Left copy only on register (hidden on mobile like Pinterest) */}
          {mode === "register" && (
            <Box sx={{ flex: 1, display: { xs: "none", md: "block" } }}>
              <Typography
                variant="h2"
                sx={(t) => ({
                  fontWeight: 800,
                  color: t.palette.common.white,
                  textShadow: "0 2px 8px rgba(0,0,0,.35)",
                })}
              >
                We’re excited to meet you!
              </Typography>
              <Typography
                variant="h5"
                sx={(t) => ({
                  mt: 1,
                  color: t.palette.common.white,
                  textShadow: "0 1px 6px rgba(0,0,0,.35)",
                })}
              >
                Your next adventure awaits…
              </Typography>
            </Box>
          )}

          {/* The form — centred overlay with a soft glass card */}
            <AuthForm
              type={mode}
              onSubmit={handleSubmit}
              loading={loading}
              error={error}
              toggleType={toggleMode}
            />
        </Stack>
      </Box>
    </Box>
  );
};

export default AuthPage;
