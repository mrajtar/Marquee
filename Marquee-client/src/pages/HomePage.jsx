import {useEffect, useState} from "react";

import PageContainer from "../components/layout/PageContainer";
import MediaRow from "../components/media/MediaRow";
import {getMedia} from "../api/mediaApi";
import Hero from "../components/home/Hero.jsx";
import RecentReviews from "../components/home/RecentReviews";

function HomePage() {
    const [media, setMedia] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const featuredMedia = media[0];

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
            <Hero media={featuredMedia}/>

            <MediaRow
                title="Trending"
                media={media}
            />

            <RecentReviews/>
        </PageContainer>
    );
}

export default HomePage;