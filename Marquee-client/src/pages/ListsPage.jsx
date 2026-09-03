import {useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {FiEdit2, FiGlobe, FiList, FiLock, FiPlus, FiTrash2,} from "react-icons/fi";
import {createList, deleteList, getMyLists, updateList,} from "../api/mediaListApi";

function ListsPage() {
    const [lists, setLists] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [showCreateForm, setShowCreateForm] = useState(false);
    const [editingList, setEditingList] = useState(null);


    async function loadLists() {
        try {
            setError("");
            const data = await getMyLists();
            setLists(data);
        } catch (error) {
            console.error(error);
            setError("Failed to load your lists.");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        loadLists();
    }, []);

    async function handleCreate(data) {
        try {
            const created = await createList(data);

            setLists((current) => [...current, created]);
            setShowCreateForm(false);
        } catch (error) {
            console.error(error);
            throw new Error("Failed to create list.");
        }
    }

    async function handleUpdate(id, data) {
        try {
            await updateList(id, data);

            setLists((current) =>
                current.map((list) =>
                    list.id === id
                        ? {
                            ...list,
                            ...data,
                        }
                        : list
                )
            );

            setEditingList(null);
        } catch (error) {
            console.error(error);
            throw new Error("Failed to update list.");
        }
    }

    async function handleDelete(list) {
        const confirmed = window.confirm(
            `Are you sure you want to delete "${list.name}"?`
        );

        if (!confirmed) {
            return;
        }

        try {
            await deleteList(list.id);

            setLists((current) =>
                current.filter((item) => item.id !== list.id)
            );
        } catch (error) {
            console.error(error);
            setError("Failed to delete the list.");
        }
    }

    if (loading) {
        return (
            <main className="page-container lists-page">
                <p className="lists-status">Loading lists...</p>
            </main>
        );
    }

    return (
        <main className="page-container lists-page">
            <div className="lists-page-header">
                <div>
                    <p className="section-eyebrow">COLLECTIONS</p>
                    <h1>Your Lists</h1>
                    <p className="lists-page-subtitle">
                        Organize the movies and shows you want to keep track of.
                    </p>
                </div>

                <button
                    className="lists-create-button"
                    type="button"
                    onClick={() => {
                        setEditingList(null);
                        setShowCreateForm(true);
                    }}
                >
                    <FiPlus size={18}/>
                    Create list
                </button>
            </div>

            {error && (
                <div className="lists-error">
                    {error}
                </div>
            )}

            {lists.length === 0 ? (
                <section className="lists-empty">
                    <div className="lists-empty-icon">
                        <FiList size={34}/>
                    </div>

                    <h2>No lists yet</h2>

                    <p>
                        Create your first list to start organizing your
                        favorite movies and shows.
                    </p>

                    <button
                        className="lists-create-button"
                        type="button"
                        onClick={() => setShowCreateForm(true)}
                    >
                        <FiPlus size={18}/>
                        Create your first list
                    </button>
                </section>
            ) : (
                <section className="lists-grid">
                    {lists.map((list) => (
                        <article className="list-card" key={list.id}>
                            <Link
                                to={`/lists/${list.id}`}
                                className="list-card-main"
                            >
                                <div className="list-card-icon">
                                    <FiList size={23}/>
                                </div>

                                <div className="list-card-content">
                                    <div className="list-card-title-row">
                                        <h2>{list.name}</h2>

                                        {list.isPublic ? (
                                            <FiGlobe
                                                size={15}
                                                title="Public"
                                            />
                                        ) : (
                                            <FiLock
                                                size={15}
                                                title="Private"
                                            />
                                        )}
                                    </div>

                                    {list.description && (
                                        <p>
                                            {list.description}
                                        </p>
                                    )}

                                    <span className="list-card-count">
                                        {list.itemCount ?? 0}{" "}
                                        {list.itemCount === 1
                                            ? "item"
                                            : "items"}
                                    </span>
                                </div>
                            </Link>

                            <div className="list-card-actions">
                                <button
                                    type="button"
                                    className="list-icon-button"
                                    aria-label={`Edit ${list.name}`}
                                    title="Edit list"
                                    onClick={() => {
                                        setShowCreateForm(false);
                                        setEditingList(list);
                                    }}
                                >
                                    <FiEdit2 size={16}/>
                                </button>

                                <button
                                    type="button"
                                    className="list-icon-button list-icon-button-danger"
                                    aria-label={`Delete ${list.name}`}
                                    title="Delete list"
                                    onClick={() => handleDelete(list)}
                                >
                                    <FiTrash2 size={16}/>
                                </button>
                            </div>
                        </article>
                    ))}
                </section>
            )}

            {showCreateForm && (
                <ListFormModal
                    title="Create list"
                    onClose={() => setShowCreateForm(false)}
                    onSubmit={handleCreate}
                />
            )}

            {editingList && (
                <ListFormModal
                    title="Edit list"
                    initialValues={{
                        name: editingList.name,
                        description: editingList.description || "",
                        isPublic: editingList.isPublic,
                    }}
                    onClose={() => setEditingList(null)}
                    onSubmit={(data) =>
                        handleUpdate(editingList.id, data)
                    }
                />
            )}
        </main>
    );
}

function ListFormModal({title, initialValues = {name: "", description: "", isPublic: true,}, onClose, onSubmit,}) {
    const [name, setName] = useState(initialValues.name);
    const [description, setDescription] = useState(
        initialValues.description
    );
    const [isPublic, setIsPublic] = useState(initialValues.isPublic);

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
            setError(error.message);
        } finally {
            setSaving(false);
        }
    }

    return (
        <div className="modal-backdrop" onMouseDown={onClose}>
            <div
                className="list-modal"
                onMouseDown={(event) => event.stopPropagation()}
            >
                <div className="list-modal-header">
                    <h2>{title}</h2>

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
                        <label htmlFor="list-name">
                            Name
                        </label>

                        <input
                            id="list-name"
                            type="text"
                            maxLength={150}
                            value={name}
                            onChange={(event) =>
                                setName(event.target.value)
                            }
                            placeholder="e.g. Watchlist"
                            autoFocus
                        />
                    </div>

                    <div className="profile-form-group">
                        <label htmlFor="list-description">
                            Description
                        </label>

                        <textarea
                            id="list-description"
                            maxLength={600}
                            rows={4}
                            value={description}
                            onChange={(event) =>
                                setDescription(event.target.value)
                            }
                            placeholder="What is this list about?"
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

export default ListsPage;