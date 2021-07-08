import Button from '@material-ui/core/Button';
import { Wrapper } from './Item.styles';
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { readImage } from '../../common/ImageReader';
import { SHOP_PREFIX_ROUTE } from '../../constants/ApiRoutes';
import './../../common/Fonts.css';

export const Item = (props) => {
    const [itemImage, setItemImage] = useState('');

    useEffect(() => {
        const cancelToken = axios.CancelToken;
        const source = cancelToken.source();

        async function fetchImage() {
            await axios.get(`${SHOP_PREFIX_ROUTE}/${props.item.shopProductServingId}/image?${(props.item.servingName === '2l') ? ('Width=900&Height=1050') : ('Width=1000&Height=1000')}`,
                { responseType: 'blob' },
                { cancelToken: source.token }
            ).then((response) => readImage(response.data, (data) => setItemImage(data))
            ).catch(error => console.log(error));
        }
        fetchImage();
        return () => {
            setItemImage('');
            source.cancel();
        };
    }, [props.item]);

    return (
        <Wrapper className='monteserrat-font'>
            <img src={`data:image/jpeg;base64,${itemImage}`} alt={props.item.productName} />

            <div className='info'>
                <h3 className='monteserrat-font'>{props.item.productName + ' ' + props.item.servingName}</h3>
                <p className='monteserrat-font'>{props.item.description}</p>
                <h3 className='monteserrat-font'>{(props.item.price).toFixed(2)} KM</h3>
            </div>

            <Button style={{paddingTop: '25px'}} className='monteserrat-font' onClick={() => { props.handleAddToCart(props.item) }}>
                <p className='monteserrat-font'> Dodaj u korpu </p>
            </Button>
        </Wrapper>
    )
}

export default Item
