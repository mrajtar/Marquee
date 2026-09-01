import { useEffect, useState } from "react";

import ReviewCard from "../reviews/ReviewCard";
import {
    getReviewsByMediaId
} from "../../api/reviewApi";
import ReviewForm from "../reviews/ReviewForm";

function MediaReviews({ mediaId, reviewCount: initialReviewCount }) {
    const [reviews, setReviews] =
        useState([]);
    
    const [reviewCount, setReviewCount] = useState(initialReviewCount);

    const [loading, setLoading] =
        useState(true);

    const [error, setError] =
        useState(null);

    function handleReviewCreated(review) {
        setReviews((currentReviews) => [
            review,
            ...currentReviews
        ]);
        
        setReviewCount(
            (currentCount) => currentCount +1
        );
    }

    useEffect(() => {
        async function loadReviews() {
            try {
                const data =
                    await getReviewsByMediaId(
                        mediaId
                    );

                setReviews(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        }

        loadReviews();
    }, [mediaId]);

    return (
        <section className="media-reviews">
            <div className="media-section-title">
                <span />

                <h2>
                    Reviews
                    {reviewCount > 0 &&
                        ` (${reviewCount})`}
                </h2>
            </div>
            
            <ReviewForm
                mediaId={mediaId}
                onReviewCreated={handleReviewCreated}
            />
            
            {loading && (
                <p className="section-message">
                    Loading reviews...
                </p>
            )}

            {error && (
                <p className="section-message">
                    Failed to load reviews.
                </p>
            )}

            {!loading &&
                !error &&
                reviews.length === 0 && (
                    <p className="section-message">
                        No reviews yet.
                    </p>
                )}

            {!loading &&
                !error &&
                reviews.length > 0 && (
                    <div className="media-reviews-list">
                        {reviews.map((review) => (
                            <ReviewCard
                                key={review.id}
                                review={review}
                                showMedia={false}
                            />
                        ))}
                    </div>
                )}
        </section>
    );
}

export default MediaReviews;