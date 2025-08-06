
import {
  Box,
  Button,
  Link,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import React from "react";
import SignInForm from "./SignInForm";
import BlurCircleBackground from "../../components/ui/BlurCircleBackground";

const SignInPage = () => {
  return (
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
        px: 30, py: 30, 
        alignItems: "center" }}>
        {/* Background elements */}
        < BlurCircleBackground />
        <SignInForm />
      </Box>
    </Box>
  );
};

export default SignInPage;


