import {
  Box,
  Button,
  Link,
  Paper,
  Stack,
  TextField,
  Typography,
  CircularProgress,
  Alert
} from "@mui/material";
import { useState } from "react";

const AuthForm = ({ toggleType, type, onSubmit, loading, error }) => {
  const [fieldErrors, setFieldErrors] = useState({});
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
    userName: '',
    country: '',
    preferredLanguage: ''
  });

  const handleChange = (e) => {
    const { id, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [id]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit?.(formData);
  };

  return (
    <Paper
      elevation={20}
      sx={{
        width: "100%",
        maxWidth: { xs: 350, sm: 450, md: 500 },
        borderRadius: "14px",
        backgroundColor: "background.white",
        position: 'relative',
        border: 1,
        borderColor: "divider",
        px: { xs: 3, sm: 6, md: 8 },
        py: { xs: 4, sm: 5, md: 6 },
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
              sx={{ textAlign: "left" }}
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
          {typeof error === "string" && error}

          {typeof error === "object" && (
            <>
              <div>{error.title || "Something went wrong"}</div>
              {error.errors &&
                Object.entries(error.errors).map(([field, messages]) =>
                  messages.map((msg, idx) => (
                    <div key={`${field}-${idx}`}>
                      {msg}
                    </div>
                  ))
                )}
            </>
          )}
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
              autoComplete="email"
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
              autoComplete={type === 'login' ? 'current-password' : 'new-password'}
              required
              {...(type !== 'login' && {
                helperText: 'Password must be at least 6 characters',
                inputProps: { minLength: 6 }
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
                id="confirmPassword"
                label="Confirm Password"
                type="password"
                fullWidth
                value={formData.confirmPassword}
                onChange={handleChange}
                variant="outlined"
                autoComplete='new-password'
                required
              />
              <TextField
                id="firstName"
                label="First Name"
                fullWidth
                value={formData.firstName}
                onChange={handleChange}
                variant="outlined"
                type="text"
                autoComplete="given-name"
                required
              />
              <TextField
                id="lastName"
                label="Last Name"
                fullWidth
                value={formData.lastName}
                onChange={handleChange}
                variant="outlined"
                autoComplete="family-name"
                required
              />
              <TextField
                id="userName"
                label="UserName"
                fullWidth
                value={formData.userName}
                onChange={handleChange}
                variant="outlined"
                autoComplete="username"
                required
              />
              <TextField
                id="country"
                label="Country"
                fullWidth
                value={formData.country}
                onChange={handleChange}
                variant="outlined"
                autoComplete="country"
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
            onClick={(e) => {
              e.preventDefault();
              console.log('Switching to register');
              toggleType();
            }}
          >
            Register an Account
          </Link>
        </>) : (
          <>
            Already have an account?{" "}
            <Link
              href="#"
              onClick={(e) => {
                e.preventDefault();
                console.log('Switching to login');
                toggleType();
              }}

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
