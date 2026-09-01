import { Link } from "react-router-dom";
import { FiStar } from "react-icons/fi";

function MediaCard({ media }) {
    const year = media.releaseDate
        ? new Date(media.releaseDate).getFullYear()
        : null;

    const averageStars =
        media.averageRating != null
            ? media.averageRating / 2
            : null;
    
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

                {averageStars != null && (
                    <div className="media-card-rating">
                        <FiStar
                            size={13}
                            fill="currentColor"
                        />

                        {averageStars.toFixed(1)}
                    </div>
                )}
            </div>

            <div className="media-card-info">
                <h3>{media.title}</h3>

                {year && (
                    <span>
                        {year}
                    </span>
                )}
            </div>
        </Link>
    );
}

export default MediaCard;