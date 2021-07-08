import { React, useState } from 'react';
import { Col, Container, Modal, ModalBody, ModalFooter, ModalHeader, Row } from 'reactstrap';
import { CgCloseO } from 'react-icons/cg';
import './EventDetails.css';
import { parseFromUTC } from '../common/DateTimeParser';

function EventDetails(props) {
    const [isOpen, setIsOpen] = useState(true);
    const hideModal = () => {
        setIsOpen(false);
        props.onClose();
    }
    let modalSize = props.eventInfo.description.length < 50 ? 'sm' : 'lg';
    return (
        <Modal size={modalSize} className='quote-font event-modal-content' isOpen={isOpen} centered={true}>
            <ModalHeader className='event-details-header'>
                <CgCloseO className='event-details-close' onClick={hideModal} />
            </ModalHeader>
            <ModalBody>
                <Container fluid>
                    <Row>
                        <Col className='event-details-title'>
                            {props.eventInfo.title}
                        </Col>
                    </Row>
                    <Row>
                        <Col className='event-details-date'>
                            Od: {parseFromUTC(props.eventInfo.beginOn)}
                        </Col>
                    </Row>
                    <Row>
                        <Col className='event-details-date'>
                            Do: {parseFromUTC(props.eventInfo.endOn)}
                        </Col>
                    </Row>
                    <Row>
                        <Col className='event-duration'>
                            <b>Trajanje:</b> {props.eventInfo.duration !== undefined ? props.eventInfo.duration + 'h' : '---'}
                        </Col>
                        <Col className='event-price'>
                            <b>Cijena:</b> {props.eventInfo.price === 0.0 ? 'Besplatno' : props.eventInfo.price}
                        </Col>
                    </Row>
                    <Row>
                        <Col >
                            <hr className='event-details-white-separator' />
                            <div className='event-details-description'>
                                {props.eventInfo.description}
                            </div>
                        </Col>
                    </Row>
                </Container>
            </ModalBody>
            <ModalFooter className='event-details-footer' />
        </Modal >
    )
}

export default EventDetails
