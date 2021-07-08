import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from "reactstrap";
import { FaExclamationCircle } from 'react-icons/fa';
import { OperationStatus } from '../constants/OperationStatus.js';
import { HandleHttpCode, HandleStatusInDetail } from './ErrorHandler';
import '../common/Fonts.css';
import './ErrorModal.css';
import { GENERIC_ERROR } from '../constants/Messages.js';

/**
 * 
 * @param {object} props Component props
 * @param {function} props.onClose Function that is called after modal is closed
 * @param {json} props.serverResponse Server response that needs to be parsed
 * @returns 
 */
export const ErrorModal = (props) => {
    const [isOpen, setIsOpen] = useState(true);

    const hideModal = () => {
        setIsOpen(false);
        props.onClose();
    }

    const printErrorDetails = () => {
        let message = GENERIC_ERROR;
        if (props.serverResponse != undefined) {
            let errorStatus = OperationStatus.UNKNOWN_ERROR;
            const serverResponse = props.serverResponse;
            if (serverResponse.hasOwnProperty('status'))
                errorStatus = serverResponse.status;
            console.log(HandleStatusInDetail(errorStatus));
        } else
            console.log(HandleHttpCode(props.httpCode));

        return <div className='error-modal-message'>{message}</div>;
    }

    return (
        <div>
            <Modal className='modal-content monteserrat-font-1-2em' isOpen={isOpen}>
                <ModalHeader className='error-modal-header'>
                    <FaExclamationCircle size="2.5em" />
                    <span className='monteserrat-font error-modal-title'>
                        Greška
                    </span>
                </ModalHeader>
                <ModalBody>
                    {printErrorDetails()}
                </ModalBody>
                <ModalFooter className='error-modal-footer'>
                    <Button className='error-modal-button' onClick={hideModal}>Zatvori</Button>
                </ModalFooter>
            </Modal>
        </div >
    );
}