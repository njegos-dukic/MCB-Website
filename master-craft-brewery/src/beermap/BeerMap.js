import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { CardDeck } from 'reactstrap';
import { Beer } from './Beer';
import './BeerMap.css';
import { BeerSlider } from './BeerSlider';
import Cocktail from './Cocktail';
import { PRODUCTS_OF_TYPE_PREFIX_ROUTE } from '../constants/ApiRoutes';
import ScrollAnimation from 'react-animate-on-scroll';

export const BeerMap = () => {
    const [beers, setBeers] = useState([]);
    const [cocktails, setCocktails] = useState([]);

    useEffect(() => {
        const cancelTokenBeer = axios.CancelToken;
        const sourceBeer = cancelTokenBeer.source();

        const cancelTokenCoctail = axios.CancelToken;
        const sourceCoctail = cancelTokenCoctail.source();

        async function fetchData() {
            await axios.get(`${PRODUCTS_OF_TYPE_PREFIX_ROUTE}Pivo`, { cancelToken: sourceBeer.token })
                .then((response) => setBeers(response.data));
            await axios.get(`${PRODUCTS_OF_TYPE_PREFIX_ROUTE}Koktel`, { cancelToken: sourceCoctail.token })
                .then((response) => setCocktails(response.data));
        }
        fetchData();

        return () => {
            setBeers([]);
            setCocktails([]);
            sourceBeer.cancel();
            sourceCoctail.cancel();
        }
    }, []);

    return (
        <div className="beer-map-background">
            <BeerSlider />
            <h1 className="monteserrat-font-3em beer-map-title">
                Karta piva
            </h1>
            {beers.map((beer, index) => {
                return (
                    <ScrollAnimation key={index} animateIn='backInLeft' animateOnce={true}>
                        <Beer beer_prop={beer} key={index} />
                    </ScrollAnimation>
                )
            })}
            <h1 className="monteserrat-font-3em beer-map-title">
                Naši kokteli
            </h1>
            <div className="coctails-container">
                <CardDeck className="deck-coctails">
                    {cocktails.map((cocktail, index) => {
                        return (
                            <ScrollAnimation animateIn='flipInY' animateOnce={true}>
                                <Cocktail cocktail_prop={cocktail} key={index} />
                            </ScrollAnimation>
                        )
                    })}
                </CardDeck>
            </div>
        </div>
    );
}
