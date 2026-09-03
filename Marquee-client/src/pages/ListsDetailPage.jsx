import {useEffect, useState} from "react";
import {Link, useNavigate, useParams} from "react-router-dom";
import {FiArrowLeft, FiEdit2, FiGlobe, FiLock, FiPlus, FiSearch, FiTrash2, FiX,} from "react-icons/fi";
import {addMediaToList, deleteList, getListById, removeMediaFromList, updateList} from "../api/mediaListApi";
import {searchMedia} from "../api/mediaApi";

function ListDetailPage() {
    const {id} = useParams();
    const navigate = useNavigate();

    const [list, setList] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [removingMediaId, setRemovingMediaId] = useState(null);
    const [editing, setEditing] = useState(false);

    const [searchQuery, setSearchQuery] = useState("");
    const [searchResults, setSearchResults] = useState([]);
    const [searchLoading, setSearchLoading] = useState(false);
    const [searchError, setSearchError] = useState("");
    const [addingMediaId, setAddingMediaId] = useState(null);

    async function loadList() {
        try {
            setError("");

            const data = await getListById(id);
            setList(data);
        } catch (error) {
            console.error(error);
            setError("Failed to load this list.");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        loadList();
    }, [id]);

    async function handleSearchSubmit(event) {
        event.preventDefault();
        const query = searchQuery.trim();
        if (!query) return;

        setSearchLoading(true);
        setSearchError("");
        try {
            const results = await searchMedia(query);
            const existingIds = new Set(list.items.map((item) => item.mediaId));
            const filteredResults = results.filter((media) => !existingIds.has(media.id));
            setSearchResults(filteredResults);
        } catch (error) {
            console.error(error);
            setSearchError("Failed to search media.");
        } finally {
            setSearchLoading(false);
        }
    }

    async function handleAddMedia(media) {
        if (addingMediaId) return;
        setAddingMediaId(media.id);
        try {
            await addMediaToList(list.id, media.id);
            const newItem = {
                mediaId: media.id,
                title: media.title,
                posterUrl: media.posterUrl,
                releaseDate: media.releaseDate,
            };
            setList((current) => ({
                ...current,
                items: [...current.items, newItem],
            }));
            setSearchResults((prev) => prev.filter((m) => m.id !== media.id));
        } catch (error) {
            console.error("Failed to add media to list:", error);
            setError("Failed to add media to list.");
        } finally {
            setAddingMediaId(null);
        }
    }

    async function handleRemove(mediaId, title) {
        const confirmed = window.confirm(
            `Remove "${title}" from this list?`
        );

        if (!confirmed) {
            return;
        }

        try {
            setRemovingMediaId(mediaId);

            await removeMediaFromList(id, mediaId);

            setList((current) => ({
                ...current,
                items: current.items.filter(
                    (item) => item.mediaId !== mediaId
                ),
            }));
        } catch (error) {
            console.error(error);
            setError("Failed to remove the item.");
        } finally {
            setRemovingMediaId(null);
        }
    }

    async function handleUpdate(data) {
        await updateList(id, data);

        setList((current) => ({
            ...current,
            ...data,
        }));

        setEditing(false);
    }

    async function handleDelete() {
        const confirmed = window.confirm(
            `Are you sure you want to delete "${list.name}"?`
        );

        if (!confirmed) {
            return;
        }

        try {
            await deleteList(id);
            navigate("/lists");
        } catch (error) {
            console.error(error);
            setError("Failed to delete the list.");
        }
    }

    if (loading) {
        return (
            <main className="page-container lists-page">
                <p className="lists-status">Loading list...</p>
            </main>
        );
    }

    if (error && !list) {
        return (
            <main className="page-container lists-page">
                <Link to="/lists" className="list-back-link">
                    <FiArrowLeft size={17}/>
                    Back to lists
                </Link>

                <div className="lists-error">
                    {error}
                </div>
            </main>
        );
    }

    return (
        <main className="page-container list-detail-page">
            <Link to="/lists" className="list-back-link">
                <FiArrowLeft size={17}/>
                Back to lists
            </Link>

            <header className="list-detail-header">
                <div className="list-detail-info">
                    <div className="list-detail-title-row">
                        <h1>{list.name}</h1>

                        {list.isPublic ? (
                            <span className="list-visibility">
                                <FiGlobe size={15}/>
                                Public
                            </span>
                        ) : (
                            <span className="list-visibility">
                                <FiLock size={15}/>
                                Private
                            </span>
                        )}
                    </div>

                    {list.description && (
                        <p>{list.description}</p>
                    )}

                    <span className="list-detail-count">
                        {list.items?.length ?? 0}{" "}
                        {list.items?.length === 1
                            ? "item"
                            : "items"}
                    </span>
                </div>

                <div className="list-detail-actions">
                    <button
                        type="button"
                        className="profile-secondary-button"
                        onClick={() => setEditing(true)}
                    >
                        <FiEdit2 size={17}/>
                        Edit
                    </button>

                    <button
                        type="button"
                        className="profile-secondary-button list-delete-button"
                        onClick={handleDelete}
                    >
                        <FiTrash2 size={17}/>
                        Delete
                    </button>
                </div>
            </header>

            <section className="list-search-section">
                <form onSubmit={handleSearchSubmit} className="list-search-form">
                    <div className="list-search-input-wrapper">
                        <FiSearch size={18} className="list-search-icon"/>
                        <input
                            type="text"
                            placeholder="Search movies or TV shows to add..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            className="list-search-input"
                        />
                    </div>
                    <button type="submit" className="profile-primary-button" disabled={searchLoading}>
                        {searchLoading ? "Searching..." : "Search"}
                    </button>
                </form>

                {searchResults.length > 0 && (
                    <ul className="list-search-results">
                        {searchResults.map((media) => (
                            <li key={media.id} className="list-search-result-item">
                                <div className="list-search-result-info">
                                    <span className="list-search-result-title">{media.title}</span>
                                    {media.releaseDate && (
                                        <span className="list-search-result-year">
                                            {new Date(media.releaseDate).getFullYear()}
                                        </span>
                                    )}
                                </div>
                                <button
                                    type="button"
                                    className="list-search-add-btn"
                                    onClick={() => handleAddMedia(media)}
                                    disabled={addingMediaId === media.id}
                                >
                                    {addingMediaId === media.id ? (
                                        "Adding..."
                                    ) : (
                                        <>
                                            <FiPlus size={14}/> Add
                                        </>
                                    )}
                                </button>
                            </li>
                        ))}
                    </ul>
                )}

                {searchError && <p className="list-search-error">{searchError}</p>}
            </section>

            {error && (
                <div className="lists-error">
                    {error}
                </div>
            )}

            {!list.items?.length ? (
                <section className="list-detail-empty">
                    <h2>This list is empty</h2>
                    <p>
                        Add movies and shows to start building your
                        collection.
                    </p>
                </section>
            ) : (
                <section className="list-items-grid">
                    {list.items.map((item) => (
                        <article
                            className="list-media-card"
                            key={item.mediaId}
                        >
                            <Link
                                to={`/media/${item.mediaId}`}
                                className="list-media-poster"
                            >
                                {item.posterUrl ? (
                                    <img
                                        src={item.posterUrl}
                                        alt={item.title}
                                        loading="lazy"
                                    />
                                ) : (
                                    <div className="media-card-placeholder">
                                        No poster
                                    </div>
                                )}
                            </Link>

                            <div className="list-media-info">
                                <Link
                                    to={`/media/${item.mediaId}`}
                                    className="list-media-title"
                                >
                                    {item.title}
                                </Link>

                                {item.releaseDate && (
                                    <span>
                                        {new Date(
                                            item.releaseDate
                                        ).getFullYear()}
                                    </span>
                                )}
                            </div>

                            <button
                                type="button"
                                className="list-media-remove"
                                title="Remove from list"
                                aria-label={`Remove ${item.title} from list`}
                                disabled={
                                    removingMediaId === item.mediaId
                                }
                                onClick={() =>
                                    handleRemove(
                                        item.mediaId,
                                        item.title
                                    )
                                }
                            >
                                <FiX size={17}/>
                            </button>
                        </article>
                    ))}
                </section>
            )}

            {editing && (
                <ListEditModal
                    list={list}
                    onClose={() => setEditing(false)}
                    onSubmit={handleUpdate}
                />
            )}
        </main>
    );
}

function ListEditModal({list, onClose, onSubmit}) {
    const [name, setName] = useState(list.name);
    const [description, setDescription] = useState(
        list.description || ""
    );
    const [isPublic, setIsPublic] = useState(list.isPublic);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");

    async function handleSubmit(event) {
        event.preventDefault();

        if (!name.trim()) {
            setError("List name is required.");
            return;
        }

        setSaving(true);
        setError("");

        try {
            await onSubmit({
                name: name.trim(),
                description: description.trim() || null,
                isPublic,
            });
        } catch (error) {
            setError("Failed to update the list.");
        } finally {
            setSaving(false);
        }
    }

    return (
        <div className="modal-backdrop" onMouseDown={onClose}>
            <div
                className="list-modal"
                onMouseDown={(event) =>
                    event.stopPropagation()
                }
            >
                <div className="list-modal-header">
                    <h2>Edit list</h2>

                    <button
                        type="button"
                        className="modal-close-button"
                        onClick={onClose}
                        disabled={saving}
                    >
                        ×
                    </button>
                </div>

                <form onSubmit={handleSubmit}>
                    <div className="profile-form-group">
                        <label htmlFor="edit-list-name">
                            Name
                        </label>

                        <input
                            id="edit-list-name"
                            type="text"
                            maxLength={150}
                            value={name}
                            onChange={(event) =>
                                setName(event.target.value)
                            }
                        />
                    </div>

                    <div className="profile-form-group">
                        <label htmlFor="edit-list-description">
                            Description
                        </label>

                        <textarea
                            id="edit-list-description"
                            rows={4}
                            maxLength={600}
                            value={description}
                            onChange={(event) =>
                                setDescription(event.target.value)
                            }
                        />
                    </div>

                    <label className="list-visibility-toggle">
                        <input
                            type="checkbox"
                            checked={isPublic}
                            onChange={(event) =>
                                setIsPublic(event.target.checked)
                            }
                        />

                        <span>
                            {isPublic ? (
                                <>
                                    <FiGlobe size={16}/>
                                    Public list
                                </>
                            ) : (
                                <>
                                    <FiLock size={16}/>
                                    Private list
                                </>
                            )}
                        </span>
                    </label>

                    {error && (
                        <p className="profile-form-error">
                            {error}
                        </p>
                    )}

                    <div className="profile-form-actions">
                        <button
                            type="submit"
                            className="profile-primary-button"
                            disabled={saving}
                        >
                            {saving ? "Saving..." : "Save"}
                        </button>

                        <button
                            type="button"
                            className="profile-secondary-button"
                            onClick={onClose}
                            disabled={saving}
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default ListDetailPage;