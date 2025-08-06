import React, { useState } from 'react';
import { Box, AppBar, Toolbar, Typography, Stack } from '@mui/material';
import HeaderButton from './HeaderButton';

const navItems = [
    { id: 1, label: 'About' },
    { id: 2, label: 'Destinations' },
    { id: 3, label: 'My Trips' },
    { id: 4, label: 'Log in', disabled: false },
];

export default function HeaderNav() {
    const [activeId, setActiveId] = useState(1);

    return (
        <AppBar
            position="sticky"
            elevation={0}
            sx={{
                height: "85px",
                bgcolor: "background.default",
                color: "text.primary",
                px: 2,
                display: "flex",
                alignItems: "center",
            }}
        >
            <Toolbar sx={{ height: "100%", gap: "50px" }}>
                <Typography
                    variant="h1"
                    component="div"
                    color="primary"
                    sx={{ width: "500px" }}
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
                                active={activeId === item.id}
                                disabled={item.disabled}
                                onClick={() => setActiveId(item.id)}
                            />
                        ))}
                    </Stack>
                </Box>
            </Toolbar>
        </AppBar>

    );
}
