import React, { useEffect, useState } from 'react'
import {
    CardText, 
    CardColumns,
} from 'reactstrap';
import './Quote.css'
import { FaQuoteLeft } from "react-icons/fa";
import axios from 'axios';
import { QUOTES_PREFIX_ROUTE } from '../constants/ApiRoutes';

export const Quotes = () => {
    const [quotes, setQuotes] = useState([]);

    useEffect(() => {
        const cancelTokenQuote = axios.CancelToken;
        const sourceQuote = cancelTokenQuote.source();

        async function fetchData() {
            await axios.get(`${QUOTES_PREFIX_ROUTE}`, { cancelToken: sourceQuote.token })
                .then((response) => setQuotes(response.data));
        }
        fetchData();

        return () => {
            setQuotes([]);
            sourceQuote.cancel();
        }
    }, []);

    return (
        <div className="card-columns-quotes-container">
            <h2 className="quote-h3-title">
                Ako se ikada budete dvoumili da li da popijete čašu dobrog piva ili ne, sjetite se ovih riječi...
            </h2>
            <div className="card-columns-quotes-container" >
                <CardColumns>
                    {quotes.map((quote, index) =>{
                        return(
                            <div className="quote-card" key={index}>
                                <div className="quote-left-icon">
                                    <FaQuoteLeft />
                                </div>
                                <div className="quote-text">
                                    {quote.quoteText}
                                </div>
                                <CardText className="quote-author">
                                    {quote.author}
                                </CardText>
                            </div>
                        )
                    })}
                </CardColumns>
            </div>
        </div>
    )
}
