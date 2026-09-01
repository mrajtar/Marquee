import { useState } from "react";
import { FiMessageSquare } from "react-icons/fi";

import { createReview } from "../../api/reviewApi";
import { useAuth } from "../../context/AuthContext";

function ReviewForm({ mediaId, onReviewCreated }) {
    const { isAuthenticated } = useAuth();

    const [content, setContent] = useState("");
    const [containsSpoilers, setContainsSpoilers] =
        useState(false);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    if (!isAuthenticated) {
        return (
            <div className="review-login-message">
                <FiMessageSquare size={18} />

                <span>
                    Log in to write a review.
                </span>
            </div>
        );
    }

    async function handleSubmit(event) {
        event.preventDefault();

        if (!content.trim()) {
            setError("Review cannot be empty.");
            return;
        }

        setError(null);
        setLoading(true);

        try {
            const review = await createReview(
                mediaId,
                content.trim(),
                containsSpoilers
            );

            setContent("");
            setContainsSpoilers(false);

            onReviewCreated?.(review);
        } catch (error) {
            console.error(
                "Failed to create review:",
                error
            );

            setError(
                "Failed to post review. Please try again."
            );
        } finally {
            setLoading(false);
        }
    }

    return (
        <form
            className="review-form"
            onSubmit={handleSubmit}
        >
            <textarea
                value={content}
                onChange={(event) =>
                    setContent(event.target.value)
                }
                placeholder="Write your review..."
                maxLength={10000}
                disabled={loading}
            />

            <div className="review-form-footer">
                <label className="spoiler-checkbox">
                    <input
                        type="checkbox"
                        checked={containsSpoilers}
                        onChange={(event) =>
                            setContainsSpoilers(
                                event.target.checked
                            )
                        }
                        disabled={loading}
                    />

                    <span>
                        Contains spoilers
                    </span>
                </label>

                <button
                    type="submit"
                    className="primary-button"
                    disabled={
                        loading ||
                        !content.trim()
                    }
                >
                    {loading
                        ? "Posting..."
                        : "Post review"}
                </button>
            </div>

            {error && (
                <p className="form-error">
                    {error}
                </p>
            )}
        </form>
    );
}

export default ReviewForm;