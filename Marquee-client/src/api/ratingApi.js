import { apiRequest } from "./client";

export async function getMyRating(mediaId) {
    return apiRequest(
        `/api/media/${mediaId}/rating`
    );
}

export async function setRating(
    mediaId,
    value
) {
    return apiRequest(
        `/api/media/${mediaId}/rating`,
        {
            method: "PUT",
            body: JSON.stringify({
                value
            })
        }
    );
}

export async function deleteRating(mediaId) {
    return apiRequest(
        `/api/media/${mediaId}/rating`,
        {
            method: "DELETE"
        }
    );
}