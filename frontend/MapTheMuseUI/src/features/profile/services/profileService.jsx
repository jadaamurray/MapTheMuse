import apiClient from "../../../api/apiClient";

export const profileService = {
  async update(ops) {
    return apiClient.patch("/me", ops, {
      headers: { "Content-Type": "application/json-patch+json" },
    });
  },
  changeUserName: (newUserName) => apiClient.post("/me/username", { newUserName }).then(res => res.data),
  changePassword: (currentPassword, newPassword) => apiClient.post("/me/password", { currentPassword, newPassword }).then(() => { }),
  startEmailChange: (newEmail) => apiClient.post("/me/email", { newEmail }).then(() => { }),
  confirmEmailChange: (userId, email, token) => apiClient.post("/me/email/confirm", { userId, email, token }).then(() => { })
};