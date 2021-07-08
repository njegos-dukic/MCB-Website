import React, { useState } from 'react';
import { IoIosWarning } from 'react-icons/io';
import { Modal, ModalBody, ModalFooter, ModalHeader, Button } from 'reactstrap';
import './ConfirmDelete.css';

function ConfirmDelete(props) {
    const [isOpen, setIsOpen] = useState(true);

    const hideModal = () => {
        setIsOpen(false);
        props.onClose();
    }

    async function deleteItem() {
        await props.onDelete();
        hideModal();
    }

    return (
        <div>
            <Modal className='confirm-delete' isOpen={isOpen}>
                <ModalHeader className='confirm-delete-header'>
                    <IoIosWarning size="2.5em" />
                    <span className='monteserrat-font confirm-delete-title'>
                        Brisanje
                    </span>
                </ModalHeader>
                <ModalBody>
                    Da li se sigurni da nastavite sa brisanjem?
                    Obrisani podaci ne mogu biti vraćeni.
                </ModalBody>
                <ModalFooter className='confirm-delete-footer'>
                    <Button className='confirm-delete-button' onClick={async () => await deleteItem()}>Da</Button>
                    <Button className='cancel-delete-button' onClick={hideModal}>Otkaži</Button>
                </ModalFooter>
            </Modal>
        </div >
    );
}

export default ConfirmDelete
