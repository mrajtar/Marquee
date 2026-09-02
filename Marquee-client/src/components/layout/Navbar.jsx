import { Link } from "react-router-dom";
import { FiUser, FiSearch } from "react-icons/fi";
import { useAuth } from "../../context/AuthContext"

function Navbar() {
    const { isAuthenticated, user } = useAuth();
    
    return (
        <header className="navbar">
            <div className="navbar-inner">
                <Link to="/" className="navbar-logo">
                    MARQUEE
                </Link>

                <nav className="navbar-links">
                    <Link to="/movies">Movies</Link>
                    <Link to="/tv-shows">TV Shows</Link>
                    <Link to="/lists">Lists</Link>
                </nav>

                <div className="navbar-actions">
                    <button
                        className="navbar-icon-button"
                        type="button"
                        aria-label="Search"
                        title="Search"
                    >
                        <FiSearch size={20} />
                    </button>

                    <Link
                        to={isAuthenticated ? "/profile" : "/login"}
                        className="navbar-icon-button"
                        aria-label="Account"
                        title={isAuthenticated ? user?.displayName || user.userName || "Profile" : "Log in"}
                    >
                        <FiUser size={22} />
                    </Link>
                </div>
            </div>
        </header>
    );
}

export default Navbar;