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

const AuthForm = ({ type, onSubmit, loading, error }) => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    ...(type === 'register' && {
      fName: '',
      lName: '',
      username: '',
      country: '',
      preferredLanguage: ''
    })
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Handle login logic here
    // probably onSubmit(formData); and then call the API
  };

  return (
    <Paper
      elevation={10}
      sx={{
        width: 500,
        borderRadius: "14px",
        backgroundColor: "background.white",
        position: 'relative',
        border: 1,
        borderColor: "primary.main",
        px: 8,
        py: 6,
      }}
    >
      <Box
        onSubmit={handleSubmit}
        component="form"
        sx={{ mb: 2.5, backgroundColor: "background.white" }}
      >
        {type === 'login' && (
          <>
            <Typography
              variant="h2"
              sx={{ textAlign: "left", }}
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
          </>
        )}
      </Box>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{
          mt: 2,
          alignItems: "center",
          px: 5,
          py: 8.3,
        }}
      >
        <Stack spacing={4}>
          <Box>
            <TextField
            label="Email"
              id="email"
              type="email"
              fullWidth
              value={formData.email}
              onChange={handleChange}
              variant="outlined"
              required
            />
          </Box>

          <Box>
            <TextField
            label="Password"
              id="password"
              type="password"
              fullWidth
              value={formData.password}
              onChange={handleChange}
              variant="outlined"
              required
              {...(type !== 'login' && {
                        helperText: 'Password must be at least 6 characters'
                    })}
            />
          </Box>

          {type === 'login' ? (
            <>
              <Link
                href="#"
                align="right"
              >
                Forgot password?
              </Link>
            </>
          ) : (
            <>
              <TextField
                id="fName"
                label="First Name"
                fullWidth
                value={formData.fName}
                onChange={handleChange}
                variant="outlined"
                type="text"
                required
              />
              <TextField
                id="lName"
                label="Last Name"
                fullWidth
                value={formData.lName}
                onChange={handleChange}
                variant="outlined"
                required
              />
              <TextField
                id="username"
                label="Username"
                fullWidth
                value={formData.username}
                onChange={handleChange}
                variant="outlined"
                required
              />
              <TextField
                id="country"
                label="Country"
                fullWidth
                value={formData.country}
                onChange={handleChange}
                variant="outlined"
                required
              />
              <TextField
                id="preferredLanguage"
                label="Preferred Language"
                fullWidth
                value={formData.preferredLanguage}
                onChange={handleChange}
                variant="outlined"
                required
              />
            </>
          )}

          <Button
            type="submit"
            variant="contained"
            disabled={loading}
            fullWidth
          >
            {loading ? <CircularProgress size={24} /> : type === 'login' ? 'Sign In' : 'Register'}
          </Button>
        </Stack>
      </Box>

      <Box sx={{ mt: 2 }} >
        {type === 'login' ? (<>
          New here?{" "}
          <Link
            href="#"
          >
            Register an Account
          </Link>
        </>) : (
          <>
            Already have an account?{" "}
            <Link
              href="#"
            >
              Sign In
            </Link>
          </>
        )}
      </Box>
    </Paper>
  );
};

export default AuthForm;
