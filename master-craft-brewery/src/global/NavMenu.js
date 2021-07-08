import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { BsList, BsX } from "react-icons/bs";
import logo1 from "../assets/Logo.png";
import { ReactComponent as LogoSVG } from "../assets/MCB.svg";
import './../common/Fonts.css'

function NavMenu() {
    const [state, setState] = useState({
        collapsed: true
    });

    const toggleNavbar = () => setState({ ...state, collapsed: !state.collapsed });
    const closeMenuForMobile = () => setState({ ...state, collapsed: true });

    return (
        <header>
            <nav className="navbar-nb dark-gray">
                <div className="navbar-container monteserrat-font-09em">
                    <Link to="/" className="navbar-logo">
                        <img className="navbar-logo-png" src={logo1} alt='logo' />
                        <LogoSVG viewBox="0 -5 240 80" />
                    </Link>
                    <div className="menu-icon" onClick={toggleNavbar}>
                        {state.collapsed ? <BsList className="fa-times dark-gray" /> : <BsX className="fa-bars dark-gray" />}
                    </div>
                    <ul id="items" className={state.collapsed ? 'nav-menu' : 'nav-menu active'}>
                        <li className="nav-item">
                            <Link to="/" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                Početna
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/events" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                Događaji
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/food-menu" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                Jelovnik
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/beer-map" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                MCB Karta Piva
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/gallery-quotes" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                Galerija
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/shop" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                SHOP
                            </Link>
                        </li>
                        <li className="nav-item contact">
                            <Link to="/contact" className="nav-links navbar-font" onClick={closeMenuForMobile}>
                                Kontakt
                            </Link>
                        </li>
                    </ul>
                </div>
            </nav>
        </header>
    );
}

export default NavMenu;