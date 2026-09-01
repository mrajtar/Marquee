import { apiRequest } from "./client";

export async function likeReview(reviewId) {
    return apiRequest(
        `/api/reviews/${reviewId}/like`,
        {
            method: "PUT"
        }
    );
}

export async function unlikeReview(reviewId) {
    return apiRequest(
        `/api/reviews/${reviewId}/like`,
        {
            method: "DELETE"
        }
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