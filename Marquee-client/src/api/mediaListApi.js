import { apiRequest } from "./client";

export function getMyLists() {
    return apiRequest("/api/lists");
}

export function getListById(id) {
    return apiRequest(`/api/lists/${id}`);
}

export function createList(data) {
    return apiRequest("/api/lists", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateList(id, data) {
    return apiRequest(`/api/lists/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteList(id) {
    return apiRequest(`/api/lists/${id}`, {
        method: "DELETE",
    });
}

export function addMediaToList(listId, mediaId) {
    return apiRequest(`/api/lists/${listId}/items`, {
        method: "POST",
        body: JSON.stringify({
            mediaId,
        }),
    });
}

export function removeMediaFromList(listId, mediaId) {
    return apiRequest(`/api/lists/${listId}/items/${mediaId}`, {
        method: "DELETE",
    });
}