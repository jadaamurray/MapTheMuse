import { StrictMode, useEffect, Suspense } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { AuthProvider } from "./features/auth/context/AuthContext";
import { DestinationsProvider } from "./features/destinations/context/DestinationsContext";
import { FavouritesProvider } from "./features/favourites/context/FavouritesContext";

function Boot() {
  useEffect(() => {
    window.__removeBootSplash?.(); // remove the splash as soon as React mounts
  }, []);

  return (
    <BrowserRouter>
      <AuthProvider>
        <DestinationsProvider autoload={true}>
          <FavouritesProvider>
            {/* keep this Suspense tiny; add route/section Suspense inside App */}
            <Suspense fallback={null}>
              <App />
            </Suspense>
          </FavouritesProvider>
        </DestinationsProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <Boot />
  </StrictMode>
);