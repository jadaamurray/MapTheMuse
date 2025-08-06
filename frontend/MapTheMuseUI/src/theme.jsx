import {
  CssBaseline,
  ThemeProvider as MuiThemeProvider,
  createTheme,
} from "@mui/material";
import React from "react";

// base theme
let theme = createTheme({
  palette: {
    primary:   { main: "rgba(16, 55, 74, 1)" },   // textnavy
    background:{ default: "rgba(252,248,239,1)", paper: "rgba(255,254,251,1)" },
    text:      { primary: "rgba(16,55,74,1)", secondary: "#000000" },
    // custom palette keys
    flightBoard: { main: "rgba(13,17,23,1)" },
    boarding:    { main: "rgba(28,184,17,1)" },
  },
  typography: {
    fontFamily: "'Outfit', Helvetica",
    h1: { fontSize: "64px", fontWeight: 700 },
    h2: { fontSize: "36px", fontWeight: 500 },
    h3: { fontSize: "28px", fontWeight: 500 },
  },
  components: {
    MuiButton: {
      styleOverrides: { root: { textTransform: "none" } },
    },
    MuiCssBaseline: {
      styleOverrides: {
        body: { backgroundColor: "rgba(252,248,239,1)" },
      },
    },
    MuiTableCell: {
      styleOverrides: {
        root: ({ theme }) => ({ ...theme.typography.body1 }),
        head: ({ theme }) => ({ ...theme.typography.h3 }),
        body: ({ theme }) => ({ ...theme.typography.body1 }),
      },
    },
    MuiListItemText: {
      styleOverrides: {
        primary:   ({ theme }) => ({ ...theme.typography.h3 }),
        secondary: ({ theme }) => ({ ...theme.typography.body1 }),
      },
    },
  },
});

// merge in  custom shadows 
theme = createTheme(theme, {
  shadows: [
    ...theme.shadows,
    // custom glow effects
    "0px 4px 5px 0px rgba(255,255,255,0.11),0px -4px 5px 0px rgba(255,255,255,0.11)",
    "0px 4px 4px 0px rgba(255,255,255,0.16),0px -4px 5px 0px rgba(255,255,255,0.15)",
  ],
});

export function ThemeProvider({ children }) {
  return (
    <MuiThemeProvider theme={theme}>
      <CssBaseline />
      {children}
    </MuiThemeProvider>
  );
}

export default theme;
