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

export async function getTrendingMedia(count = 20) {
    return apiRequest(`/api/media/trending?count=${count}`);
}

export async function getFeaturedMedia() {
    return apiRequest(`/api/media/featured`);
}

export async function updateMedia(id, data) {
    return apiRequest(`/api/media/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}
export async function getGenres() {
    return apiRequest("/api/genres");
}

export async function getKeywords() {
    return apiRequest("/api/keywords");
}