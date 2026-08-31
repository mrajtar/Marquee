import { Link } from "react-router-dom";

function Hero({ media }) {
    if (!media) {
        return null;
    }

    const year = media.releaseDate
        ? new Date(media.releaseDate).getFullYear()
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
                <p className="hero-eyebrow">
                    Featured
                </p>

                <h1>{media.title}</h1>

                <div className="hero-meta">
                    {year && <span>{year}</span>}
                    <span>{media.type}</span>
                </div>

                <Link
                    to={`/media/${media.id}`}
                    className="hero-button"
                >
                    View details
                </Link>
            </div>
        </section>
    );
}

export default Hero;