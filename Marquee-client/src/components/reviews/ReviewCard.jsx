import { useState } from "react";
import {
    FiHeart,
    FiEye,
    FiEyeOff
} from "react-icons/fi";

function ReviewCard({ review }) {
    const [showSpoiler, setShowSpoiler] = useState(
        !review.containsSpoilers
    );

    return (
        <article className="review-card">
            <div className="review-card-header">
                <div className="review-author">
                    <div className="review-avatar">
                        {review.profileImageUrl ? (
                            <img
                                src={review.profileImageUrl}
                                alt=""
                            />
                        ) : (
                            <span>
                                {(
                                    review.displayName ||
                                    review.username
                                )
                                    .charAt(0)
                                    .toUpperCase()}
                            </span>
                        )}
                    </div>

                    <div>
                        <strong>
                            {review.displayName ||
                                review.username}
                        </strong>

                        <span>
                            @{review.username}
                        </span>
                    </div>
                </div>

                <time>
                    {formatDate(review.createdAt)}
                </time>
            </div>

            <div className="review-card-content">
                {review.containsSpoilers &&
                !showSpoiler ? (
                    <div className="spoiler-warning">
                        <FiEyeOff size={20} />

                        <span>
                            This review contains spoilers.
                        </span>

                        <button
                            type="button"
                            onClick={() =>
                                setShowSpoiler(true)
                            }
                        >
                            <FiEye size={16} />
                            Reveal review
                        </button>
                    </div>
                ) : (
                    <p>{review.content}</p>
                )}
            </div>

            <div className="review-card-footer">
                <button
                    type="button"
                    className={
                        review.likedByCurrentUser
                            ? "review-like liked"
                            : "review-like"
                    }
                >
                    <FiHeart
                        size={17}
                        fill={
                            review.likedByCurrentUser
                                ? "currentColor"
                                : "none"
                        }
                    />

                    <span>{review.likeCount}</span>
                </button>
            </div>
        </article>
    );
}

function formatDate(value) {
    const date = new Date(value);

    return date.toLocaleDateString(
        undefined,
        {
            day: "numeric",
            month: "short",
            year: "numeric"
        }
    );
}

export default ReviewCard;