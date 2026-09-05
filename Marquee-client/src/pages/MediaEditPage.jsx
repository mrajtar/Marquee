import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import { FiArrowLeft, FiSave } from "react-icons/fi";
import { getMediaDetails, getGenres, getKeywords, updateMedia } from "../api/mediaApi";
import PageContainer from "../components/layout/PageContainer";
import { useAuth } from "../context/AuthContext";

function MediaEditPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { isAdmin } = useAuth();

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");
    
    const [formData, setFormData] = useState({
        title: "",
        originalTitle: "",
        overview: "",
        posterUrl: "",
        backdropUrl: "",
        releaseDate: "",
        status: "",
        imdbId: "",
        tmdbId: "",
        runtimeMinutes: "",
        numberOfSeasons: "",
        numberOfEpisodes: "",
    });
    const [mediaType, setMediaType] = useState(1); // 1 = Movie, 2 = TV Show
    
    const [allGenres, setAllGenres] = useState([]);
    const [allKeywords, setAllKeywords] = useState([]);
    const [selectedGenreIds, setSelectedGenreIds] = useState([]);
    const [selectedKeywordIds, setSelectedKeywordIds] = useState([]);
    const [keywordSearch, setKeywordSearch] = useState("");

    useEffect(() => {
        async function loadData() {
            try {
                const [media, genres, keywords] = await Promise.all([
                    getMediaDetails(id),
                    getGenres(),
                    getKeywords(),
                ]);

                setMediaType(media.mediaType);

                setFormData({
                    title: media.title || "",
                    originalTitle: media.originalTitle || "",
                    overview: media.overview || "",
                    posterUrl: media.posterUrl || "",
                    backdropUrl: media.backdropUrl || "",
                    releaseDate: media.releaseDate ? media.releaseDate.slice(0, 10) : "",
                    status: media.status?.toString() || "",
                    imdbId: media.imdbId || "",
                    tmdbId: media.tmdbId?.toString() || "",
                    runtimeMinutes: media.runtimeMinutes?.toString() || "",
                    numberOfSeasons: media.numberOfSeasons?.toString() || "",
                    numberOfEpisodes: media.numberOfEpisodes?.toString() || "",
                });

                setAllGenres(genres);
                setAllKeywords(keywords);
                
                setSelectedGenreIds(media.genres?.map(g => g.id) || []);
                setSelectedKeywordIds(media.keywords?.map(k => k.id) || []);
            } catch (err) {
                console.error(err);
                setError("Failed to load media data.");
            } finally {
                setLoading(false);
            }
        }
        loadData();
    }, [id]);

    function handleChange(event) {
        const { name, value } = event.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    }

    function toggleGenre(genreId) {
        setSelectedGenreIds(prev =>
            prev.includes(genreId)
                ? prev.filter(id => id !== genreId)
                : [...prev, genreId]
        );
    }

    function toggleKeyword(keywordId) {
        setSelectedKeywordIds(prev =>
            prev.includes(keywordId)
                ? prev.filter(id => id !== keywordId)
                : [...prev, keywordId]
        );
    }

    async function handleSubmit(event) {
        event.preventDefault();
        setSaving(true);
        setError("");

        const payload = {
            title: formData.title,
            originalTitle: formData.originalTitle || null,
            overview: formData.overview || null,
            posterUrl: formData.posterUrl || null,
            backdropUrl: formData.backdropUrl || null,
            releaseDate: formData.releaseDate ? new Date(formData.releaseDate).toISOString() : null,
            status: formData.status ? parseInt(formData.status, 10) : null,
            imdbId: formData.imdbId || null,
            tmdbId: formData.tmdbId ? parseInt(formData.tmdbId, 10) : null,
            runtimeMinutes: mediaType === 1 && formData.runtimeMinutes ? parseInt(formData.runtimeMinutes, 10) : null,
            numberOfSeasons: mediaType === 2 && formData.numberOfSeasons ? parseInt(formData.numberOfSeasons, 10) : null,
            numberOfEpisodes: mediaType === 2 && formData.numberOfEpisodes ? parseInt(formData.numberOfEpisodes, 10) : null,
            genreIds: selectedGenreIds,
            keywordIds: selectedKeywordIds,
        };

        try {
            await updateMedia(id, payload);
            navigate(`/media/${id}`);
        } catch (err) {
            console.error(err);
            setError("Failed to update media.");
        } finally {
            setSaving(false);
        }
    }

    const filteredKeywords = allKeywords.filter(keyword =>
        keyword.name.toLowerCase().includes(keywordSearch.toLowerCase())
    );

    if (loading) return <PageContainer><p>Loading...</p></PageContainer>;
    if (!isAdmin) return <PageContainer><p>You do not have permission to edit this media.</p></PageContainer>;

    return (
        <PageContainer>
            <Link to={`/media/${id}`} className="media-back-link">
                <FiArrowLeft size={17} /> Back to media
            </Link>
            <h1>Edit Media</h1>

            {error && <p className="form-error">{error}</p>}

            <form onSubmit={handleSubmit} className="media-edit-form">
                <div className="profile-form-group">
                    <label>Title</label>
                    <input type="text" name="title" value={formData.title} onChange={handleChange} required />
                </div>
                <div className="profile-form-group">
                    <label>Original Title</label>
                    <input type="text" name="originalTitle" value={formData.originalTitle} onChange={handleChange} />
                </div>
                <div className="profile-form-group">
                    <label>Overview</label>
                    <textarea name="overview" rows={6} value={formData.overview} onChange={handleChange} />
                </div>
                <div className="profile-form-group">
                    <label>Poster URL</label>
                    <input type="url" name="posterUrl" value={formData.posterUrl} onChange={handleChange} />
                </div>
                <div className="profile-form-group">
                    <label>Backdrop URL</label>
                    <input type="url" name="backdropUrl" value={formData.backdropUrl} onChange={handleChange} />
                </div>
                <div className="profile-form-group">
                    <label>Release Date</label>
                    <input type="date" name="releaseDate" value={formData.releaseDate} onChange={handleChange} />
                </div>
                
                <div className="profile-form-group">
                    <label>Status</label>
                    <select name="status" value={formData.status} onChange={handleChange}>
                        <option value="">Select status</option>
                        <option value="1">Planned</option>
                        <option value="2">In Production</option>
                        <option value="3">Released</option>
                        <option value="4">Ongoing</option>
                        <option value="5">Ended</option>
                        <option value="6">Cancelled</option>
                    </select>
                </div>

                <div className="profile-form-group">
                    <label>IMDb ID</label>
                    <input type="text" name="imdbId" value={formData.imdbId} onChange={handleChange} />
                </div>
                <div className="profile-form-group">
                    <label>TMDB ID</label>
                    <input type="number" name="tmdbId" value={formData.tmdbId} onChange={handleChange} />
                </div>
                
                {mediaType === 1 && (
                    <div className="profile-form-group">
                        <label>Runtime (minutes)</label>
                        <input type="number" name="runtimeMinutes" value={formData.runtimeMinutes} onChange={handleChange} />
                    </div>
                )}
                {mediaType === 2 && (
                    <>
                        <div className="profile-form-group">
                            <label>Number of Seasons</label>
                            <input type="number" name="numberOfSeasons" value={formData.numberOfSeasons} onChange={handleChange} />
                        </div>
                        <div className="profile-form-group">
                            <label>Number of Episodes</label>
                            <input type="number" name="numberOfEpisodes" value={formData.numberOfEpisodes} onChange={handleChange} />
                        </div>
                    </>
                )}

                <div className="profile-form-group">
                    <label>Genres</label>
                    <div className="checkbox-list">
                        {allGenres.map(genre => (
                            <label key={genre.id} className="checkbox-item">
                                <input
                                    type="checkbox"
                                    checked={selectedGenreIds.includes(genre.id)}
                                    onChange={() => toggleGenre(genre.id)}
                                />
                                {genre.name}
                            </label>
                        ))}
                    </div>
                </div>
                
                <div className="profile-form-group">
                    <label>Keywords</label>
                    <input
                        type="text"
                        placeholder="Search keywords..."
                        value={keywordSearch}
                        onChange={(e) => setKeywordSearch(e.target.value)}
                    />
                    <div className="checkbox-list keyword-checkbox-list">
                        {filteredKeywords.map(keyword => (
                            <label key={keyword.id} className="checkbox-item">
                                <input
                                    type="checkbox"
                                    checked={selectedKeywordIds.includes(keyword.id)}
                                    onChange={() => toggleKeyword(keyword.id)}
                                />
                                {keyword.name}
                            </label>
                        ))}
                    </div>
                </div>

                <div className="profile-form-actions">
                    <button type="submit" className="profile-primary-button" disabled={saving}>
                        <FiSave size={17} />
                        {saving ? "Saving..." : "Save Changes"}
                    </button>
                </div>
            </form>
        </PageContainer>
    );
}

export default MediaEditPage;