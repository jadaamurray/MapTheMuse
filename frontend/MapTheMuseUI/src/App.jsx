import { Suspense } from 'react'
import { useRoutes } from "react-router-dom";
import './App.css';
import { Box } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from './theme';
import HeaderNav from './components/layout/Header/Header'
import Footer from './components/layout/Footer/Footer';
import { routes } from './routes';

function App() {
  const element = useRoutes(routes);

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
            <Suspense fallback={<div>Loading…</div>}>{element}</Suspense>
          </Box>
        </main>
        {/* footer */}
        <Footer />
      </Box>
    </ThemeProvider>
  );
}

export default App;