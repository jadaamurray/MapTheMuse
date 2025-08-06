import { useState } from 'react'
import './App.css'
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from './theme';
import HeaderNav from './components/Header/Header'
import Footer from './components/Footer/Footer';

function App() {

  return (
    <ThemeProvider>
      {/* Normalise browser styles and apply theme’s global overrides */}
      <CssBaseline />
      {/* header/nav bar */}
      <HeaderNav />
      {/* main content area */}
      <main>
        {/* Add your main content here */}
      </main> 
      {/* footer */}
      <Footer />
    </ThemeProvider>
  );
}

export default App
