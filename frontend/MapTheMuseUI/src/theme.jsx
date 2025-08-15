import React from 'react';
import {
  CssBaseline,
  ThemeProvider as MuiThemeProvider,
  createTheme,
} from '@mui/material';
import { red } from '@mui/material/colors';

// Base theme definition
let theme = createTheme({
  palette: {
    primary: {
      main: 'hsla(200, 64%, 18%, 1.00)',   // textnavy
    },
    secondary: {
      main: 'rgba(186, 104, 115, 1)', // rose
      apricot: 'rgba(250, 198, 104, 1)', // apricot
      burntSienna: 'rgba(204, 85, 0, 1)', // burnt sienna
    },
    error: {
      main: "#f44336", // red
    },
    background: {
      white: 'rgba(255, 254, 251, 1)', // white
      cream: 'rgba(252, 248, 239, 1)', // cream
    },
    text: {
      primary: 'rgba(30, 30, 30, 1)',  // grey900
      secondary: 'rgba(68, 68, 68, 1)',  // grey600
      disabled: 'rgba(179,179,179,1)',  // grey400
    },
    action: { active: 'rgba(16, 55, 74, 1)' },      // textnavy
    success: { main: 'rgba(28, 184, 17, 1)' },       // boarding green
    info: { main: 'rgba(243,212,213,1)' },       // soft pink
    divider: 'rgba(217,217,217,1)',       // grey300
    // custom palette keys
    flightBoard: { main: "rgba(13,17,23,1)" },
    boarding: { main: "rgba(28,184,17,1)" },
  },
  typography: {
    fontFamily: ['Inter', 'Outfit', 'Helvetica', 'sans-serif'].join(','),
    h1: { fontFamily: 'Outfit, Helvetica', fontSize: '64px', fontWeight: 700 },
    h2: { fontFamily: 'Outfit, Helvetica', fontSize: '36px', fontWeight: 500 },
    h3: { fontFamily: 'Outfit, Helvetica', fontSize: '28px', fontWeight: 500 },
    body1: { fontFamily: 'Inter, Helvetica', fontSize: '16px', fontWeight: 400 },
    body2: { fontFamily: 'Inter, Helvetica', fontSize: '18px', fontWeight: 400 },
    subtitle1: { fontFamily: 'Inter, Helvetica', fontSize: '16px', fontWeight: 600 },
    button: { fontFamily: 'Inter, Helvetica', fontSize: '16px', fontWeight: 400 },
  },
  shape: {
    borderRadius: 8,
  },
  // Custom spacing scale, returns number * 8px
  spacing: (factor) => {
    const map = { 2: 1, 3: 1.5, 4: 2, 6: 3, 8: 4 };
    return (map[factor] || factor) * 8;
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
          boxShadow: 'none',
          '&:hover': { boxShadow: 'none' },
        },
        containedPrimary: {
          backgroundColor: 'rgba(16,55,74,1)',
          color: 'rgba(245,245,245,1)',
          '&:hover': { backgroundColor: 'rgba(13,44,59,1)' },
        },
        // override containedSecondary for apricot buttons
        containedSecondary: {
          backgroundColor: 'rgba(250,198,104,1)',
          color: 'rgba(30,30,30,1)',
          '&:hover': { backgroundColor: 'rgba(224,167,57,1)' },
        },
      },
    },
    MuiInputBase: {
      styleOverrides: {
        root: ({ theme }) => ({
          ...theme.typography.body1,
          backgroundColor: '#FFFFFF',
          borderRadius: theme.shape.borderRadius,
          border: `1px solid ${theme.palette.divider}`,
        }),
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          boxShadow: 'none',
          borderRadius: '14px',
        },
      },
    },
    MuiLink: {
      styleOverrides: {
        root: ({ theme }) => ({
          ...theme.typography.body1,
          textDecoration: 'underline',
          cursor: 'pointer',
        }),
      },
    },
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          backgroundColor: 'rgba(255, 254, 251, 1)',
        },
      },
    },
    MuiTableCell: {
      styleOverrides: {
        root: ({ theme }) => ({ ...theme.typography.body1 }),
        head: ({ theme }) => ({ ...theme.typography.subtitle1 }),
      },
    },
    MuiListItemText: {
      styleOverrides: {
        primary: ({ theme }) => ({ ...theme.typography.subtitle1 }),
        secondary: ({ theme }) => ({ ...theme.typography.body1 }),
      },
    },
  },
});

// ThemeProvider wrapper
export const ThemeProvider = ({ children }) => (
  <MuiThemeProvider theme={theme}>
    <CssBaseline />
    {children}
  </MuiThemeProvider>
);
