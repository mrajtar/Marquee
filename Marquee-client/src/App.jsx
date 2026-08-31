import { Routes, Route } from "react-router-dom";

import Navbar from "./components/layout/Navbar";
import HomePage from "./pages/HomePage";
import MediaPage from "./pages/MediaPage";

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
            </Routes>
        </>
    );
}

export default App;