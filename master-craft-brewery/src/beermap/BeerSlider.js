import React, { useEffect, useRef, useState } from 'react';
import BanjaluckiKraft from '../assets/slider/Banjaluckikraft-slider.jpg';
import Ipa from '../assets/slider/Ipa-slider.jpg';
import PaleAle from '../assets/slider/Paleale-slider.jpg';
import Pilsner from '../assets/slider/Pilsner-slider.jpg';
import Stout from '../assets/slider/Stout-slider.jpg';
import { IoArrowBack, IoArrowForward } from "react-icons/io5";

const slides = [
    { image: BanjaluckiKraft, alt: 'BanjaluckiKraft' },
    { image: Ipa, alt: 'Ipa' },
    { image: PaleAle, alt: 'PaleAle' },
    { image: Pilsner, alt: 'Pilsner' },
    { image: Stout, alt: 'Stout' }
];

export const BeerSlider = () => {
    const [current, setCurrent] = useState(0);
    const length = slides.length;
    const timeout = useRef(null);

    useEffect (() => {
        const nextSlide = () => {
            setCurrent(current => (current === length - 1 ? 0 : current + 1))
        };
         
        timeout.current = setTimeout(nextSlide, 2500);

        return function() {
            if(timeout.current){
                clearTimeout(timeout.current)
            }
        }
    }, [current, length]);

    const nextSlide = () =>{
        setCurrent(current === length - 1 ? 0 : current + 1);
        return function() {
            if(timeout.current){
                clearTimeout(timeout.current)
            }
        }
    }

    const prevSlide = () => {
        setCurrent(current === 0 ? length - 1 : current - 1);
        return function() {
            if(timeout.current){
                clearTimeout(timeout.current)
            }
        }
    }

    if(!Array.isArray(slides) || slides.length === 0){
        return null;
    }

    return(
        <section className="slider-section">
            <div className="slider-wrapper">
                {slides.map((slide, index) => {
                    return(
                        <div id="image-container" className="slider-slide" key={index}>
                            {index ===  current && (
                                <div className="slider">
                                    <img className="slider-image" src={slide.image} alt={slide.alt} />
                                </div>
                            )}
                        </div>
                    )
                })}
                <div className="slider-buttons">
                    <IoArrowBack className="slider-button dark-gray" onClick={prevSlide} />
                    <IoArrowForward className="slider-button dark-gray" onClick={nextSlide} />
                </div>
            </div>
        </section>
    )
}