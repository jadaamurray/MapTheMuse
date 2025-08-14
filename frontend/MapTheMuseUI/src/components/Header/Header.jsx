import React, { useState } from 'react';
import { Box, AppBar, Toolbar, Typography, Stack } from '@mui/material';
import HeaderButton from './HeaderButton';
import { Link as RouterLink, useLocation, useNavigate } from "react-router-dom";

const navItems = [
    { id: 1, label: 'About', path: "/about" },
    { id: 2, label: 'Destinations', path: "/destinations" },
    { id: 3, label: 'My Trips', path: "/trips" },
    { id: 4, label: 'Log in', path: "/login", disabled: false },
];

export default function HeaderNav() {
    const navigate = useNavigate();
    const { pathname } = useLocation();

    const handleClick = (item) => {
        console.log("handling click");
        if (item.disabled) return;
        console.log("Click handled. Navigating to ", item.path);
        navigate(item.path);
    };

    return (
        <AppBar
            position="sticky"
            elevation={0}
            sx={{
                height: "85px",
                width: '100%',
                bgcolor: "background.cream",
                color: "text.primary",
                px: 2,
                display: "flex",
                alignItems: "center",
            }}
        >
            <Toolbar
                disableGutters
                sx={{
                    height: "100%",
                    gap: "50px",
                    width: '100%',
                    justifyContent: 'space-between'
                }}
            >
                <Typography
                    variant="h1"
                    onClick={() => navigate('/homepage')} 
                    color="primary"
                    sx={{
                        textAlign: 'left',
                        flexGrow: 1,
                        whiteSpace: 'nowrap',
                        textDecoration: "none",
                        cursor: "pointer"
                    }}
                >
                    Map The Muse
                </Typography>
                <Box
                    component="nav"
                    sx={{
                        display: 'flex',
                        gap: 2,            // spacing between buttons
                        alignItems: 'center',
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
                        {navItems.map(item => (
                            <HeaderButton
                                key={item.id}
                                label={item.label}
                                active={pathname === item.path}
                                disabled={item.disabled}
                                onClick={() => handleClick(item)}
                            />
                        ))}
                    </Stack>
                </Box>
            </Toolbar>
        </AppBar>

    );
}
