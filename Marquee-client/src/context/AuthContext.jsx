import { createContext, useContext, useState, useEffect } from "react";
import { getMe } from "../api/userApi";

const AuthContext = createContext(null);

function AuthProvider({ children }) {
    const [token, setToken] = useState(
        localStorage.getItem("accessToken")
    );
    
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {
        async function loadUser() {
            if (!token) {
                setUser(null);
                setLoading(false);
                return;
            }
            try {
                const currentUser = await getMe();
                setUser(currentUser);
            }
            catch (error) {
                console.log("Failed to load current user:", error);
                localStorage.removeItem("accessToken");
                setToken(null);
                setUser(null);
            }
            finally {
                setLoading(false);
            }
        }
        loadUser();
    }, [token]);
    
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
        setUser(null);
    }

    function updateUser(userData) {
        setUser(userData);
    }

    return (
        <AuthContext.Provider
            value={{
                token,
                user,
                loading,
                isAuthenticated: Boolean(token && user),
                login,
                logout,
                updateUser
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