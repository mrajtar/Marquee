import { Link } from "react-router-dom";
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
            <Link
                to={`/media/${review.mediaId}`}
                className="review-media"
            >
                <div className="review-media-poster">
                    {review.mediaPosterUrl ? (
                        <img
                            src={review.mediaPosterUrl}
                            alt={review.mediaTitle}
                            loading="lazy"
                        />
                    ) : (
                        <div className="review-media-placeholder">
                            No poster
                        </div>
                    )}
                </div>

                <strong>{review.mediaTitle}</strong>
            </Link>

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
    return new Date(value).toLocaleDateString(
        undefined,
        {
            day: "numeric",
            month: "short",
            year: "numeric"
        }
    );
}

export default ReviewCard;