import axios from 'axios';
import React, { useCallback, useEffect, useState } from 'react'
import { useHistory, useLocation } from 'react-router-dom'
import AdminLayout from '../AdminLayout'
import { Container, Table, 
    Modal, ModalHeader, ModalBody } from 'reactstrap';
import { LOGIN_PREFIX_ROUTE, QUOTES_PREFIX_ROUTE } from '../../constants/ApiRoutes';
import { FiEdit } from 'react-icons/fi';
import { MdDeleteForever } from 'react-icons/md';
import './QuoteGallery.css';
import { ShowResponse, ShowError } from '../Notifier';
import { ToastContainer } from 'react-toastify';
import { AdminAddEditQuote } from './AdminAddEditQuote';
import ConfirmDelete from '../ConfirmDelete';

export const AdminQuotes = () => {
    const [token, setToken] = useState('');
    const [quotes, setQuotes] = useState([]);
    const [headers, setHeaders] = useState({ 'Authorize': '' });
    const [addModal, setAddModal] = useState(false);
    const [editModal, setEditModal] = useState(false);
    const [changeHappened, setChangeHappened] = useState(false);
    const [deleteData, setDeleteData] = useState(
        {
            'quoteId': 0,
            'show': false
        }
    );
    const closeModal = () => setDeleteData({
        'quoteId': 0,
        'show': false
    });

    const location = useLocation();
    const history = useHistory();

    const redirectToLogin = useCallback(() => {
        history.push({ pathname: `/admin` });
    }, [history]);

    async function sendPostRequest(newQuote) {
        await axios.post(QUOTES_PREFIX_ROUTE, newQuote, { headers: headers })
            .then((response) => ShowResponse(response))
            .catch((error) => ShowError(error, () => { redirectToLogin() }));
        setChangeHappened(!changeHappened);
    }

    async function sendPutRequest(updatedQuote) {
        await axios.put(QUOTES_PREFIX_ROUTE, updatedQuote, { headers: headers })
            .then((response) => ShowResponse(response))
            .catch((error) => ShowError(error, () => { redirectToLogin() }));
        setChangeHappened(!changeHappened);
    }

    async function sendDeleteRequest(quoteId) {
        await axios.delete(`${QUOTES_PREFIX_ROUTE}/${quoteId}`, { headers: headers })
            .then((response) => ShowResponse(response))
            .catch((error) => ShowError(error, () => { redirectToLogin() }));
        setChangeHappened(!changeHappened);
    }

    useEffect(() => {
        const cancelTokenForQuotes = axios.CancelToken;
        const sourceForQuotes = cancelTokenForQuotes.source();

        async function fetchData(headers){
            await axios.get(LOGIN_PREFIX_ROUTE, { headers: headers })
                .then(async (response) => {
                    await axios.get(QUOTES_PREFIX_ROUTE, { cancelToken: sourceForQuotes.token })
                        .then((async response => {
                            setQuotes(response.data);
                        }));
                })
                .catch((error) => redirectToLogin());
        }

        if (location.state !== undefined && location.state.token !== undefined) {
            setToken(location.state.token);
            const headers = {
                'Authorize': location.state.token
            }
            setHeaders(headers);
            fetchData(headers);
        } else 
            redirectToLogin();

        return () => {
            setToken('');
            setQuotes([]);
            setHeaders({ 'Authorize': '' });
            sourceForQuotes.cancel();
        }
    }, [location, changeHappened, redirectToLogin]);

    const [modal, setModal] = useState(false);
  
    const toggle = () => setModal(!modal);
    
    const showAddModal = () => {
        setAddModal(true);
        setEditModal(false);
        toggle();
    }

    const [quoteForEdit, setQuoteForEdit] = useState({});
    const showEditModal = (quote) => {
        setEditModal(true);
        setAddModal(false);
        setQuoteForEdit(quote);
        toggle();
    }

    const EditQuote = () => {
    
        return (
            <AdminAddEditQuote quote={quoteForEdit} 
                func={async (editedQuote) => sendPutRequest(editedQuote)} 
                toggle={() => toggle()} />
        )
    }
    
    const AddQuote = () => {
    
        return (
            <AdminAddEditQuote 
                func={async (newQuote) => sendPostRequest(newQuote)} 
                toggle={() => toggle()} />
        )
    }

    return (
        <div>
            {deleteData.show && <ConfirmDelete onClose={closeModal} onDelete={async () => await sendDeleteRequest(deleteData.quoteId)} />}
            <AdminLayout token={token} add={() => showAddModal()}>
                <ToastContainer
                    position="bottom-right"
                    autoClose={false}
                    newestOnTop={false}
                    closeOnClick
                    rtl={false}
                    pauseOnFocusLoss
                    draggable
                />
                <Container fluid className='admin-index-container'>
                    <Table dark className="admin-quote-table">
                        <thead>
                            <tr>
                                <th>Citat</th>
                                <th>Autor</th>
                                <th colSpan={2}>Opcije</th>
                            </tr>
                        </thead>
                        <tbody>
                            {quotes.map((quote, index) => {
                                return(
                                    <tr key={index}>
                                        <td>{quote.quoteText}</td>
                                        <td>{quote.author}</td>
                                        <td>
                                            <FiEdit className='admin-index-options-button'
                                                onClick={() => showEditModal(quote)} />
                                        </td>
                                        <td>
                                            <MdDeleteForever className='admin-index-options-button'
                                                onClick={() => setDeleteData({ 'quoteId': quote.quoteId, 'show': true })} />
                                        </td>
                                    </tr>
                                )
                            })}
                        </tbody>
                    </Table>
                </Container>
                <Modal isOpen={modal} size="md" toggle={toggle} className="admin-quotes-gallery-modal" unmountOnClose={true}>
                    <ModalHeader toggle={toggle} >
                    {addModal && <div>Dodaj citat</div>}
                    {editModal && <div>Izmijeni citat</div>}
                    </ModalHeader>
                    <ModalBody>
                        {addModal && <AddQuote />}
                        {editModal && <EditQuote />}
                    </ModalBody>
                </Modal>
            </AdminLayout>        
        </div>
    )
}

