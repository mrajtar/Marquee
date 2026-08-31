import { Link } from "react-router-dom";
import { FiUser, FiSearch } from "react-icons/fi";
function Navbar() {
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
                        className="navbar-search"
                        type="button"
                        aria-label="Search"
                        title="Search"
                    >
                        <FiSearch size={20} />
                    </button>

                    <button
                        className="navbar-user"
                        type="button"
                        aria-label="Account"
                        title="Account"
                    >
                        <FiUser size={24} />
                    </button>
                </div>
            </div>
        </header>
    );
}

export default Navbar;