import { React, useState, useEffect } from 'react';
import { FaInfoCircle } from 'react-icons/fa';
import { Card, CardBody, CardFooter, CardImg, CardTitle, Container, Row, Col } from 'reactstrap';
import '../common/Fonts.css';
import './FoodCard.css';
import FoodDetails from './FoodDetails';
import axios from 'axios';
import { PRODUCTS_PREFIX_ROUTE } from '../constants/ApiRoutes';
import { readImage } from '../common/ImageReader';

function FoodCard(props) {
    const [showModal, setShowModal] = useState(false);
    const closeModal = () => setShowModal(false);

    const [foodImage, setFoodImage] = useState('');

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchImage() {
            await axios.get(`${PRODUCTS_PREFIX_ROUTE}/${props.menuItem.productId}/image?Width=1000&Height=1000`,
                { responseType: 'blob' },
                { cancelToken: source.token }
            ).then((response) => readImage(response.data, (data) => setFoodImage(data)));
        }
        fetchImage();
        return () => {
            setFoodImage('');
            source.cancel();
        };
    }, [props.menuItem.productId]);

    return (
        <>
            {showModal && <FoodDetails menuItem={props.menuItem} onClose={closeModal}></FoodDetails>}
            <Card className='monteserrat-font food-card text-center' key={1 + Math.random() * (1000 - 1)}>
                <Container>
                    <Row>
                        <Col xs={2} sm={2} className='padding-0' />
                        <Col xs={8} sm={8} className='padding-0'>
                            <CardImg className='food-card-img' variant='top' src={`data:image/jpeg;base64,${foodImage}`} />
                        </Col>
                        <Col xs={2} sm={2} className='padding-0'>
                            <FaInfoCircle className='food-card-details-icon' onClick={() => setShowModal(true)}>
                            </FaInfoCircle>
                        </Col>
                    </Row>
                </Container>
                <CardBody className='food-card-body'>
                    <CardTitle className='food-card-title'>
                        {props.menuItem.name}
                    </CardTitle>
                    <CardFooter className='food-card-footer text-left' >
                        {props.menuItem.description}
                    </CardFooter>
                </CardBody>
            </Card >
        </>
    )
}

export default FoodCard
