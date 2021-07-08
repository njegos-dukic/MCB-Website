import React from 'react'
import { CardImg, CardText, CardImgOverlay, CardTitle } from 'reactstrap';
import {IoIosArrowForward} from "react-icons/io";
import { Link } from 'react-router-dom';

export const GalleryCard = (props) => {
    return (
        <div className="gallery-card">
            <CardImg className="gallery-card-img" width="100%" height="100%" src={`data:image/jpeg;base64,${props.image}`} alt="coctail" />
            <CardImgOverlay className="gallery-card-overlay">
                <CardTitle className="gallery-card-title" tag="h5">{props.title}</CardTitle>
                <CardText className="gallery-card-description">
                    {props.description}
                </CardText>
                <Link to={`/gallery/${props.id}`} className="gallery-card-link">Pregledaj fotografije<IoIosArrowForward/></Link>
            </CardImgOverlay>
        </div>
    )
}
