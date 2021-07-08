import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { Row, Col } from 'reactstrap';
import { PRODUCTS_PREFIX_ROUTE } from '../constants/ApiRoutes';
import { readImage } from '../common/ImageReader';

export const Beer = (props) => {
    var beerClass = "beer-name";
    var lowercaseBeer = props.beer_prop.name.toLowerCase();
    if (lowercaseBeer.includes("craft") || lowercaseBeer.includes("kraft")) {
        beerClass += " bl-craft-blue-color";
    } else if (lowercaseBeer.includes("pilsner")) {
        beerClass += " pilsner-yellow-color";
    } else if (lowercaseBeer.includes("ipa")) {
        beerClass += " ipa-green-color";
    } else if (lowercaseBeer.includes("pale")) {
        beerClass += " pale-ale-bronze-color";
    } else {
        beerClass += " beer-name-default";
    }

    const [beerImage, setBeerImage] = useState('');

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchImage() {
            await axios.get(`${PRODUCTS_PREFIX_ROUTE}/${props.beer_prop.productId}/image?Width=500&Height=500`,
                { responseType: 'blob' },
                { cancelToken: source.token }
            ).then((response) => readImage(response.data, (data) => setBeerImage(data)));
        }
        fetchImage();
        return () => {
            setBeerImage('');
            source.cancel();
        };
    }, [props.beer_prop.productId]);

    return (
        <div className="beer-container">
            <Row xs="1" sm="1" md="2" lg="2" className="beer-row">
                <Col lg="7" md="6" className="beer-description-column">
                    <div>
                        <h2 className={beerClass}>{props.beer_prop.name}</h2>
                        <p className="beer-description monteserrat-font-1-2em">
                            {props.beer_prop.description}
                        </p>
                    </div>
                </Col>
                <Col lg="5" md="6" className="beer-image-column">
                    <img src={`data:image/jpeg;base64,${beerImage}`} alt="" className="beer-image" />
                </Col>
            </Row>
        </div>
    )
}
