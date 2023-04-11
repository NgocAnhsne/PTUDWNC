import React, { useEffect } from 'react';
const Rss = () => {
    useEffect(() => {
    document.title = 'RSS Feed';
    }, []);
    return (
    <h1>
        RSS Feed
    </h1>
    );
}
export default Rss;