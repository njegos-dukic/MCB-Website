import React from 'react';
import { FaQuoteLeft } from 'react-icons/fa';
import './FoodQuote.css';
import defaultImage from '../assets/food-menu-quote-chicken.jpg';

/**
 * 
 * @param {object} props Component props
 * @param {string} props.quote Quote to display
 * @param {string} props.author Author of the quote
 * @param {string} props.imageUrl Url for image to display as background
 * @returns 
 */
function FoodQuote(props) {
    return (
        <div className='food-qoute-container' style={{
            backgroundImage: `linear-gradient(0deg, rgba(0,0,0,0.45) 0%, rgba(0,0,0,0.45) 50%,rgba(0,0,0,0.45) 100% ), url(${(props.imageUrl) || defaultImage})`
        }}>
            <FaQuoteLeft className='food-qoute-sign' />
            <div className='food-qoute-text'>
                {props.quote}
                <p className='food-quote-author'>
                    {props.author}
                </p>
            </div>
        </div >
    )
}

export default FoodQuote
