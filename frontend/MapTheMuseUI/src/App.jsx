import { useState } from 'react'
import './App.css'
import { Box } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from './theme';
import HeaderNav from './components/Header/Header'
import Footer from './components/Footer/Footer';
import AuthPage from './pages/SignIn/AuthPage';

function App() {

  return (
    <ThemeProvider>
      {/* Normalise browser styles and apply theme’s global overrides */}
      <CssBaseline />
      {/* header/nav bar */}
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          minHeight: '100vh',      // stretch to fill screen
          p: 2, // padding around content
        }}
      >
        <HeaderNav />
        {/* main content area */}
        <main>
          <Box
            component="main"
            sx={{
              flexGrow: 1,           // push footer to bottom
              px: 2, py: 4,         // padding
            }}
          >
            <AuthPage />
          </Box>
        </main>
        {/* footer */}
        <Footer />
      </Box>
    </ThemeProvider>
  );
}

export default App
