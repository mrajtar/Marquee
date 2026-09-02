import { useAuth } from "../context/AuthContext";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

function ProfilePage() {
    const { user, logout, loading } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!loading && !user) {
            navigate("/", { replace: true });
        }
    }, [user, loading, navigate]);

    function handleLogout() {
        logout();
        setTimeout(() => {
            navigate("/", { replace: true });
        }, 100);
    }

    if (loading) {
        return (
            <main className="page-container">
                <p>Loading...</p>
            </main>
        );
    }

    if (!user) {
        return null;
    }
    
    return (
        <main className="page-container">
            <h1>{user.displayName || user.userName}</h1>

            <p>@{user.userName}</p>

            {user.bio && <p>{user.bio}</p>}

            <button onClick={handleLogout}>
                Log out
            </button>
        </main>
    );
}

export default ProfilePage;