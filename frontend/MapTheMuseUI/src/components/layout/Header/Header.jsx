import React, { useState } from "react";
import { Box, AppBar, Toolbar, Typography, Stack, IconButton, Drawer, Divider } from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
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
  const navItems = ALL_ITEMS.filter(i => NAV_BY_ROLE[role].includes(i.id));

  const [mobileOpen, setMobileOpen] = useState(false);
  const handleMobileToggle = () => setMobileOpen(p => !p);

  const handleClick = (item) => {
    if (item.id === "logout") {
      logout();
      navigate("/login");
    } else {
      navigate(item.path);
    }
    setMobileOpen(false); // close drawer on navigate
  };

  return (
    <AppBar
      position="sticky"
      elevation={0}
      sx={{
        height: { xs: 56, sm: 60 },
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
          gap: { md: "50px", xs: 2 },
          width: "100%",
          justifyContent: "space-between",
        }}
      >
        {/* Mobile menu button */}
        <IconButton
          onClick={handleMobileToggle}
          edge="start"
          sx={{ display: { xs: "inline-flex", md: "none" }, mr: 1 }}
          aria-label="open navigation menu"
        >
          <MenuIcon />
        </IconButton>

        {/* Brand */}
        <Typography
          variant="h5" // a bit smaller on mobile
          onClick={() => navigate("/")}
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

        {/* Desktop nav (md and up) */}
        <Box
          component="nav"
          sx={{
            display: { xs: "none", md: "flex" },
            gap: 2,
            alignItems: "center",
            height: 64,
            px: 4,
          }}
        >
          <Stack direction="row" spacing={4} alignItems="center" justifyContent="space-between" sx={{ flexGrow: 1 }}>
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

        {/* Mobile Drawer */}
        <Drawer
          anchor="left"
          open={mobileOpen}
          onClose={handleMobileToggle}
          ModalProps={{ keepMounted: true }} // better perf on mobile
          sx={{
            display: { xs: "block", md: "none" },
            "& .MuiDrawer-paper": { width: 280, p: 2 },
          }}
        >
          <Typography variant="h6" sx={{ fontWeight: 700, px: 1, mb: 1 }} onClick={() => navigate("/")}>
            Map The Muse
          </Typography>
          <Divider sx={{ mb: 1 }} />
          <Stack spacing={0.5}>
            {navItems.map((item) => (
              <HeaderButton
                key={`m-${item.id}`}
                label={item.label}
                active={pathname === item.path}
                onClick={() => handleClick(item)}
                color={item.id === "logout" ? "error" : "primary"}
              />
            ))}
          </Stack>
        </Drawer>
      </Toolbar>
    </AppBar>
  );
}
