import MediaCard from "./MediaCard";

function MediaRow({ title, media }) {
    return (
        <section className="media-section">
            <div className="media-section-header">
                <h2>{title}</h2>
            </div>

            <div className="media-row">
                {media.map((item) => (
                    <MediaCard
                        key={item.id}
                        media={item}
                    />
                ))}
            </div>
        </section>
    );
}

export default MediaRow;