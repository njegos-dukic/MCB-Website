import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { CardBody, CardImg, CardText, CardTitle } from 'reactstrap';
import { readImage } from '../common/ImageReader';
import { PRODUCTS_PREFIX_ROUTE } from '../constants/ApiRoutes';

export default function Cocktail(props) {
    const [cocktailImage, setCocktailImage] = useState('');

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchImage() {
            await axios.get(`${PRODUCTS_PREFIX_ROUTE}/${props.cocktail_prop.productId}/image?Width=500&Height=500`,
                { responseType: 'blob' },
                { cancelToken: source.token }
            ).then((response) => readImage(response.data, (data) => setCocktailImage(data)));
        }
        fetchImage();
        return () => {
            setCocktailImage('');
            source.cancel();
        };
    }, [props.cocktail_prop.productId]);

    return (
        <div className="coctail-card">
            <CardImg top width="100%" src={`data:image/jpeg;base64,${cocktailImage}`} alt="coctail" />
            <CardBody>
                <CardTitle tag="h2" className="monteserrat-font-1-8em ">{props.cocktail_prop.name}</CardTitle>
                {/* <CardSubtitle tag="h6" className="mb-2 text-muted">Card subtitle</CardSubtitle> */}
                <hr />
                <CardText >
                    {props.cocktail_prop.description}
                </CardText>
            </CardBody>
        </div>
    )
}
