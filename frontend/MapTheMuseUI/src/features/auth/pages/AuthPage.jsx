import {
  Box,
  Button,
  Link,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import React, { useState } from "react";
import AuthForm from "../components/AuthForm";
import useAuth from "../hooks/useAuth";
import BlurCircleBackground from "../../../components/ui/BlurCircleBackground";
import { useLocation, useNavigate } from "react-router-dom";

const AuthPage = () => {
  /* const [type, setType] = useState('login');  // will be either login or register
  const toggleType = () => {
    setType(prev => (prev === 'login' ? 'register' : 'login'));
  }; */

  const { login, register, loading, error } = useAuth();
  const { pathname } = useLocation();
  const navigate = useNavigate();

  const mode = pathname.endsWith("/register") ? "register" : "login";

  const toggleMode = () => {
    navigate(mode === "login" ? "/register" : "/login");
  };

  const handleSubmit = async (formData) => {
    if (mode === 'login') {
      await login(formData);
    } else {
      await register(formData);
    }
  };

  return (
    // Full‐page container
    <Box
      sx={{
        flexGrow: 1,
        alignSelf: "stretch",
        width: "100%",
        bgcolor: "background.default",
        borderRadius: "25px",
        overflow: "hidden",
        border: "6px solid",
        borderColor: "background.default",
        boxShadow: "16px 49px 45.3px rgba(12, 12, 13, 0.4)",
      }}
    >
      <Box sx={{
        position: "relative",
        display: "flex",
        justifyContent: "center",
        px: 10, py: 30,
        alignItems: "center"
      }}>
        {/* Background elements */}
        <BlurCircleBackground />
        {/* Foreground elements */}
        <Stack direction="row" spacing={4} alignItems={"center"} zIndex={1}>
          {mode === 'register' && (
            <>
              <Box sx={{ flex: '0 1 50%' }}>
                <Typography
                  variant="h1"
                  textAlign="left"
                  sx={(theme) => ({
                    textShadow: `
                  var(--sds-size-depth-0) var(--sds-size-depth-100) var(--sds-size-depth-100) var(--sds-color-black-200),
                  var(--sds-size-depth-0) var(--sds-size-depth-100) var(--sds-size-depth-100) var(--sds-color-black-100)
                `,
                    WebkitTextStrokeColor: theme.palette.background.white,
                    WebkitTextStrokeWidth: '2px',
                  })}
                >
                  We are excited to meet you!
                </Typography>
                <Typography
                  variant="h2"
                  textAlign="left"
                  color="black"
                  sx={(theme) => ({
                    WebkitTextStrokeColor: theme.palette.background.white,
                    WebkitTextStrokeWidth: '1px',
                  })}
                >
                  Your next adventure awaits...
                </Typography>
              </Box>
            </>
          )}
          <Box sx={{ minWidth: '300px' }}>
            <AuthForm
              type={mode}
              onSubmit={handleSubmit}
              loading={loading}
              error={error}
              toggleType={toggleMode}
            />
          </Box>
        </Stack>
      </Box>
    </Box>
  );
};

export default AuthPage;
