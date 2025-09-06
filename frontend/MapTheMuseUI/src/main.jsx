import { StrictMode, React } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from './features/auth/context/AuthContext'
import { DestinationsProvider } from './features/destinations/context/DestinationsContext.jsx';
import { FavouritesProvider } from './features/favourites/context/FavouritesContext.jsx';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <DestinationsProvider autoLoad={true}>
          <FavouritesProvider>
          <App />
          </FavouritesProvider>
        </DestinationsProvider>
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>,
)
