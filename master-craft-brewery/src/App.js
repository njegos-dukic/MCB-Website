import './App.css';
import './common/Colors.css';
import './common/Fonts.css';
import './common/Scrollbar.css';
import 'bootstrap/dist/css/bootstrap.css';
import "animate.css/animate.compat.css";
import Layout from './global/Layout';
import { Route, Switch } from 'react-router-dom';
import { BeerMap } from './beermap/BeerMap.js';
import { Contact } from './contact/Contact.js';
import { GalleryQuotes } from './gallery-quotes/GalleryQuotes.js';
import { GalleryPage } from './gallery-quotes/GalleryPage.js';
import { HomePage } from './home/HomePage.js';
import { Shop } from './shop/Shop.js';

import { ErrorBoundary } from './error-handling/ErrorBoundary.js';
import ErrorPage from './error-handling/ErrorPage';
import FoodMenu from './food-menu/FoodMenu';
import Events from './events/Events';
import Login from './admin/Login';
import Home from './admin/Home';
import AdminFoodMenu from './admin/AdminFoodMenu';
import { AdminQuotes } from './admin/gallery-quotes/AdminQuotes.js';
import { AdminGallery } from './admin/gallery-quotes/AdminGallery.js';


function App() {
    return (
        <ErrorBoundary>
            <Layout>
                <Switch>
                    <Route exact path='/' component={HomePage} />
                    <Route exact path='/beer-map' component={BeerMap} />
                    <Route exact path='/food-menu' component={FoodMenu} />
                    <Route exact path='/contact' component={Contact} />
                    <Route exact path='/events' component={Events} />
                    <Route exact path='/gallery-quotes' component={GalleryQuotes} />
                    <Route exact path="/gallery/:id" render={(props) => (
                        <GalleryPage id={props.match.params.id} />
                    )} />
                    <Route exact path='/admin' component={Login} />
                    <Route exact path='/shop' component={Shop} />
                    <Route exact path='/admin/food-menu' component={AdminFoodMenu} />
                    <Route exact path='/admin/home' component={Home} />
                    <Route exact path='/admin/quotes' component={AdminQuotes} />
                    <Route exact path='/admin/gallery' component={AdminGallery} />
                    <Route path='*' component={ErrorPage} />
                    <Route path='/not-found'>
                        <ErrorPage httpCode={404} />
                    </Route>
                </Switch>
            </Layout>
        </ErrorBoundary >
    );
}

export default App;
