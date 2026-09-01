import { useState } from "react";
import { FiStar } from "react-icons/fi";

function RatingStars({
                         value = 0,
                         onChange,
                         disabled = false
                     }) {
    const [hoverValue, setHoverValue] = useState(null);

    const displayedValue =
        hoverValue ?? value;

    return (
        <div
            className="rating-stars"
            onMouseLeave={() => setHoverValue(null)}
        >
            {Array.from({ length: 10 }, (_, index) => {
                const starNumber = index + 1;

                const isFull =
                    displayedValue >= starNumber;

                const isHalf =
                    displayedValue === starNumber - 0.5;

                return (
                    <button
                        key={starNumber}
                        type="button"
                        className="rating-star"
                        disabled={disabled}
                        onMouseMove={(event) => {
                            const rect =
                                event.currentTarget.getBoundingClientRect();

                            const mouseX =
                                event.clientX - rect.left;

                            const newValue =
                                mouseX <
                                rect.width / 2
                                    ? starNumber - 0.5
                                    : starNumber;

                            setHoverValue(newValue);
                        }}
                        onClick={() => {
                            if (hoverValue !== null) {
                                onChange?.(
                                    hoverValue * 2
                                );
                            }
                        }}
                        aria-label={`${starNumber} stars`}
                    >
                        <span className="star-icon">
                            <FiStar className="star-outline" />

                            {isFull && (
                                <FiStar className="star-filled" />
                            )}

                            {isHalf && (
                                <span className="star-half">
                                    <FiStar className="star-filled" />
                                </span>
                            )}
                        </span>
                    </button>
                );
            })}
        </div>
    );
}

export default RatingStars;