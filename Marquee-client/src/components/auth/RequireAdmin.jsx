import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

function RequireAdmin({ children }) {
    const { isAdmin, loading } = useAuth();

    if (loading) return <div>Loading...</div>;
    if (!isAdmin) return <Navigate to="/" replace />;

    return children;
}

export default RequireAdmin;