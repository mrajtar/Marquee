import { Routes, Route } from "react-router-dom";

import Navbar from "./components/layout/Navbar";
import HomePage from "./pages/HomePage";
import MediaPage from "./pages/MediaPage";
import LoginPage from "./pages/LoginPage";
import ReviewDetailPage from "./pages/ReviewDetailPage";
import RegisterPage from "./pages/RegisterPage";

function App() {
    return (
        <>
            <Navbar />

            <Routes>
                <Route
                    path="/"
                    element={<HomePage />}
                />

                <Route
                    path="/media/:id"
                    element={<MediaPage />}
                />
                
                <Route
                    path="/login"
                    element={<LoginPage />}
                />
                
                <Route
                    path="/reviews/:reviewId"
                    element={<ReviewDetailPage />}
                />
                
                <Route
                    path="/register"
                    element={<RegisterPage />}
                />
            </Routes>
        </>
    );
}

export default App;