import { React, useState } from 'react';
import {
    Card, CardBody, CardDeck, CardFooter, Col,
    Container, Modal, ModalBody, ModalFooter, ModalHeader, Row
} from 'reactstrap';
import { FaHamburger } from 'react-icons/fa';
import { CgCloseO } from 'react-icons/cg';
import './FoodDetails.css';

function FoodDetails(props) {
    const [isOpen, setIsOpen] = useState(true);
    const hideModal = () => {
        setIsOpen(false);
        props.onClose();
    }

    const showServingCard = (portionSize, price) => {
        return (
            <Card className='food-details-serving-card' key={1 + Math.random() * (1000 - 1)}>
                <CardBody className='food-details-serving'>
                    <span className='food-currency'>KM</span> {price}</CardBody>
                <CardFooter className='food-details-serving-footer'>{portionSize}</CardFooter>
            </Card>
        );
    }
    return (
        <Modal className='monteserrat-font-1-2em food-modal-content' isOpen={isOpen}>
            <ModalHeader className='food-details-header'>
                <CgCloseO className='food-details-close' onClick={hideModal} />
            </ModalHeader>
            <ModalBody>
                <Container fluid>
                    <Row>
                        <Col className='food-details-title'>
                            <FaHamburger className='food-details-icon' />
                            {props.menuItem.name}
                        </Col>
                    </Row>
                    <Row>
                        <Col className='food-details-text'>
                            {props.menuItem.description}
                        </Col>
                    </Row>
                    <Row>
                        <Col className='food-details-list'>
                            <p>Sastojci</p>
                            <hr className='food-details-white-separator' />
                            <ul>
                                {
                                    props.menuItem.ingredients.map(
                                        ingredient => (
                                            <li key={ingredient}>{ingredient}</li>
                                        )
                                    )
                                }
                            </ul>
                        </Col>
                    </Row>
                    <Row>
                        <Col className='food-details-list'>
                            <p>Porcije</p>
                            <hr className='food-details-white-separator' />
                            <Container fluid >
                                <CardDeck className='food-details-serving-cards'>
                                    {
                                        props.menuItem.servings.map(
                                            serving => (
                                                <>
                                                    {showServingCard(serving.name, serving.price)}
                                                </>)
                                        )
                                    }
                                </CardDeck>
                            </Container>
                        </Col>
                    </Row>
                </Container>
            </ModalBody>
            <ModalFooter className='food-details-footer' />
        </Modal >
    );
}

export default FoodDetails