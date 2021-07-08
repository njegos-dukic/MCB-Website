import React, { useCallback, useEffect, useState } from 'react';
import { Card, CardDeck, CardBody, Container } from 'reactstrap';
import { useHistory, useLocation } from 'react-router-dom';
import { FaBuilding, FaHamburger, FaImages, FaUser } from 'react-icons/fa';
import { MdEvent } from 'react-icons/md';
import { IoMdBeer } from 'react-icons/io';
import { GiMeal } from 'react-icons/gi';
import { GrBlockQuote } from 'react-icons/gr';
import AdminLayout from './AdminLayout.js';
import './Home.css';
import { LOGIN_PREFIX_ROUTE } from '../constants/ApiRoutes.js';
import axios from 'axios';

function Home(props) {
    const [token, setToken] = useState();
    const location = useLocation();
    const history = useHistory();

    const redirectToOption = (url) => {
        history.push({ pathname: url, state: { token: token } });
    }

    const redirectToLogin = useCallback(() => {
        history.push({ pathname: `/admin` });
    }, [history]);

    const renderCard = (url, title, icon) => {
        return (
            <Card className='admin-card'>
                <CardBody className='admin-card-body'>
                    {icon}
                    <a className='admin-card-url' onClick={() => redirectToOption(url)}>{title}</a>
                </CardBody>
            </Card>)
    }

    useEffect(() => {
        async function fetchData() {
            // Check if user is logged in, and if it is send get request
            await axios.get(LOGIN_PREFIX_ROUTE, { headers: { 'Authorize': location.state.token } })
                .catch((error) => redirectToLogin());
        }
        if (location.state !== undefined && location.state.token !== undefined) {
            fetchData();
            setToken(location.state.token);
        }
        else
            redirectToLogin();
        return () => {
            setToken();
        }
    }, [location, redirectToLogin])

    return (
        <AdminLayout>
            <Container fluid>
                <CardDeck className='admin-cards quote-font'>
                    {renderCard('/admin/company', 'Kompanija', <FaBuilding className='admin-card-icon' />)}
                    {renderCard('/admin/administrators', 'Administratori', <FaUser className='admin-card-icon' />)}
                    {renderCard('/admin/events', ' Događaji', <MdEvent className='admin-card-icon' />)}
                    {renderCard('/admin/gallery', 'Slike', <FaImages className='admin-card-icon' />)}
                    {renderCard('/admin/quotes', 'Citati', <GrBlockQuote className='admin-card-icon' />)}
                    {renderCard('/admin/food-menu', 'Jelovnik', <FaHamburger className='admin-card-icon' />)}
                    {renderCard('/admin/products', 'Proizvodi', <IoMdBeer className='admin-card-icon' />)}
                    {renderCard('/admin/product-options', 'Tipovi proizvoda i porcije', <GiMeal className='admin-card-icon' />)}
                </CardDeck>``
            </Container>
        </AdminLayout>
    )
}

export default Home
