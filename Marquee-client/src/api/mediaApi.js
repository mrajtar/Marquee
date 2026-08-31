import { apiRequest } from "./client";

export async function getMedia() {
    return apiRequest("/api/media");
}

export async function getMediaById(id) {
    return apiRequest(`/api/media/${id}`);
}

export async function getMediaDetails(id) {
    return apiRequest(
        `/api/media/${id}/details`
    );
}

export async function searchMedia(searchTerm) {
    return apiRequest(
        `/api/media/search?searchTerm=${encodeURIComponent(searchTerm)}`
    );
}