import React from 'react';
import { Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import { OperationStatus } from '../constants/OperationStatus.js';
import { HandleStatusInDetail, HandleHttpCode } from './ErrorHandler';
import sadBeer from '../assets/sad-beer.png';
import '../common/Colors.css';
import './ErrorPage.css';
import { GENERIC_ERROR } from '../constants/Messages.js';

function ErrorPage(props) {

    const printErrorDetails = () => {
        let httpCode = 404;
        if (props.serverResponse !== undefined) {
            let errorStatus = OperationStatus.UNKNOWN_ERROR;
            const serverResponse = props.serverResponse;
            if (serverResponse.includes('"status":'))
                errorStatus = JSON.parse(serverResponse).status;
            console.log(HandleStatusInDetail(errorStatus));
            return GENERIC_ERROR;
        }
        if (props.httpCode !== undefined)
            httpCode = props.httpCode;
        return HandleHttpCode(httpCode);
    }

    return (
        <div className='segoi-ui error-page'>
            <div className='error-page-text'>
                <div className='error-page-notification'>
                    Ups...
                </div>
                <div className='error-page-message'>
                    {printErrorDetails()}
                </div>
                <div className='error-page-button-container'>
                    <Button className='error-page-button' tag={Link} to="/">POČETNA</Button>
                </div>
            </div>
            <div className='error-page-image-container'>
                <img className='error-page-image' src={sadBeer} alt='errorImage' ></img>
            </div>
        </div>
    )
}

export default ErrorPage

