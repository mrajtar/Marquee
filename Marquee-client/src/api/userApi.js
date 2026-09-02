import { apiRequest } from "./client";

export function getMe() {
    return apiRequest("/api/users/me");
}

export function updateMe(data) {
    return apiRequest("/api/users/me", {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteMe() {
    return apiRequest("/api/users/me", {
        method: "DELETE",
    });
}