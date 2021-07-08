import { React, useState } from 'react';
import { FaHamburger } from 'react-icons/fa';
import { CgMenuCheese } from 'react-icons/cg';
import './FoodMenuNavbar.css';

function FoodMenuNavbar(props) {
    const [navMenuClick, setNavMenuClick] = useState(false);

    const handleClick = () => {
        setNavMenuClick(!navMenuClick);
        props.showSubmenu(navMenuClick);
    }

    const handleSubmenuClick = async (name) => {
        setNavMenuClick(false);
        props.showSubmenu(true);
        await props.selectionChanged(name);
    }

    const resize = () => {
        if (window.innerWidth >= 576)
            props.showSubmenu(true);
    }

    const showSubmenus = () => {
        return (props.menus.map(
            menu => (
                <li className="food-nav-item" key={menu.name}>
                    <button className="food-nav-button" onClick={async () => await handleSubmenuClick(menu.name)}>
                        <FaHamburger className='food-nav-item-icon' />
                        {menu.name}
                    </button>
                </li>)
        ));
    }
    window.addEventListener('resize', resize);

    return (
        <>
            <nav className="food-navbar">
                <div className="food-navbar-container">
                    <div className="food-menu-icon" onClick={handleClick}>
                        {navMenuClick ? <></> :
                            <CgMenuCheese />}
                    </div>
                    <ul className={navMenuClick ? 'food-nav-menu active' : 'food-nav-menu'}>
                        {showSubmenus()}
                    </ul>
                </div>
            </nav>
        </>
    )
}

export default FoodMenuNavbar
