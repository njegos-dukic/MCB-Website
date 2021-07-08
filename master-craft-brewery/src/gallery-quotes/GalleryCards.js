import React, { useEffect, useState } from 'react'
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";
import { GalleryCard } from './GalleryCard';
import { GALLERIES_PREFIX_ROUTE } from '../constants/ApiRoutes';
import axios from 'axios';

export const GalleryCards = () => {
    const [galleries, setGalleries] = useState([]);

    useEffect(() => {
        const cancelTokenGallery = axios.CancelToken;
        const sourceGallery = cancelTokenGallery.source();

        async function fetchData() {
            await axios.get(`${GALLERIES_PREFIX_ROUTE}`, { cancelToken: sourceGallery.token })
                .then((response) => setGalleries(response.data));
        }
        fetchData();

        return () => {
            setGalleries([]);
            sourceGallery.cancel();
        }
    }, []);


    return (
        <div className="gallery-cards-container">
            <Carousel
                additionalTransfrom={0}
                arrows
                autoPlay
                autoPlaySpeed={2200}
                centerMode={false}
                className=""
                containerClass="container-with-dots"
                dotListClass=""
                draggable
                focusOnSelect={false}
                infinite
                itemClass=""
                keyBoardControl
                minimumTouchDrag={80}
                renderButtonGroupOutside={false}
                renderDotsOutside={false}
                responsive={{
                    desktop: {
                        breakpoint: {
                            max: 3000,
                            min: 1024
                        },
                        items: 3,
                        partialVisibilityGutter: 40
                    },
                    tablet: {
                        breakpoint: {
                            max: 1024,
                            min: 630
                        },
                        items: 2,
                        partialVisibilityGutter: 30
                    },
                    mobile: {
                        breakpoint: {
                            max: 630,
                            min: 0
                        },
                        items: 1.1,
                        partialVisibilityGutter: 30
                    }
                }}
                showDots={false}
                sliderClass=""
                slidesToSlide={1}
                swipeable
                >
                    {galleries.map((gallery, index) => {
                        if(gallery.thumbnail !== null) {
                            return(
                                <GalleryCard
                                    key={index} 
                                    id={gallery.galleryId}
                                    image={gallery.thumbnail.photoInfo.fileData}
                                    title={gallery.name}
                                    description={gallery.description}    
                                />
                            )
                        }
                    })}
            </Carousel>
        </div>
    )
}