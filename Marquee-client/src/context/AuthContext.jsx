import { createContext, useContext, useState } from "react";

const AuthContext = createContext(null);

function AuthProvider({ children }) {
    const [token, setToken] = useState(
        localStorage.getItem("accessToken")
    );

    function login(accessToken) {
        localStorage.setItem(
            "accessToken",
            accessToken
        );

        setToken(accessToken);
    }

    function logout() {
        localStorage.removeItem("accessToken");

        setToken(null);
    }

    return (
        <AuthContext.Provider
            value={{
                token,
                isAuthenticated: Boolean(token),
                login,
                logout
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}

export default AuthProvider;