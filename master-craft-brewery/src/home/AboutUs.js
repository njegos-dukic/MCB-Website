import React from 'react'
import { Container, Row, Col } from  'reactstrap';
import About1 from '../assets/home/about-1.jpg';
import About2 from '../assets/home/about-2.jpg';
import About3 from '../assets/home/about-3.jpg';
import Production from '../assets/home/kako-mi-to-radimo_auto_x2.jpg'
import './AboutUs.css';
import ScrollAnimation from 'react-animate-on-scroll';


export const AboutUs = () => {
    return (
        <div className="about-us-container">
            <Container className="about-us-reactstrap-container">
                <Row>
                    <Col lg="6">
                        <img className="about-us-image-left" src={About1} alt="about" />
                    </Col>
                    <Col className="about-us-text-container" lg="6">
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Naša piva proizvodimo od izuzetno kvalitetnih prvobitnih sirovina: <u className="home-font">slad, hmelj, kvacas i voda.</u>
                            </div>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Ne koristimo zamjenske sirovine, <u className="home-font">ni kukuruz, ni kukuruznu krupicu, ni rižu, poštujemo zakon o čistoći iz 1516. godine.</u>
                            </div>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Trop, koji je nus proizvod nakon kuvanja piva, je izuzetna stočna hrana. Naše domaće životinje su srećne i zadovoljne. 
                            </div>
                        </ScrollAnimation>

                    </Col>
                </Row>
                <Row>
                    <Col className="about-us-text-container" lg="6">
                        <ScrollAnimation animateIn='backInRight' animateOnce={true}>
                            <div className="about-us-text home-font">
                                <u className="home-font">Ne filtriramo i ne pasterizujemo, želimo sačuvati sve benefite za naš organizam,</u> zato ga moramo čuvati na hladnom i tamnom mjestu.
                            </div>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInRight' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Nemamo potrebe koristiti nikakve pojačivače ukusa i aroma. <u className="home-font">Prirodno je najukusnije i najaromatičnije.</u>
                            </div>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInRight' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Izričito ne koristimo konzervanse niti aditive. <u className="home-font">Svježe paletiran hmelj je naš konzervans i antioksidans.</u>
                            </div>
                        </ScrollAnimation>
                    </Col>
                    <Col lg="6">
                        <img className="about-us-image-right" src={About2} alt="about" />
                    </Col>
                </Row>
                <Row>
                    <Col lg="6">
                        <img className="about-us-image-left" src={About3} alt="about" />
                    </Col>
                    <Col className="about-us-text-container" lg="6">
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Garancija za vrhunski kvalitet je <u className="home-font">savremena procesna oprema i tradicionalna tehnologija</u>, a tokom proizvodnje posebnu pažnju poklanjamo higijeni.
                            </div>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <div className="about-us-text home-font">
                                Svim procesima dajemo dovoljno vremena, nikud ne žurimo, <u className="home-font">strpljivo čekamo da se svi procesi završe na prirodan način.</u>
                            </div>
                        </ScrollAnimation>
                        <ScrollAnimation animateIn='backInLeft' animateOnce={true}>
                            <div className="about-us-text home-font">
                                <u className="home-font">POSJETITE NAS!</u>
                            </div>
                        </ScrollAnimation>
                    </Col>
                </Row>
            </Container>
            <img className="about-us-image-middle"  src={Production} alt="production" />
        </div>
    )
}
