import {useState} from "react";
import {Link, useLocation, useNavigate} from "react-router-dom";
import {FiLogIn} from "react-icons/fi";

import {useAuth} from "../context/AuthContext";
import {loginUser} from "../api/authApi";

function LoginPage() {
    const navigate = useNavigate();
    const location = useLocation();

    const from = location.state?.from?.pathname || "/";

    const {login} = useAuth();

    const [username, setUsername] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [error, setError] =
        useState(null);

    const [loading, setLoading] =
        useState(false);

    async function handleSubmit(event) {
        event.preventDefault();

        setError(null);
        setLoading(true);

        try {
            const data = await loginUser(
                username,
                password
            );

            login(data.accessToken);

            navigate(from, {replace: true});
        } catch (error) {
            console.error("Login failed:", error);
            setError(
                "Invalid username or password."
            );
        } finally {
            setLoading(false);
        }
    }

    return (
        <main className="auth-page">
            <div className="auth-card">
                <div className="auth-heading">
                    <FiLogIn size={22}/>
                    <h1>Log in</h1>
                </div>
                <p className="auth-description">
                    Sign in to rate movies, write reviews, create lists and more.
                </p>
                <form className="auth-form" onSubmit={handleSubmit}>
                    <label>
                        <span>Username</span>

                        <input
                            type="text"
                            value={username}
                            onChange={(event) =>
                                setUsername(
                                    event.target.value
                                )
                            }
                            autoComplete="username"
                            required
                        />
                    </label>

                    <label>
                        <span>Password</span>

                        <input
                            type="password"
                            value={password}
                            onChange={(event) =>
                                setPassword(
                                    event.target.value
                                )
                            }
                            autoComplete="current-password"
                            required
                        />
                    </label>

                    {error && (
                        <p className="form-error">
                            {error}
                        </p>
                    )}

                    <button
                        type="submit"
                        className="primary-button auth-submit"
                        disabled={loading}
                    >
                        {loading
                            ? "Logging in..."
                            : "Log in"}
                    </button>
                </form>
                <p className="auth-switch">
                    Don't have an account?{" "}
                    <Link to="/register">
                        Register
                    </Link>
                </p>
            </div>
        </main>
    );
}

export default LoginPage;