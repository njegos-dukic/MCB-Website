import { React, useState, useEffect } from 'react';
import { CardDeck, Col, Container, Row } from 'reactstrap';
import './Events.css';
import EventCard from './EventCard';
import ScrollAnimation from 'react-animate-on-scroll';
import { FaQuoteLeft } from 'react-icons/fa';
import axios from 'axios';
import { EVENT_PREFIX_ROUTE } from '../constants/ApiRoutes';
import './../common/Fonts.css'

function Events() {
    const [events, setEvents] = useState([]);

    const showCards = (isUpcoming = false) => {
        return events.filter(x => x.isUpcoming === isUpcoming)
            .map(x => {
                return (<EventCard eventInfo={x} key={1 + Math.random() * (1000 - 1)} />);
            }
            );
    }

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchData() {
            await axios.get(EVENT_PREFIX_ROUTE, { cancelToken: source.token })
                .then((response) => setEvents(response.data));
        }
        fetchData();
        return () => {
            setEvents([]);
            source.cancel();
        };
    }, []);

    return (
        <Container fluid className='quote-font events-container'>
            <Row className=' events-row-heading'>
                <Col className='events-heading'>
                    <ScrollAnimation animateIn='fadeIn' duration={2}>
                        <FaQuoteLeft></FaQuoteLeft>
                        <p className='quote-font'>
                            Učestvujte u nagradnim igrama, u PUB kvizovima, muzici uživo i drugim događajima
                            za koje je odgovoran MCB.
                            <br />
                            U divnom ambijentu MCB pivare možete od sada da proslavljate rođendane,
                            mature, da organizujete poslovne sastanke i sve ostale događaje.
                            Rezervaciju obavite preko telefona, porukom ili uživo, a nama prepustite
                            da se pobrinemo za vrhunsku hranu i pivo.
                        </p>
                    </ScrollAnimation>
                </Col>
            </Row>
            <Row>
                <Col>
                    <h2 className='events-title quote-font'>Predstojeći događaji</h2>
                    <hr className='events-separator' />
                    <CardDeck className='events-deck'>
                        {showCards(true)}
                    </CardDeck>
                </Col>
            </Row>
            <Row>
                <Col>
                    <h2 className='events-title quote-font'>Protekli događaji</h2>
                    <hr className='events-separator' />
                    <CardDeck className='events-deck'>
                        {showCards()}
                    </CardDeck>
                </Col>
            </Row>
        </Container >
    )
}

export default Events
