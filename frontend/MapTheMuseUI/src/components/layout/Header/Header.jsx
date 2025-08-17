import React from "react";
import { Box, AppBar, Toolbar, Typography, Stack } from "@mui/material";
import HeaderButton from "./HeaderButton";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../../features/auth/context/AuthContext";
import useAuth from "../../../features/auth/hooks/useAuth";

const ALL_ITEMS = [
    { id: "about", label: "About", path: "/about" },
    { id: "destinations", label: "Destinations", path: "/destinations" },
    { id: "mytrips", label: "My Trips", path: "/trips" },
    { id: "login", label: "Log in", path: "/login" },
    { id: "profile", label: "Profile", path: "/profile" },
    { id: "admin", label: "Admin", path: "/admin" },
    { id: "logout", label: "Log Out" }
];

const NAV_BY_ROLE = {
    guest: ["about", "destinations", "login"],
    user: ["about", "destinations", "mytrips", "profile", "logout"],
    admin: ["about", "destinations", "mytrips", "profile", "admin", "logout"],
};

export default function HeaderNav() {
    const navigate = useNavigate();
    const { pathname } = useLocation();
    const { user } = useAuthContext();
    const { logout } = useAuth();
    const isAdmin = Array.isArray(user?.roles) && user.roles.includes("Admin");


    const role = isAdmin ? "admin" : user ? "user" : "guest";
    const allowedIds = NAV_BY_ROLE[role];

    const navItems = ALL_ITEMS.filter((item) => allowedIds.includes(item.id));

    const handleClick = (item) => {
        if (item.id === "logout") {
            logout();
            navigate("/login"); // optional redirect
        } else {
            navigate(item.path);
        }
    };

    return (
        <AppBar
            position="sticky"
            elevation={0}
            sx={{
                height: "60px",
                width: "100%",
                bgcolor: "background.white",
                color: "text.primary",
                px: 2,
                display: "flex",
                alignItems: "center",
                borderBottom: 1,
                borderBottomColor: "divider",
            }}
        >
            <Toolbar
                disableGutters
                sx={{
                    height: "100%",
                    gap: "50px",
                    width: "100%",
                    justifyContent: "space-between",
                }}
            >
                <Typography
                    variant="h3"
                    onClick={() => navigate("/homepage")}
                    color="primary"
                    sx={{
                        textAlign: "left",
                        flexGrow: 1,
                        whiteSpace: "nowrap",
                        textDecoration: "none",
                        cursor: "pointer",
                        fontWeight: 700,
                    }}
                >
                    Map The Muse
                </Typography>

                <Box
                    component="nav"
                    sx={{
                        display: "flex",
                        gap: 2,
                        alignItems: "center",
                        height: 64,
                        px: 4,
                    }}
                >
                    <Stack
                        direction="row"
                        spacing={4}
                        alignItems="center"
                        justifyContent="space-between"
                        sx={{ flexGrow: 1 }}
                    >
                        {navItems.map((item) => (
                            <HeaderButton
                                key={item.id}
                                label={item.label}
                                active={pathname === item.path}
                                onClick={() => handleClick(item)}
                                color={item.id === "logout" ? "error" : "primary"}

                            />
                        ))}
                    </Stack>
                </Box>
            </Toolbar>
        </AppBar>
    );
}
