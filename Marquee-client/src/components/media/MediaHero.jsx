import { Link } from "react-router-dom";
import {FiArrowLeft, FiCheck, FiPlus, FiStar} from "react-icons/fi";
import RatingStars from "./RatingStars";
import { useEffect, useState } from "react";
import { getMyRating, setRating } from "../../api/ratingApi";
import { useAuth } from "../../context/AuthContext";
import { getMyLists, addMediaToList } from "../../api/mediaListApi";

function MediaHero({ media }) {

    const { isAuthenticated } = useAuth();

    const [userRating, setUserRating] =
        useState(0);

    const [savingRating, setSavingRating] =
        useState(false);
    const [showListPicker, setShowListPicker] = useState(false);
    const [lists, setLists] = useState([]);
    const [listsLoading, setListsLoading] = useState(false);
    const [listsError, setListsError] = useState("");
    const [addingToListId, setAddingToListId] = useState(null);
    const [addedListIds, setAddedListIds] = useState(new Set());
    
    
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

    async function openListPicker() {
        if (!isAuthenticated) {
            return;
        }
        setShowListPicker((prev) => !prev);
        if (!showListPicker && lists.length === 0) {
            setListsLoading(true);
            setListsError("");
            try {
                const data = await getMyLists(media.id);
                setLists(data);
                const initialAdded = new Set();
                data.forEach((list) => {
                    if (list.containsMedia) {
                        initialAdded.add(list.id);
                    }
                });
                setAddedListIds(initialAdded);
            } catch (error) {
                console.error(error);
                setListsError("Failed to load your lists.");
            } finally {
                setListsLoading(false);
            }
        }
    }

    async function handleAddToList(listId) {
        if (addingToListId) return;
        setAddingToListId(listId);
        try {
            await addMediaToList(listId, media.id);
            setAddedListIds((prev) => new Set(prev).add(listId));
            setLists((current) =>
                current.map((list) =>
                    list.id === listId
                        ? { ...list, itemCount: (list.itemCount ?? 0) + 1, containsMedia: true }
                        : list
                )
            );
        } catch (error) {
            console.error("Failed to add to list:", error);
            setListsError("Failed to add to list.");
        } finally {
            setAddingToListId(null);
        }
    }
    
    const renderListPicker = () => {
        if (listsLoading) {
            return <div className="list-picker-status">Loading lists...</div>;
        }
        if (listsError) {
            return <div className="list-picker-status list-picker-error">{listsError}</div>;
        }
        if (lists.length === 0) {
            return (
                <div className="list-picker-status">
                    You have no lists yet.{" "}
                    <Link to="/lists" className="list-picker-link">
                        Create one
                    </Link>
                </div>
            );
        }
        return (
            <ul className="list-picker-list">
                {lists.map((list) => {
                    const isAdded = addedListIds.has(list.id);
                    return (
                        <li key={list.id} className="list-picker-item">
                            <div className="list-picker-info">
                                <span className="list-picker-name">{list.name}</span>
                                <span className="list-picker-count">
                                    {list.itemCount ?? 0} items
                                </span>
                            </div>
                            <button
                                type="button"
                                className={`list-picker-add-btn ${isAdded ? "added" : ""}`}
                                onClick={() => handleAddToList(list.id)}
                                disabled={addingToListId === list.id || isAdded}
                            >
                                {addingToListId === list.id ? (
                                    "Adding..."
                                ) : isAdded ? (
                                    <>
                                        <FiCheck size={14} /> Added
                                    </>
                                ) : (
                                    <>
                                        <FiPlus size={14} /> Add
                                    </>
                                )}
                            </button>
                        </li>
                    );
                })}
            </ul>
        );
    };


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

                            {/* Add to list button + picker */}
                            <div className="list-picker-container">
                                {isAuthenticated ? (
                                    <button
                                        type="button"
                                        className="secondary-button list-picker-toggle"
                                        onClick={openListPicker}
                                    >
                                        <FiPlus size={16} />
                                        Add to list
                                    </button>
                                ) : (
                                    <Link to="/login" className="secondary-button">
                                        <FiPlus size={16} />
                                        Add to list
                                    </Link>
                                )}

                                {showListPicker && isAuthenticated && (
                                    <div className="list-picker-dropdown">
                                        {renderListPicker()}
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default MediaHero;