import { React, useState, useEffect } from 'react';
import { Card } from 'reactstrap';
import { FaInfoCircle } from 'react-icons/fa';
import ScrollAnimation from 'react-animate-on-scroll';
import EventDetails from './EventDetails';
import './EventCard.css';
import axios from 'axios';
import { parseFromUTC } from '../common/DateTimeParser';
import { readImage } from '../common/ImageReader';
import { EVENT_PREFIX_ROUTE } from '../constants/ApiRoutes';
import './../common/Fonts.css'

function EventCard(props) {
    const [showModal, setShowModal] = useState(false);
    const closeModal = () => setShowModal(false);

    const [eventImage, setEventImage] = useState('');

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchImage() {
            await axios.get(`${EVENT_PREFIX_ROUTE}/${props.eventInfo.eventId}/image?Width=1000&Height=1000`,
                { responseType: 'blob' },
                { cancelToken: source.token }
            ).then((response) => readImage(response.data, (data) => setEventImage(data)));
        }
        fetchImage();
        return () => {
            setEventImage('');
            source.cancel();
        };
    }, [props.eventInfo.eventId]);

    return (
        <>
            {showModal && <EventDetails eventInfo={props.eventInfo} onClose={closeModal}></EventDetails>}
            <ScrollAnimation animateIn='fadeInUp'>
                <Card className='quote-font event-card'>
                    <div className='event-card-body'>
                        <div className='event-card-inner'>
                            <div className='event-card-front' style={{ backgroundImage: `linear-gradient(0deg, rgba(0, 0, 0, 0.75) 0%, rgba(0, 0, 0, 0.75) 50%, rgba(0, 0, 0, 0.75) 100%), url(data:image/jpeg;base64,${eventImage})` }}>
                                <p className='navbar-font'>{props.eventInfo.title}</p>
                            </div>
                            <div className='event-card-back'>
                                <p className='event-card-title navbar-font'>{props.eventInfo.title}</p>
                                <p className='event-card-date navbar-font'> {parseFromUTC(props.eventInfo.beginOn)} {props.eventInfo.endOn == null ? '' : ' - ' + parseFromUTC(props.eventInfo.endOn)}</p>
                                < button className='event-card-button' onClick={() => setShowModal(true)}>
                                    <FaInfoCircle className='event-card-button-icon' /> VIŠE
                                </button>
                            </div>
                        </div>
                    </div>
                </Card>
            </ScrollAnimation>
        </>
    )
}

export default EventCard
