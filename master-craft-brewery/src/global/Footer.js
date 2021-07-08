import React from 'react';
import './Footer.css';
import '../common/Fonts.css'
import { Link } from 'react-router-dom';
import { ReactComponent as Logo } from '../assets/MCB.svg';
import { FaFacebookF, FaInstagram } from 'react-icons/fa';

export const Footer = () => {
  let year = new Date().getFullYear();
  return (
    <div className="footer-container">
      <div className="logo-copyright-navigation">
        <div className="logo-copyright">
          <Link to='/' className='vector-logo'>
            <Logo />
          </Link>

          <small className="website-rights monteserrat-font-12px">Copyright: The Master Craft Brewery © {year}</small>
        </div>

        <div className="navigation">
          <h2 className="navigation-header">NAVIGACIJA</h2>

          <Link className="navigation-link-first" to="/" target='_blank' aria-label='Početna'>
            <p className="link-paragraph">Početna</p>
          </Link>

          <Link className="navigation-link" to="/" target='_blank' aria-label='O-Nama'>
            <p className="link-paragraph">O nama</p>
          </Link>

          <Link className="navigation-link" to="/beer-map" target='_blank' aria-label='Karta Piva'>
            <p className="link-paragraph">MCB Karta Piva</p>
          </Link>

          <Link className="navigation-link" to="/" target='_blank' aria-label='Jelovnik'>
            <p className="link-paragraph">Jelovnik</p>
          </Link>

          <Link className="navigation-link" to="/shop" target='_blank' aria-label='Shop'>
            <p className="link-paragraph">Shop</p>
          </Link>

          <Link className="navigation-link" to="/contact" target='_blank' aria-label='Kontakt'>
            <p className="link-paragraph">Kontakt</p>
          </Link>

          <Link className="navigation-link" to="/admin" aria-label='Prijava'>
            <p className="link-paragraph">Prijava</p>
          </Link>
        </div>
      </div>
      <div className="contact-location">
        <div className="contact">
          <h2 className="contact-header">KONTAKT</h2>

          <Link className="email-link" to={{ pathname: 'mailto:info@themastercraftbrewery.com' }} target='_blank' aria-label='Mail'>
            <p className="mailto">info@themastercraftbrewery.com</p>
          </Link>

          <p className="contact-info">Tel: +387 65 916 917</p>
          <p className="contact-info">Tel: +387 65 626 110</p>
        </div>

        <div className="location">
          <Link className="location-link" to={{ pathname: 'https://www.google.com/maps/place/The+Master+Craft+Brewery/@44.7634883,17.1996815,16.06z/data=!4m5!3m4!1s0x0:0xb408886d90e3ce03!8m2!3d44.763778!4d17.1978144' }} target='_blank' aria-label='Address'>
            <h2>LOKACIJA</h2>
          </Link>
          <p className="address-info-str">Bulevar Vojvode Stepe Stepanovića 44</p>
          <p className="address-info">78000 Banja Luka</p>
          <p className="address-info">Republika Srpska, Bosna i Hercegovina</p>
        </div>
      </div>

      <div className="social-icons">
        <Link to={{ pathname: 'https://www.facebook.com/themastercraftbrewery' }} className='social-icon-link' target='_blank' aria-label='Facebook'>
          <FaFacebookF className="fb-icon" />
        </Link>

        <Link className="ig-wrap social-icon-link" to={{ pathname: 'https://www.instagram.com/the_master_craft_brewery' }} target='_blank' aria-label='Instagram'>
          <FaInstagram className="ig-icon" />
        </Link>
      </div>

      <div className="map">
        <iframe className="map-frame" alt='MCB Map' title='MCB Map'
          src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d11331.340329949073!2d17.1978144!3d44.763778!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0xb408886d90e3ce03!2sThe%20Master%20Craft%20Brewery!5e0!3m2!1sen!2sba!4v1616510499348!5m2!1sen!2sba" 
          width="370px" 
          height="370px" 
          style={{ filter: "invert(90%)", border: "0" }} 
          frameBorder="0"
          allowFullScreen=""
          loading="lazy">
        </iframe>
      </div>
    </div>
  );
}