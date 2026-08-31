import { useEffect, useState } from "react";

import PageContainer from "../components/layout/PageContainer";
import MediaRow from "../components/media/MediaRow";
import { getMedia } from "../api/mediaApi";

function HomePage() {
    const [media, setMedia] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        async function loadMedia() {
            try {
                const data = await getMedia();

                setMedia(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        }

        loadMedia();
    }, []);

    if (loading) {
        return (
            <PageContainer>
                <p>Loading...</p>
            </PageContainer>
        );
    }

    if (error) {
        return (
            <PageContainer>
                <p>Failed to load media.</p>
                <p>{error}</p>
            </PageContainer>
        );
    }

    return (
        <PageContainer>
            <section className="home-intro">
                <h1>Discover something worth watching.</h1>

                <p>
                    Explore movies and TV shows,
                    discover what people are watching,
                    and find your next favorite.
                </p>
            </section>

            <MediaRow
                title="Trending"
                media={media}
            />
        </PageContainer>
    );
}

export default HomePage;