import { useState } from "react";
import {
    Link,
    useNavigate
} from "react-router-dom";
import { FiUserPlus } from "react-icons/fi";

import { registerUser } from "../api/authApi";

function RegisterPage() {
    const navigate = useNavigate();

    const [username, setUsername] =
        useState("");

    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [confirmPassword, setConfirmPassword] =
        useState("");

    const [error, setError] =
        useState(null);

    const [loading, setLoading] =
        useState(false);

    async function handleSubmit(event) {
        event.preventDefault();

        setError(null);

        if (
            password !==
            confirmPassword
        ) {
            setError(
                "Passwords do not match."
            );

            return;
        }

        setLoading(true);

        try {
            await registerUser(
                username,
                email,
                password
            );

            navigate("/login");
        } catch (error) {
            console.error(
                "Registration failed:",
                error
            );

            setError(
                "Registration failed. Please check your information and try again."
            );
        } finally {
            setLoading(false);
        }
    }

    return (
        <main className="auth-page">
            <div className="auth-card">
                <div className="auth-heading">
                    <FiUserPlus size={22} />

                    <h1>Create account</h1>
                </div>

                <p className="auth-description">
                    Join Marquee and keep track of
                    what you watch.
                </p>

                <form
                    className="auth-form"
                    onSubmit={handleSubmit}
                >
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
                        <span>Email</span>

                        <input
                            type="email"
                            value={email}
                            onChange={(event) =>
                                setEmail(
                                    event.target.value
                                )
                            }
                            autoComplete="email"
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
                            autoComplete="new-password"
                            required
                        />
                    </label>

                    <label>
                        <span>
                            Confirm password
                        </span>

                        <input
                            type="password"
                            value={
                                confirmPassword
                            }
                            onChange={(event) =>
                                setConfirmPassword(
                                    event.target.value
                                )
                            }
                            autoComplete="new-password"
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
                            ? "Creating account..."
                            : "Create account"}
                    </button>
                </form>

                <p className="auth-switch">
                    Already have an account?{" "}
                    <Link to="/login">
                        Log in
                    </Link>
                </p>
            </div>
        </main>
    );
}

export default RegisterPage;