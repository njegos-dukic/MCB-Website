import React from 'react'
import { GalleryCards } from './GalleryCards.js'
import './Gallery.css';
import { Quotes } from './Quotes.js'

export const GalleryQuotes = () => {
    return (
        <div className="gallery-and-quotes-background">
            <GalleryCards />
            <Quotes />
        </div>
    )
}
