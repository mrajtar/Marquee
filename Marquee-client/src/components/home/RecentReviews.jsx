import { useEffect, useState } from "react";

import ReviewCard from "../reviews/ReviewCard";
import { getRecentReviews } from "../../api/reviewApi";

function RecentReviews() {
    const [reviews, setReviews] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        async function loadReviews() {
            try {
                const data = await getRecentReviews(10);

                setReviews(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        }

        loadReviews();
    }, []);

    if (loading) {
        return (
            <section className="media-section">
                <div className="media-section-header">
                    <h2>Recent Reviews</h2>
                </div>

                <p>Loading reviews...</p>
            </section>
        );
    }

    if (error) {
        return (
            <section className="media-section">
                <div className="media-section-header">
                    <h2>Recent Reviews</h2>
                </div>

                <p>Failed to load reviews.</p>
            </section>
        );
    }

    if (reviews.length === 0) {
        return (
            <section className="media-section">
                <div className="media-section-header">
                    <h2>Recent Reviews</h2>
                </div>

                <p>No reviews yet.</p>
            </section>
        );
    }

    return (
        <section className="media-section">
            <div className="media-section-header">
                <h2>Recent Reviews</h2>
            </div>

            <div className="review-row">
                {reviews.map((review) => (
                    <ReviewCard
                        key={review.id}
                        review={review}
                    />
                ))}
            </div>
        </section>
    );
}

export default RecentReviews;