import { lazy } from "react";
import { Navigate } from "react-router-dom";
import { useAuthContext } from "./features/auth/context/AuthContext";

// Lazy-loaded pages for performance
const Homepage = lazy(() => import("./pages/Homepage"));
const DestinationsPage = lazy(() =>
    import("./features/destinations/pages/DestinationsPage")
);
const DetailDestinationPage = lazy(() =>
    import("./features/destinations/pages/DetailDestinationPage")
);
const AuthPage = lazy(() => import("./features/auth/pages/AuthPage"));
const ProfilePage = lazy(() =>
    import("./features/profile/pages/ProfilePage")
);
const EditProfilePage = lazy(() =>
    import("./features/profile/pages/EditProfilePage")
);
const MediaPage = lazy(() => import("./features/media/pages/MediaPage"));

/**
 * Wrapper for protecting routes.
 * If no user, redirects to /login
 */
function RequireAuth({ children }) {
    const { user } = useAuthContext();
    if (!user) {
        return <Navigate to="/login" replace />;
    }
    return children;
}

export const routes = [
    { path: "/", element: <Homepage /> },
    { path: "/login", element: <AuthPage /> },
    { path: "/register", element: <AuthPage /> },

    {
        path: "/destinations",
        children: [
            { index: true, element: <DestinationsPage /> },
            { path: ":id", element: <DetailDestinationPage /> },
        ],
    },

    {
        path: "/profile",
        element: (
            <RequireAuth>
                <ProfilePage />
            </RequireAuth>
        ),
    },
    {
        path: "/profile/edit",
        element: (
            <RequireAuth>
                <EditProfilePage />
            </RequireAuth>
        ),
    },
    {
        path: "/admin",
        element: (
            <RequireAuth>
                <div>Coming Soon...</div>
            </RequireAuth>
        ),
    },

    { path: "/media", element: <MediaPage /> },
    { path: "/about", element: <div>Coming Soon...</div> },
    { path: "/trips", element: <div>Coming Soon...</div> },
    { path: "*", element: <div>Not found</div> },
];