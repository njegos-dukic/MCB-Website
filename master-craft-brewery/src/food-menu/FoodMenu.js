import { React, useState, useEffect } from 'react'
import { Col, Container, Row } from 'reactstrap';
import FoodCard from './FoodCard.js';
import FoodMenuHeader from './FoodMenuHeader.js';
import FoodQuote from './FoodQuote.js';
import './FoodMenu.css';
import '../common/Fonts.css';
import '../common/Colors.css';
import FoodOrder from './FoodOrder.js';
import burgerImage from '../assets/food-menu-quote-burger.jpg';
import ScrollAnimation from 'react-animate-on-scroll';
import FoodMenuNavbar from './FoodMenuNavbar.js';
import axios from 'axios';
import { MENU_PREFIX_ROUTE } from '../constants/ApiRoutes.js';

function FoodMenu() {
    const [menus, setMenus] = useState([]);
    const [currentSubmenu, setCurrentSubmenu] = useState();
    const [menuVisiblity, setMenuVisibility] = useState();

    useEffect(() => {
        const cancelTokenForMenus = axios.CancelToken;
        const sourceForMenus = cancelTokenForMenus.source();

        const cancelTokenForMenu = axios.CancelToken;
        const sourceForMenu = cancelTokenForMenu.source();
        async function fetchData() {
            await axios.get(MENU_PREFIX_ROUTE, { cancelToken: sourceForMenus.token }).then((async response => {
                setMenus(response.data);
                await axios.get(`${MENU_PREFIX_ROUTE}/${response.data[0].menuId}`, { cancelToken: sourceForMenu.token })
                    .then((response) => {
                        setCurrentSubmenu(response.data);
                        setMenuVisibility(true);
                    });
            }));
        }
        fetchData();
        return () => {
            setMenus([]);
            setCurrentSubmenu();
            setMenuVisibility();
            sourceForMenus.cancel();
            sourceForMenu.cancel();
        };
    }, []);

    async function selectSubmenu(name) {
        const selectedSubmenu = menus.find((element) => element.name === name);
        if (selectedSubmenu !== undefined) {
            await axios.get(`/menu/${selectedSubmenu.menuId}`)
                .then((response) => {
                    setCurrentSubmenu(response.data);
                });
            if (currentSubmenu !== null && currentSubmenu !== undefined) {
                setMenuVisibility(true);
            }
        }
    }

    const showFoodCards = () => {
        let foodCards = [];
        let count = currentSubmenu.menuItems.length - 1;
        for (let i = 0; i < count; i += 2) {
            foodCards.push(
                <ScrollAnimation animateIn='backInLeft' key={i}>
                    <Row>
                        <Col lg={6} sm={12}>
                            <FoodCard menuItem={currentSubmenu.menuItems[i]}></FoodCard>
                        </Col>
                        <Col lg={6} sm={12}>
                            <FoodCard menuItem={currentSubmenu.menuItems[i + 1]}></FoodCard>
                        </Col>
                    </Row>
                </ScrollAnimation>
            );
        }
        if (count % 2 === 0) {
            foodCards.push(
                <ScrollAnimation animateIn='backInLeft' key={count - 1}>
                    <Row>
                        <Col lg={6} sm={12}>
                            <FoodCard menuItem={currentSubmenu.menuItems[count - 1]}></FoodCard>
                        </Col>
                    </Row>
                </ScrollAnimation >
            )
        }
        return (<>{foodCards}</>)
    }

    return (
        <div className='monteserrat-font'>
            <FoodMenuHeader />
            <Container fluid className='food-menu-container'>
                <Row>
                    <Col xl={7} xs={12} className='menu'>
                        <FoodMenuNavbar showSubmenu={(visibility) => setMenuVisibility(visibility)} menus={menus}
                            selectionChanged={async (name) => await selectSubmenu(name)} />
                        {menuVisiblity && (<div className='menu-header'>
                            <h1>{currentSubmenu.name}</h1>
                            {currentSubmenu.description}
                        </div>)}
                        {menuVisiblity && <Container fluid className='menu-body'>
                            {showFoodCards()}
                        </Container>}
                    </Col>
                    <Col xl={4} className='food-menu-additional'>
                        <FoodOrder />
                        <FoodQuote quote='Da biste jeli dobru hranu, ne treba vam srebrna viljuška.' author='Paul Prudhomme' />
                        <FoodQuote quote='Tajna za sreću su dobra hrana & dobri prijatelji.' author='Desi Quote' imageUrl={burgerImage} />
                    </Col>
                </Row>
            </Container>
        </div >
    )
}

export default FoodMenu
