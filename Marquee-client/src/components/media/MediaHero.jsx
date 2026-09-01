import { Link } from "react-router-dom";
import { FiArrowLeft, FiStar } from "react-icons/fi";
import RatingStars from "./RatingStars";
import { useEffect, useState } from "react";

import {
    getMyRating,
    setRating
} from "../../api/ratingApi";

import { useAuth } from "../../context/AuthContext";

function MediaHero({ media }) {

    const { isAuthenticated } = useAuth();

    const [userRating, setUserRating] =
        useState(0);

    const [savingRating, setSavingRating] =
        useState(false);
    
    const year = media.releaseDate
        ? new Date(media.releaseDate).getFullYear()
        : null;

    const isMovie = media.mediaType === 0;

    const mediaTypeLabel = isMovie
        ? "Movie"
        : "TV Show";

    const averageStars =
        media.averageRating != null
            ? media.averageRating / 2
            : null;

    useEffect(() => {
        if (!isAuthenticated) {
            setUserRating(0);
            return;
        }

        async function loadRating() {
            try {
                const rating =
                    await getMyRating(media.id);

                setUserRating(
                    rating?.value ?? 0
                );
            } catch (error) {
                setUserRating(0);
            }
        }

        loadRating();
    }, [
        media.id,
        isAuthenticated
    ]);
    async function handleRatingChange(value) {
        if (!isAuthenticated) {
            return;
        }

        setSavingRating(true);

        try {
            await setRating(
                media.id,
                value
            );

            setUserRating(value);
        } catch (error) {
            console.error(
                "Failed to save rating:",
                error
            );
        } finally {
            setSavingRating(false);
        }
    }
    
    return (
        <section className="media-hero">
            <div className="media-hero-backdrop">
                {media.backdropUrl && (
                    <img
                        src={media.backdropUrl}
                        alt=""
                    />
                )}
            </div>

            <div className="media-hero-overlay" />

            <div className="media-hero-content">
                <Link
                    to="/"
                    className="media-back-link"
                >
                    <FiArrowLeft size={17} />
                    Back
                </Link>

                <div className="media-hero-main">
                    <div className="media-detail-poster">
                        {media.posterUrl ? (
                            <img
                                src={media.posterUrl}
                                alt={`${media.title} poster`}
                            />
                        ) : (
                            <div className="media-card-placeholder">
                                No poster
                            </div>
                        )}
                    </div>

                    <div className="media-hero-info">
                        <p className="media-type">
                            {mediaTypeLabel}
                        </p>

                        <h1>{media.title}</h1>

                        <div className="media-meta">
                            {year && (
                                <span>{year}</span>
                            )}

                            {averageStars != null && (
                                <span className="media-rating">
                                    <FiStar
                                        size={17}
                                        fill="currentColor"
                                    />

                                    {averageStars.toFixed(1)}
                                </span>
                            )}

                            {media.ratingCount > 0 && (
                                <span>
                                    {media.ratingCount}{" "}
                                    {media.ratingCount === 1
                                        ? "rating"
                                        : "ratings"}
                                </span>
                            )}

                            {media.reviewCount > 0 && (
                                <span>
                                    {media.reviewCount}{" "}
                                    {media.reviewCount === 1
                                        ? "review"
                                        : "reviews"}
                                </span>
                            )}
                        </div>

                        {media.overview && (
                            <p className="media-overview">
                                {media.overview}
                            </p>
                        )}

                        <div className="media-actions">
                            <div className="user-rating">
                                <div className="user-rating-label">
                                    <span>Your rating</span>

                                    <strong>
                                        {userRating > 0
                                            ? (
                                                userRating / 2
                                            ).toFixed(1)
                                            : "—"}
                                    </strong>
                                </div>

                                {isAuthenticated ? (
                                    <RatingStars
                                        value={
                                            userRating / 2
                                        }
                                        onChange={
                                            handleRatingChange
                                        }
                                        disabled={
                                            savingRating
                                        }
                                    />
                                ) : (
                                    <Link
                                        to="/login"
                                        className="rating-login"
                                    >
                                        Log in to rate
                                    </Link>
                                )}
                            </div>

                            <button
                                type="button"
                                className="secondary-button"
                            >
                                Add to list
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default MediaHero;