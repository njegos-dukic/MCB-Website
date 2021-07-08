import { useState, useEffect } from 'react';
import Item from './item/Item'
import Cart from './cart/Cart'
import Drawer from '@material-ui/core/Drawer';
import Grid from '@material-ui/core/Grid';
import Snackbar from '@material-ui/core/Snackbar';
import Alert from '@material-ui/lab/Alert'
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';
import Badge from '@material-ui/core/Badge';
import { Wrapper } from './Shop.styles';
import { StyledButton } from './Shop.styles';
import { SHOP_PREFIX_ROUTE } from '../constants/ApiRoutes';
import axios from 'axios';
import './../common/Fonts.css'

export const Shop = () => {
  const [cartOpen, setCartOpen] = useState(false);

  const [cartItems, setCartItems] = useState([]);
  const [data, setData] = useState([]);
  const [snackbarOpen, setSnackbarOpen] = useState(false);

  const [verified, setVerified] = useState(false);

  useEffect(() => {
    const cancelToken = axios.CancelToken
    const source = cancelToken.source()

    async function fetchProducts() {
      await axios.get(SHOP_PREFIX_ROUTE, { cancelToken: source.token })
        .then((response) => {
          setData(response.data);
        })
        .catch(error => console.log(error));
    }

    fetchProducts();

    return () => {
      setData([])
      source.cancel()
    }
  }, []);

  const getTotalItems = (items) => items.reduce((ack, items) => ack + items.amount, 0);

  const handleAddToCart = (clickedItem) => {
    setSnackbarOpen(true);

    setCartItems(prev => {
      const isItemInCart = prev.find(item => item.shopProductServingId === clickedItem.shopProductServingId);
      if (isItemInCart) {
        return prev.map(item => (
          item.shopProductServingId === clickedItem.shopProductServingId
            ? { ...item, amount: item.amount + clickedItem.incrementAmount }
            : item
        ))
      }

      return [...prev, { ...clickedItem, amount: clickedItem.incrementAmount }];
    })
  };

  const handleRemoveFromCart = (id) => {
    setCartItems(prev => (
      prev.reduce((ack, item) => {
        if (item.shopProductServingId === id) {
          if (item.amount === item.incrementAmount) return ack;

          return [...ack, { ...item, amount: item.amount - item.incrementAmount }]
        } else {
          return [...ack, item]
        }
      }, [])
    ))
  };

  return (
    <div className='monteserrat-font' style={{ background: '#2b2b2b', overflow: 'hidden', paddingLeft: '50px', marginBottom: '0px', paddingBottom: '25px' }}>
      <Wrapper>
        <Drawer anchor='right' color='white' open={cartOpen} onClose={() => setCartOpen(false)}>
          <Cart cartItems={cartItems} addToCart={handleAddToCart} verified={verified} setVerified={(ver) => setVerified(ver)} removeFromCart={handleRemoveFromCart}></Cart>
        </Drawer>

        <StyledButton onClick={() => setCartOpen(true)} >
          <Badge badgeContent={getTotalItems(cartItems)} color='error' style={{ position: 'fixed' }} >
            <AddShoppingCartIcon fontSize="large" style={{ color: '#fff', position: 'fixed' }} />
          </Badge>
        </StyledButton>

        <Grid container spacing={4}>
          {
            data.map((item, index) => <Grid item key={index} xs={12} sm={4}>
              <Item item={item} handleAddToCart={handleAddToCart} />
            </Grid>)
          }
        </Grid>

        <Snackbar open={snackbarOpen} autoHideDuration={1000} onClose={() => setSnackbarOpen(false)}>
          <Alert className='monteserrat-font' onClose={() => setSnackbarOpen(false)} severity="success">
            Proizvod dodat u korpu.
          </Alert>
        </Snackbar>
      </Wrapper>
    </div>
  );
}
