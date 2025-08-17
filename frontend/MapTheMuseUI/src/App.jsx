import { useState } from 'react'
import './App.css';
import { Box } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from './theme';
import HeaderNav from './components/layout/Header/Header'
import Footer from './components/layout/Footer/Footer';
import AuthPage from './features/auth/pages/AuthPage';
import { Routes, Route } from 'react-router-dom';
import Homepage from './pages/Homepage';
import DestinationsPage from './features/destinations/pages/DestinationsPage';
import DetailDestinationPage from './features/destinations/pages/DetailDestinationPage';
import ProfilePage from './features/profile/pages/ProfilePage';
import MediaPage from './features/media/pages/MediaPage';

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
          margin: 0,
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
              <Route path="/login" element={<AuthPage />} />
              <Route path="/register" element={<AuthPage />} />

              {/* Destinations routes */}
              <Route path="/destinations">
                {/* List page: /destinations */}
                <Route index element={<DestinationsPage />} />
                {/* Detail page: /destinations/:id */}
                <Route path=":id" element={<DetailDestinationPage />} />
              </Route>
              {/* Profile routes */}
              <Route path='/profile' element={<ProfilePage />} />
              {/* Media routes */}
              <Route path='/media' element={<MediaPage />} />
              <Route path="*" element={<div>Not found</div>} />


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
