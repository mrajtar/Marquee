import { useParams, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { FiArrowLeft, FiHeart, FiEye, FiEyeOff } from "react-icons/fi";
import { getReviewById } from "../api/reviewApi";
import { useAuth } from "../context/AuthContext";
import { likeReview, unlikeReview } from "../api/reviewLikeApi";

function ReviewDetailPage() {
    const { reviewId } = useParams();
    const [review, setReview] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showSpoiler, setShowSpoiler] = useState(false);
    const [liked, setLiked] = useState(false);
    const [likeCount, setLikeCount] = useState(0);
    const { isAuthenticated } = useAuth();

    useEffect(() => {
        async function loadReview() {
            try {
                const data = await getReviewById(reviewId);
                setReview(data);
                setLiked(data.likedByCurrentUser === true);
                setLikeCount(data.likeCount);
                setShowSpoiler(!data.containsSpoilers);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }
        loadReview();
    }, [reviewId]);

    if (loading) return <p className="page-container">Loading…</p>;
    if (error) return <p className="page-container">Error: {error}</p>;
    if (!review) return <p className="page-container">Review not found.</p>;

    async function handleLike() {
        if (!isAuthenticated) {
            window.location.href = "/login";
            return;
        }
        
        const previousLiked = liked;
        const previousCount = likeCount;
        setLiked(!liked);
        setLikeCount(previousCount + (liked ? -1 : 1));
        try {
            if (!liked) await likeReview(review.id);
            else await unlikeReview(review.id);
        } catch (error) {
            setLiked(previousLiked);
            setLikeCount(previousCount);
        }
    }

    return (
        <div className="page-container review-detail-page">
            <Link to={`/media/${review.mediaId}`} className="media-back-link">
                <FiArrowLeft size={17} /> Back
            </Link>

            <article className="review-detail">
                <header className="review-detail-header">
                    <div className="review-author">
                        <div className="review-avatar">
                            {review.profileImageUrl ? (
                                <img src={review.profileImageUrl} alt="" />
                            ) : (
                                <span>{(review.displayName || review.username).charAt(0).toUpperCase()}</span>
                            )}
                        </div>
                        <div>
                            <strong>{review.displayName || review.username}</strong>
                            <span>@{review.username}</span>
                        </div>
                    </div>
                    <time>{formatDate(review.createdAt)}</time>
                </header>

                <div className="review-detail-content">
                    {review.containsSpoilers && !showSpoiler ? (
                        <div className="spoiler-warning">
                            <FiEyeOff size={24} />
                            <span>This review contains spoilers.</span>
                            <button type="button" onClick={() => setShowSpoiler(true)}>
                                <FiEye size={16} /> Reveal review
                            </button>
                        </div>
                    ) : (
                        <p>{review.content}</p>
                    )}
                </div>

                <footer className="review-detail-footer">
                    <button
                        type="button"
                        className={liked ? "review-like liked" : "review-like"}
                        onClick={handleLike}
                    >
                        <FiHeart size={18} fill={liked ? "currentColor" : "none"} />
                        <span>{likeCount}</span>
                    </button>
                </footer>
            </article>
        </div>
    );
}

function formatDate(value) {
    return new Date(value).toLocaleDateString(undefined, {
        day: "numeric",
        month: "short",
        year: "numeric"
    });
}

export default ReviewDetailPage;