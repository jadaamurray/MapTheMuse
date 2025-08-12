import { useState } from 'react'
import './App.css'
import { Box } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from './theme';
import HeaderNav from './components/Header/Header'
import Footer from './components/Footer/Footer';
import AuthPage from './pages/AuthPage';
import { Routes, Route} from 'react-router-dom';
import Homepage from './pages/Homepage';

function App() {

  return (
    <ThemeProvider>
      <CssBaseline />
      {/* header/nav bar */}
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          minHeight: '100vh',      // stretch to fill screen
          width: '100%',
          margin:0,
          //p: 2,
        }}
      >
        <HeaderNav />
        {/* main content area */}
        <main>
          <Box
            component="main"
            sx={{
              flex: 1,           // push footer to bottom
              px: 2, py: 4,         // padding
            }}
          >
            < Routes >
            <Route path='/homepage' element={<Homepage />} />
            <Route path="/auth" element={<AuthPage />} />
            </Routes>
          </Box>
        </main>
        {/* footer */}
        <Footer />
      </Box>
    </ThemeProvider>
  );
}

export default App
