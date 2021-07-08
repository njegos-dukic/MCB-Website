import React from 'react'
import { AboutUs } from './AboutUs';
import './Home.css';
import { Row, Col } from 'reactstrap';
import ScrollAnimation from 'react-animate-on-scroll';
import { ReactComponent as LogoSVG } from "../assets/MCB.svg";
import { Link } from 'react-router-dom';
import './../common/Fonts.css'

export const HomePage = () => {
    return (
        <div className="home-page-container">
                <div className="home-page-header-container">
                    <Row lg="1" md="1" sm="1" xs="1" className="home-page-row">
                        <ScrollAnimation animateIn='fadeIn' animateOnce={true}>
                            <Col className="home-page-logo-col">
                                <LogoSVG className="home-page-logo" viewBox="0 -10 240 -5" />
                            </Col>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <Col>
                                <h1 className="home-page-h1 navbar-font">POSJETITE NAŠ ONLINE SHOP!</h1>
                            </Col>
                        </ScrollAnimation>
                        <ScrollAnimation  animateIn='backInRight' animateOnce={true}>
                            <Col>
                                <Link to='/shop' className="home-page-link">
                                    <button className="home-page-button">
                                            PORUČI PIVO
                                    </button>
                                </Link>
                            </Col>
                        </ScrollAnimation>
                    </Row>
                </div>
            <AboutUs />
        </div>
    )
}
