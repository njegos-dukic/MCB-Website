import React from 'react';
import { FaTruck } from 'react-icons/fa';
import './FoodOrder.css';

function FoodOrder() {
    return (
        <div className='monteserrat-font food-menu-order'>
            <h3>NOVO!</h3>
            <p><br />Moguća dostava hladnog piva i jela sa MCB jelovnika preko aplikacije Ceger unutar 30 minuta</p>
            <a href='https://cegerdostava.com/' target='_blank' rel='noreferrer'>
                <FaTruck className='order-icon' />
                Naruči
            </a>
        </div >
    )
}

export default FoodOrder
