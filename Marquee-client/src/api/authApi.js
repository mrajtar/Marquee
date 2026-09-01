import { apiRequest } from "./client";

export async function loginUser(
    username,
    password
) {
    return apiRequest(
        "/api/auth/login",
        {
            method: "POST",
            body: JSON.stringify({
                username,
                password
            })
        }
    );
}

export async function registerUser(
    username,
    email,
    password
) {
    return apiRequest(
        "/api/auth/register",
        {
            method: "POST",
            body: JSON.stringify({
                username,
                email,
                password
            })
        }
    );
}