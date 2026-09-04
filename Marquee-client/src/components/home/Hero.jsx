import { Link } from "react-router-dom";
import { FiArrowRight, FiStar } from "react-icons/fi";

function Hero({ media }) {
    if (!media) {
        return null;
    }

    const year = media.releaseDate
        ? new Date(media.releaseDate).getFullYear()
        : null;

    const averageStars =
        media.averageRating != null
            ? media.averageRating / 2
            : null;

    return (
        <section className="hero">
            <div className="hero-backdrop">
                {media.backdropUrl && (
                    <img
                        src={media.backdropUrl}
                        alt=""
                    />
                )}
            </div>

            <div className="hero-overlay" />

            <div className="hero-content">
                <span className="hero-eyebrow">
                    Featured
                </span>

                <h1>{media.title}</h1>

                <div className="hero-meta">
                    {year && <span>{year}</span>}

                    <span>
                        {media.mediaType === 1
                            ? "Movie"
                            : "TV Show"}
                    </span>

                    {averageStars != null && (
                        <span className="hero-rating">
                            <FiStar
                                size={16}
                                fill="currentColor"
                            />

                            {averageStars.toFixed(1)}
                        </span>
                    )}
                </div>

                {media.overview && (
                    <p className="hero-overview">
                        {media.overview}
                    </p>
                )}

                <Link
                    to={`/media/${media.id}`}
                    className="hero-button"
                >
                    View details
                    <FiArrowRight size={17} />
                </Link>
            </div>
        </section>
    );
}

export default Hero;