import { useEffect, useMemo, useState } from "react";
import {
    Box, Stack, TextField, Button, Alert, CircularProgress, Divider,
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ProfileHeader from "../components/ProfileHeader";
import { useNavigate } from "react-router-dom";
import { useProfile } from "../hooks/useProfile";
import { buildProfilePatch } from "../utils/buildProfilePatch";

const USERNAME_RE = /^(?![.])(?!.*[._]{2})[a-zA-Z0-9._]{3,32}(?<![.])$/;

export default function EditProfilePage() {
    const {
        user,
        loading,
        error,
        clearError,
        update,
        changeUserName,
        startEmailChange,
    } = useProfile();

    const navigate = useNavigate();

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        userName: "",
        country: "",
        preferredLanguage: "",
        profilePictureUrl: "",
        email: "",
    });

    const [saving, setSaving] = useState(false);
    const [msg, setMsg] = useState(null);   // success message
    const [localErr, setLocalErr] = useState(null); // client-side validation message

    useEffect(() => {
        if (user) {
            setForm({
                firstName: user.firstName || "",
                lastName: user.lastName || "",
                userName: user.userName || "",
                country: user.country || "",
                preferredLanguage: user.preferredLanguage || "",
                profilePictureUrl: user.profilePictureUrl || "",
                email: user.email || "",
            });
        }
    }, [user]);

    const actions = useMemo(
        () => (
            <Button
                variant="outlined"
                startIcon={<ArrowBackIcon />}
                sx={{ borderRadius: 3 }}
                onClick={() => navigate("/profile")}
            >
                Back to profile
            </Button>
        ),
        [navigate]
    );

    const onChange = (e) => {
        setLocalErr(null);
        if (error) clearError?.();
        setForm((f) => ({ ...f, [e.target.name]: e.target.value }));
    };

    const usernameInvalid =
        form.userName && !USERNAME_RE.test(form.userName);

    const onSave = async (e) => {
        e.preventDefault();
        setLocalErr(null);
        setMsg(null);
        if (error) clearError?.();

        // Client-side username check (server still enforces)
        if (usernameInvalid) {
            setLocalErr(
                "Username must be 3–32 chars, letters/numbers/._ only, no leading/trailing . or _ and no doubles."
            );
            return;
        }

        const trim = (v) => (typeof v === "string" ? v.trim() : v);

        // Basics - build a PATCH payload for basics
        const basicsOps = buildProfilePatch(user, {
            firstName: form.firstName,
            lastName: form.lastName,
            country: form.country,
            preferredLanguage: form.preferredLanguage,
            profilePictureUrl: form.profilePictureUrl,
        });


        // Username (lowercased)
        const desiredUserName = trim(form.userName).toLowerCase();
        const usernameChanged =
            desiredUserName &&
            desiredUserName.toLowerCase() !== (user?.userName || "").toLowerCase();

        // Email
        const newEmail = trim(form.email);
        const emailChanged =
            newEmail && newEmail.toLowerCase() !== (user?.email || "").toLowerCase();

        // If nothing changed, return
        if (basicsOps.length === 0 && !usernameChanged && !emailChanged) {
            setMsg("No changes to save.");
            return;
        }

        setSaving(true);
        try {
            // Only call the endpoints that correspond to changes
            if (basicsOps.length > 0) {
                await update(basicsOps);
            }

            if (usernameChanged) {
                console.log('sending username change to useProfile: ', desiredUserName);
                await changeUserName(desiredUserName);
            }

            if (emailChanged) {
                await startEmailChange(newEmail);
                setMsg("We sent a confirmation link to your new email.");
            } else if (Object.keys(basicsOps).length > 0 || usernameChanged) {
                setMsg("Profile updated.");
            }

            navigate("/profile");
        } catch {
            // Hook sets ui friendly error already, keep user on the page
        } finally {
            setSaving(false);
        }
    };

    if (!user || loading) {
        return (
            <Stack alignItems="center" sx={{ py: 8 }}>
                <CircularProgress />
            </Stack>
        );
    }

    return (
        <Box sx={{ pb: 6, height: "100%" }}>
            <ProfileHeader user={user} actions={actions} />

            <Box sx={{ maxWidth: 720, mx: "auto", px: 3 }}>
                <form onSubmit={onSave}>
                    <Stack spacing={2.5}>
                        {localErr && (
                            <Alert severity="error" onClose={() => setLocalErr(null)}>
                                {localErr}
                            </Alert>
                        )}
                        {error && (
                            <Alert severity="error" onClose={clearError}>
                                {String(error)}
                            </Alert>
                        )}
                        {msg && (
                            <Alert severity="success" onClose={() => setMsg(null)}>
                                {msg}
                            </Alert>
                        )}

                        <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                            <TextField
                                name="firstName"
                                label="First name"
                                value={form.firstName}
                                onChange={onChange}
                                fullWidth
                                disabled={saving}
                            />
                            <TextField
                                name="lastName"
                                label="Last name"
                                value={form.lastName}
                                onChange={onChange}
                                fullWidth
                                disabled={saving}
                            />
                        </Stack>

                        <TextField
                            name="userName"
                            label="Username"
                            value={form.userName}
                            onChange={onChange}
                            helperText={
                                usernameInvalid
                                    ? "Invalid username format."
                                    : "Letters, numbers, . or _. 3–32 chars. We’ll store it lowercase."
                            }
                            error={Boolean(usernameInvalid)}
                            inputProps={{ maxLength: 32 }}
                            fullWidth
                            disabled={saving}
                        />

                        <TextField
                            name="email"
                            label="Email"
                            type="email"
                            value={form.email}
                            onChange={onChange}
                            helperText="Changing your email sends a confirmation link."
                            fullWidth
                            disabled={saving}
                        />

                        <Divider />

                        <TextField
                            name="country"
                            label="Country"
                            value={form.country}
                            onChange={onChange}
                            fullWidth
                            disabled={saving}
                        />

                        <TextField
                            name="preferredLanguage"
                            label="Preferred language"
                            value={form.preferredLanguage}
                            onChange={onChange}
                            fullWidth
                            disabled={saving}
                        />

                        <TextField
                            name="profilePictureUrl"
                            label="Profile picture URL"
                            value={form.profilePictureUrl}
                            onChange={onChange}
                            fullWidth
                            disabled={saving}
                        />

                        <Stack direction="row" spacing={1.5} justifyContent="flex-end" sx={{ pt: 1 }}>
                            <Button variant="outlined" onClick={() => navigate("/profile")} disabled={saving}>
                                Cancel
                            </Button>
                            <Button type="submit" variant="contained" disabled={saving}>
                                {saving ? "Saving…" : "Save changes"}
                            </Button>
                        </Stack>
                    </Stack>
                </form>
            </Box>
        </Box>
    );
}
