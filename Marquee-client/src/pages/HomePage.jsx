import {useEffect, useState} from "react";
import PageContainer from "../components/layout/PageContainer";
import MediaRow from "../components/media/MediaRow";
import Hero from "../components/home/Hero.jsx";
import RecentReviews from "../components/home/RecentReviews";
import { getTrendingMedia, getFeaturedMedia } from "../api/mediaApi";

function HomePage() {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [trending, setTrending] = useState([]);
    const [featured, setFeatured] = useState(null);

    useEffect(() => {
        async function loadHomeData(){
            try
            {
                const [trendingData, featuredData] = 
                    await Promise.all([getTrendingMedia(20), getFeaturedMedia()]);
                setTrending(trendingData);
                setFeatured(featuredData);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        }
        loadHomeData()
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
            <Hero media={featured}/>

            <MediaRow
                title="Trending"
                media={trending}
            />
            <RecentReviews/>
        </PageContainer>
    );
}

export default HomePage;