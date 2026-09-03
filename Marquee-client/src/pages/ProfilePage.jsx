import {useAuth} from "../context/AuthContext";
import {useState} from "react";
import {useNavigate} from "react-router-dom";
import {FiEdit2, FiLogOut, FiSave, FiUser, FiX} from "react-icons/fi";
import {getMe, updateMe} from "../api/userApi";

function ProfilePage() {
    const {user, logout, updateUser} = useAuth();
    const navigate = useNavigate();

    const [isEditing, setIsEditing] = useState(false);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");

    const [displayName, setDisplayName] = useState("");
    const [bio, setBio] = useState("");
    const [profileImageUrl, setProfileImageUrl] = useState("");

    function handleLogout() {
        logout();
        setTimeout(() => {
            navigate("/", {replace: true});
        }, 100);
    }

    const displayNameFallback = user.displayName || user.userName;

    function startEditing() {
        setDisplayName(user.displayName || "");
        setBio(user.bio || "");
        setProfileImageUrl(user.profileImageUrl || "");
        setError("");
        setIsEditing(true);
    }

    function cancelEditing() {
        setError("");
        setIsEditing(false);
    }

    async function handleSave(event) {
        event.preventDefault();

        setSaving(true);
        setError("");

        try {
            await updateMe({
                displayName: displayName.trim() || null,
                bio: bio.trim() || null,
                profileImageUrl: profileImageUrl.trim() || null,
            });

            const freshUser = await getMe();
            updateUser(freshUser);

            setIsEditing(false);
        } catch (error) {
            console.error(error);
            setError("Failed to update your profile.");
        } finally {
            setSaving(false);
        }
    }

    return (
        <main className="profile-page page-container">
            <section className="profile-header">
                <div className="profile-avatar">
                    {user.profileImageUrl ? (
                        <img
                            src={user.profileImageUrl}
                            alt={displayNameFallback}
                        />
                    ) : (
                        <FiUser size={48}/>
                    )}
                </div>

                {!isEditing ? (
                    <>
                        <div className="profile-header-info">
                            <h1>{displayNameFallback}</h1>

                            <p className="profile-username">
                                @{user.userName}
                            </p>

                            {user.bio && (
                                <p className="profile-bio">
                                    {user.bio}
                                </p>
                            )}
                        </div>

                        <div className="profile-actions">
                            <button
                                className="profile-secondary-button"
                                type="button"
                                onClick={startEditing}
                            >
                                <FiEdit2 size={17}/>
                                Edit profile
                            </button>

                            <button
                                className="profile-logout-button"
                                type="button"
                                onClick={handleLogout}
                            >
                                <FiLogOut size={17}/>
                                Log out
                            </button>
                        </div>
                    </>
                ) : (
                    <form
                        className="profile-edit-form"
                        onSubmit={handleSave}
                    >
                        <div className="profile-form-group">
                            <label htmlFor="displayName">
                                Display name
                            </label>

                            <input
                                id="displayName"
                                type="text"
                                maxLength={50}
                                value={displayName}
                                onChange={(event) =>
                                    setDisplayName(event.target.value)
                                }
                                placeholder="Your display name"
                            />
                        </div>

                        <div className="profile-form-group">
                            <label htmlFor="bio">
                                Bio
                            </label>

                            <textarea
                                id="bio"
                                maxLength={1000}
                                rows={4}
                                value={bio}
                                onChange={(event) =>
                                    setBio(event.target.value)
                                }
                                placeholder="Tell people a little about yourself..."
                            />
                        </div>

                        <div className="profile-form-group">
                            <label htmlFor="profileImageUrl">
                                Profile image URL
                            </label>

                            <input
                                id="profileImageUrl"
                                type="url"
                                maxLength={500}
                                value={profileImageUrl}
                                onChange={(event) =>
                                    setProfileImageUrl(event.target.value)
                                }
                                placeholder="https://..."
                            />
                        </div>

                        {error && (
                            <p className="profile-form-error">
                                {error}
                            </p>
                        )}

                        <div className="profile-form-actions">
                            <button
                                className="profile-primary-button"
                                type="submit"
                                disabled={saving}
                            >
                                <FiSave size={17}/>
                                {saving ? "Saving..." : "Save changes"}
                            </button>

                            <button
                                className="profile-secondary-button"
                                type="button"
                                onClick={cancelEditing}
                                disabled={saving}
                            >
                                <FiX size={17}/>
                                Cancel
                            </button>
                        </div>
                    </form>
                )}
            </section>
        </main>
    );
}

export default ProfilePage;