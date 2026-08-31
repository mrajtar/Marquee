import { Link } from "react-router-dom";

function MediaCard({ media }) {
    return (
        <Link
            to={`/media/${media.id}`}
            className="media-card"
        >
            <div className="media-card-poster">
                {media.posterUrl ? (
                    <img
                        src={media.posterUrl}
                        alt={media.title}
                        loading="lazy"
                    />
                ) : (
                    <div className="media-card-placeholder">
                        No poster
                    </div>
                )}
            </div>

            <div className="media-card-info">
                <h3>{media.title}</h3>

                {media.releaseDate && (
                    <span>
                        {new Date(
                            media.releaseDate
                        ).getFullYear()}
                    </span>
                )}
            </div>
        </Link>
    );
}

export default MediaCard;