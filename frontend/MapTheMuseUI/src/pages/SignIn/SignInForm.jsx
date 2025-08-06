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

const SignInForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    // Handle login logic here
  };

  return (
    <Paper
      elevation={6}
      sx={{
        width: 412,
        borderRadius: "14px",
        padding: "17px 23px",
        backgroundColor: "background.white",
        position: 'relative',
        border: 1,
        borderColor: "border.default",
      }}
    >
      <Box sx={{ mb: 2.5, backgroundColor: "background.white" }}>
        <Typography
          variant="h2"
          sx={{textAlign: "left",}}
        >
          Hello!
        </Typography>
        <Typography
          variant="body2"
          sx={{
            fontSize: "18px",
            letterSpacing: "-0.36px",
            textAlign: "left",
          }}
        >
          Sign in to get started
        </Typography>
      </Box>

      <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2, alignItems: "center" }}>
        <Stack spacing={3}>
          <Box>
            <Typography
              component="label"
              htmlFor="email"
              sx={{
                display: "block",
                mb: 1,
                fontFamily: "Inter, Helvetica",
                fontSize: "16px",
                fontWeight: 400,
                textAlign: "left",
              }}
            >
              Email
            </Typography>
            <TextField
              id="email"
              fullWidth
              value={email}
              onChange={handleEmailChange}
              variant="outlined"
              sx={{
                "& .MuiOutlinedInput-root": {
                  borderRadius: "8px",
                  backgroundColor: "common.white",                },
              }}
            />
          </Box>

          <Box>
            <Typography
              component="label"
              htmlFor="password"
              sx={{
                display: "block",
                mb: 1,
                fontFamily: "Inter, Helvetica",
                fontSize: "16px",
                fontWeight: 400,
                textAlign: "left",
              }}
            >
              Password
            </Typography>
            <TextField
              id="password"
              type="password"
              fullWidth
              value={password}
              onChange={handlePasswordChange}
              variant="outlined"
              sx={{
                "& .MuiOutlinedInput-root": {
                  borderRadius: "8px",
                  backgroundColor: "common.white",
                },
              }}
            />
          </Box>

          <Button
            type="submit"
            variant="contained"
            fullWidth
            sx={{
              backgroundColor: "primary.main",
              color: "text.onBrand",
              borderRadius: "8px",
              border: 1,
              borderColor: "primary.main",
              py: 1.5,
            }}
          >
            Sign In
          </Button>

          <Box>
            <Link
              href="#"
              underline="always"
              sx={{
                color: "text.primary",
                fontFamily: "Inter, Helvetica",
                fontSize: "16px",
                fontWeight: 400,
              }}
            >
              Forgot password?
            </Link>
          </Box>
        </Stack>
      </Box>

      <Box sx={{ mt: 2 }}>
        <Typography
          component="span"
          sx={{
            color: "#1e1e1e",
            fontFamily: "Inter, Helvetica",
            fontSize: "16px",
            fontWeight: 400,
          }}
        >
          New here?{" "}
          <Link
            href="#"
            underline="always"
            sx={{
              fontFamily: "Inter, Helvetica",
              fontSize: "16px",
              fontWeight: 400,
            }}
          >
            Register an Account
          </Link>
        </Typography>
      </Box>
    </Paper>
  );
};

export default SignInForm;
