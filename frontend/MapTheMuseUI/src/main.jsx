import { StrictMode, useEffect, Suspense } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { AuthProvider } from "./context/AuthProvider";
import { DestinationsProvider } from "./context/DestinationsProvider";
import { FavouritesProvider } from "./context/FavouritesProvider";

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