import CartItem from '../cartItem/CartItem';
import { Wrapper } from './Cart.styles';
import { Dialog, DialogTitle, DialogContent, Button, TextField, Grid, CssBaseline, Snackbar } from '@material-ui/core';
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles';
import Alert from '@material-ui/lab/Alert'
import React, { useState } from 'react'
import { Typography } from '@material-ui/core';
import Reaptcha from 'reaptcha';
import { IsValidEmail, IsValidString } from "../../error-handling/InputValidator";
import axios from 'axios';
import * as Messages from '../../constants/Messages.js';
import * as Routes from '../../constants/ApiRoutes';
import { ToastContainer } from 'react-toastify';
import { ShowResponse } from "../../admin/Notifier";
import './../../common/Fonts.css'

function Cart(props) {
    const [dialogOpen, setDialogOpen] = useState(false);
    const [alertOpen, setAlertOpen] = useState(false);
    const [captchaAlertOpen, setCaptchaAlertOpen] = useState(false);

    const [captchaError, setCaptchaError] = useState('');

    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');

    const [street, setStreet] = useState('');
    const [city, setCity] = useState('');
    const [ZIP, setZIP] = useState('');
    const [country, setCountry] = useState('');

    const [email, setEmail] = useState('');
    const [phone, setPhone] = useState('');

    const onVerify = () => {
        props.setVerified(true);
    };
    const createCaptcha = () => (<Reaptcha style={{ width: '50%' }} className="captcha" sitekey={'6LcyxXAbAAAAAFa2ho1OcCs_J48t_cj8DGtm4nAl'} onVerify={onVerify} />)

    const calculateTotalPrice = (items) => {
        return items.reduce((ack, item) => ack + item.amount * item.price, 0);
    }

    const calculateTotalDelivery = (items) => {
        return (items.length === 0 ? 0.00 : (calculateTotalPrice(items) >= 100 ? 0.00 : (calculateTotalPrice(items) >= 70 ? 1.00 : (calculateTotalPrice(items) >= 40 ? 2.00 : 3.00))));
    }

    const calculateTotal = (items) => {
        return calculateTotalPrice(items) + calculateTotalDelivery(items);
    }


    const handleDialogOpen = () => {
        setDialogOpen(true);
    };

    const handleDialogClose = () => {
        setDialogOpen(false);
    };

    const darkTheme = createMuiTheme({
        palette: {
            type: 'dark',
        },
    });

    const handleOrder = async () => {
        if (props.cartItems.length === 0) {
            setCaptchaError(Messages.CART_EMPTY);
            setAlertOpen(true);
            return;
        }

        let valid = true;

        if (!IsValidString(name))
            valid = false

        if (!IsValidString(surname))
            valid = false

        if (!IsValidEmail(email))
            valid = false

        if (!IsValidString(phone))
            valid = false

        if (!IsValidString(street))
            valid = false

        if (!IsValidString(city))
            valid = false

        if (!IsValidString(ZIP))
            valid = false

        if (!IsValidString(country))
            valid = false

        if (!valid) {
            setCaptchaError(Messages.NO_INPUT);
            setAlertOpen(true);
            return;
        }

        let items = []

        props.cartItems.foreach((item) => {
            items.push({ shopProductServingId: item.shopProductServingId, price: item.price, orderedAmount: item.amount })
        })

        let order = {
            fullName: name + ' ' + surname,
            phoneNumber: phone,
            email: email,
            countryName: country,
            city: city,
            street: street,
            postalCode: ZIP,
            deliveryCost: calculateTotalDelivery(props.cartItems),
            productOrders: items
        }

        await axios.post(Routes.ORDER_ROUTE, order)
            .then((response) => {
                ShowResponse(response);

                handleDialogClose()
                setName('')
                setSurname('')
                setEmail('')
                setPhone('')
                setCountry('')
                setCity('')
                setZIP('')
                setStreet('')
            })
            .catch((err) => console.log(err));
    }

    return (
        <Wrapper className='monteserrat-font'>
            <h4 className='monteserrat-font'>Vaša korpa</h4>
            <hr />
            {props.cartItems.length === 0 ? <p className='monteserrat-font'>Nema proizvoda u korpi.</p> : null}
            {props.cartItems.map((item, index) => (
                <CartItem key={index} item={item} addToCart={props.addToCart} removeFromCart={props.removeFromCart} />
            ))}
            <br />
            <h6 className='monteserrat-font'>Cijena: {calculateTotalPrice(props.cartItems).toFixed(2)} KM</h6>
            <h6 className='monteserrat-font'>Dostava: {calculateTotalDelivery(props.cartItems).toFixed(2)} KM</h6>
            <h6 className='monteserrat-font'>Ukupno: {calculateTotal(props.cartItems).toFixed(2)} KM</h6>
            <br />
            {!props.verified === true ? createCaptcha() : null}
            <br />

            <Button className='monteserrat-font' size="large" style={{ width: '84%', background: '#4f4f4f', color: 'white' }} variant="outlined" onClick={() => {
                setCaptchaError('');

                if (!props.verified) {
                    setCaptchaError(Messages.INVALID_CAPTCHA);
                    setCaptchaAlertOpen(true);
                    return;
                }

                handleDialogOpen()
            }}>
                NARUČI
            </Button>

            <ThemeProvider theme={darkTheme}>
                <CssBaseline />
                <Dialog className='monteserrat-font' open={dialogOpen} onClose={handleDialogClose} aria-labelledby="form-dialog-title" fullWidth maxWidth="sm" >
                    <DialogTitle id="form-dialog-title">
                        <p className='monteserrat-font'> Završi narudžbu </p>
                    </DialogTitle>
                    <DialogContent>
                        <Grid container direction="column" spacing={2} >
                            <Grid item>
                                <TextField variant='filled'
                                    value={name}
                                    label='Ime'
                                    hint='Ime'
                                    onChange={e => setName(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={surname}
                                    label='Prezime'
                                    hint='Prezime'
                                    onChange={e => setSurname(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={street}
                                    label='Ulica i broj'
                                    hint='Ulica i broj'
                                    onChange={e => setStreet(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={city}
                                    label='Grad'
                                    hint='Grad'
                                    onChange={e => setCity(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={ZIP}
                                    label='Poštanski broj'
                                    hint='Poštanski broj'
                                    onChange={e => setZIP(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={country}
                                    label='Država'
                                    hint='Država'
                                    onChange={e => setCountry(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={email}
                                    label='Email'
                                    hint='Email'
                                    onChange={e => setEmail(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <TextField variant='filled'
                                    value={phone}
                                    label='Telefon'
                                    hint='Telefon'
                                    onChange={e => setPhone(e.target.value)}
                                    fullWidth maxWidth="sm"
                                />
                            </Grid>

                            <Grid item>
                                <Button size="large" style={{ background: '#1f1f1f', color: 'white' }} variant="outlined" onClick={() => handleOrder()} fullWidth maxWidth="sm" >
                                    <Typography className='monteserrat-font'>
                                        POTVRDI
                                    </Typography>
                                </Button>
                            </Grid>

                            <Grid item>
                                <Snackbar open={alertOpen} autoHideDuration={2000} onClose={() => setAlertOpen(false)}>
                                    <Alert className='monteserrat-font' onClose={() => setAlertOpen(false)} severity="error">
                                        {captchaError}
                                    </Alert>
                                </Snackbar>
                            </Grid>
                        </Grid>
                    </DialogContent>
                </Dialog>
            </ThemeProvider>

            <Snackbar open={captchaAlertOpen} autoHideDuration={2000} onClose={() => setCaptchaAlertOpen(false)}>
                <Alert className='monteserrat-font' onClose={() => setCaptchaAlertOpen(false)} severity="error">
                    {captchaError}
                </Alert>
            </Snackbar>

            <ToastContainer className='monteserrat-font'
                position="bottom-right"
                autoClose={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
            />
        </Wrapper>
    )
}

export default Cart