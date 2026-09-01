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
export async function createReview(
    mediaId,
    content,
    containsSpoilers
) {
    return apiRequest(
        `/api/media/${mediaId}/reviews`,
        {
            method: "POST",
            body: JSON.stringify({
                content,
                containsSpoilers
            })
        }
    );
}
export async function getReviewById(reviewId) {
    return apiRequest(`/api/reviews/${reviewId}`);
}