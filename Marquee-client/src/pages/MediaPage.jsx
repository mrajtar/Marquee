import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import PageContainer from "../components/layout/PageContainer";
import { getMediaDetails } from "../api/mediaApi";

function MediaPage() {
    const { id } = useParams();

    const [media, setMedia] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        async function loadMedia() {
            try {
                const data = await getMediaDetails(id);

                setMedia(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        }

        loadMedia();
    }, [id]);

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

    if (!media) {
        return (
            <PageContainer>
                <p>Media not found.</p>
            </PageContainer>
        );
    }

    return (
        <PageContainer>
            <h1>{media.title}</h1>

            <p>{media.overview}</p>
        </PageContainer>
    );
}

export default MediaPage;