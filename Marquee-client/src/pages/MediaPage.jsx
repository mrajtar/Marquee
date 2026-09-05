import {useContext, useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";

import PageContainer from "../components/layout/PageContainer";
import MediaHero from "../components/media/MediaHero";
import {getMediaDetails} from "../api/mediaApi";
import MediaReviews from "../components/media/MediaReviews";
import {useAuth} from "../context/AuthContext";
import {FiEdit2} from "react-icons/fi";

function MediaPage() {
    const {id} = useParams();
    const { isAuthenticated, isAdmin } = useAuth();

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
        <div className="media-page">
            
            <MediaHero media={media}/>

            <PageContainer>
                {isAuthenticated && isAdmin && (
                    <div className="media-admin-toolbar">
                        <Link
                            to={`/media/${media.id}/edit`}
                            className="admin-edit-button"
                        >
                            <FiEdit2 size={16} />
                            Edit
                        </Link>
                    </div>
                )}
                <MediaInformation media={media}/>
                <MediaReviews
                    mediaId={media.id}
                    reviewCount={
                        media.reviewCount
                    }
                />
            </PageContainer>
        </div>
    );
}

function MediaInformation({media}) {
    return (
        <section className="media-information">
            <section className="media-info-section">
                <h2>Genres</h2>

                <div className="tag-list">
                    {media.genres?.map((genre) => (
                        <span
                            key={genre.id}
                            className="media-tag"
                        >
                            {genre.name}
                        </span>
                    ))}
                </div>
            </section>
        </section>
    );
}

export default MediaPage;