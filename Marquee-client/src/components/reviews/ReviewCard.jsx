import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import {
    FiHeart,
    FiEye,
    FiEyeOff
} from "react-icons/fi";
import { likeReview, unlikeReview } from "../../api/reviewLikeApi.js";
import { useAuth } from "../../context/AuthContext";

function ReviewCard({ review, showMedia = true }) {
    const [showSpoiler, setShowSpoiler] = useState(
        !review.containsSpoilers
    );
    const navigate = useNavigate();
    const { isAuthenticated } = useAuth();
    const [liked, setLiked] = useState(review.likedByCurrentUser === true);
    const [likeCount, setLikeCount] = useState(review.likeCount);
    const [likeLoading, setLikeLoading] = useState(false);

    async function handleLike() {
        if (!isAuthenticated) {
            navigate("/login");
            return;
        }

        if (likeLoading) {
            return;
        }

        const previousLiked = liked;
        const previousLikeCount = likeCount;

        const nextLiked = !liked;
        
        setLiked(nextLiked);
        setLikeCount(
            nextLiked
                ? previousLikeCount + 1
                : previousLikeCount - 1
        );

        setLikeLoading(true);

        try {
            if (nextLiked) {
                await likeReview(review.id);
            } else {
                await unlikeReview(review.id);
            }
        } catch (error) {
            console.error(
                "Failed to update review like:",
                error
            );
            
            setLiked(previousLiked);
            setLikeCount(previousCount);
        } finally {
            setLikeLoading(false);
        }
    }
    
    return (
        <article className="review-card">
            {showMedia && (
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
            )}
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
                        liked
                            ? "review-like liked"
                            : "review-like"
                    }
                    onClick={handleLike}
                    disabled={likeLoading}
                    aria-label={liked
                                ? "Unlike review"
                                : "Like review"
                    }
                >
                    <FiHeart
                        size={17}
                        fill={
                            liked
                                ? "currentColor"
                                : "none"
                        }
                    />

                    <span>{likeCount}</span>
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