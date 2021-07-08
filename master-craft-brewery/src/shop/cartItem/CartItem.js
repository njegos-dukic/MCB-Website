import Button from '@material-ui/core/Button';
import { Wrapper } from './CartItem.styles';
import React, { useState, useEffect } from 'react';
import { Typography } from '@material-ui/core';
import axios from 'axios';
import { readImage } from '../../common/ImageReader';
import { SHOP_PREFIX_ROUTE } from '../../constants/ApiRoutes';
import './../../common/Fonts.css'

function CartItem(props) {
    const [image, setImage] = useState('');

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchImage() {
            await axios.get(`${SHOP_PREFIX_ROUTE}/${props.item.shopProductServingId}/image?${(props.item.servingName === '2l') ? ('Width=90&Height=105') : ('Width=100&Height=100')}`,
                { responseType: 'blob' },
                { cancelToken: source.token }
            ).then((response) => readImage(response.data, (data) => setImage(data)));
        }
        fetchImage();
        return () => {
            setImage('');
            source.cancel();
        };
    }, [props.item]);

    return (
        <Wrapper>
            <div>
                <h5 className='monteserrat-font'>{props.item.productName + ' ' + props.item.servingName}</h5>
                <div className="information">
                    <p className='monteserrat-font'>{(props.item.price).toFixed(2)} KM</p>
                    <p className='monteserrat-font'>Ukupno: {(props.item.amount * props.item.price).toFixed(2)} KM</p>
                </div>
                <div className="buttons">
                    <Button size="small" style={{ background: '#4f4f4f', color: 'white' }} variant="outlined" onClick={() => props.removeFromCart(props.item.shopProductServingId)}>
                        <Typography className='monteserrat-font'>-</Typography>
                    </Button>
                    <h6 className='monteserrat-font' style={{ alignSelf: 'center' }}>{props.item.amount}</h6>
                    <Button size="small" style={{ background: '#4f4f4f', color: 'white' }} variant="outlined" onClick={() => props.addToCart(props.item)}>
                        <Typography className='monteserrat-font'>+</Typography>
                    </Button>
                </div>
            </div>
            <img src={`data:image/jpeg;base64,${image}`} alt={props.item.productName} />
            <br />
        </Wrapper>
    )
}

export default CartItem
