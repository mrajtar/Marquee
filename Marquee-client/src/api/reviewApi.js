import { apiRequest } from "./client";

export async function getRecentReviews(count = 10) {
    return apiRequest(
        `/api/reviews/recent?count=${count}`
    );
}

export async function getReviewsByMediaId(mediaId) {
    return apiRequest(
        `/api/media/${mediaId}/reviews`
    );
}