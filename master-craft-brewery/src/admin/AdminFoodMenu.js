import React, { useCallback, useEffect, useState } from 'react';
import { Container, Table } from 'reactstrap';
import { useHistory, useLocation } from 'react-router-dom';
import { LOGIN_PREFIX_ROUTE, MENU_PREFIX_ROUTE, PRODUCT_SERVINGS_PREFIX_ROUTE } from '../constants/ApiRoutes';
import { FiEdit } from 'react-icons/fi';
import { MdDeleteForever } from 'react-icons/md';
import { BsInfoSquare } from 'react-icons/bs';
import { ToastContainer } from 'react-toastify';
import { ShowResponse, ShowError } from './Notifier';
import AdminLayout from './AdminLayout';
import AdminActionFoodMenu from './AdminActionFoodMenu';
import ConfirmDelete from './ConfirmDelete';
import axios from 'axios';
import 'react-toastify/dist/ReactToastify.css';
import 'animate.css';
import './AdminIndex.css';

function AdminFoodMenu() {
    const [modalData, setModalData] = useState(
        {
            'menuId': 0,
            'show': false
        }
    );
    const closeModal = () => setModalData({
        'menuId': 0,
        'show': false
    });

    const [menus, setMenus] = useState([]);
    const [productServings, setProductServings] = useState([]);
    const [token, setToken] = useState('');
    const [selectedMenu, setSelectedMenu] = useState();
    const [showDetails, setShowDetails] = useState(false);
    const [showAdd, setShowAdd] = useState(false);
    const [showUpdate, setShowUpdate] = useState(false);
    const [showTable, setShowTable] = useState(true);
    const [headers, setHeaders] = useState({ 'Authorize': '' });

    const hide = () => {
        setShowAdd(false);
        setShowDetails(false);
        setShowUpdate(false);
        setShowTable(true);
    }
    const show = (showElementFunction) => {
        showElementFunction(true);
        setShowTable(false);
    }

    const location = useLocation();
    const history = useHistory();

    const redirectToLogin = useCallback(() => {
        history.push({ pathname: `/admin` });
    }, [history]);

    async function sendPostRequest(newMenu) {
        await axios.post(MENU_PREFIX_ROUTE, newMenu, { headers: headers })
            .then((response) => ShowResponse(response))
            .catch((error) => ShowError(error, () => { redirectToLogin() }));
    }

    async function sendPutRequest(updatedMenu) {
        await axios.put(MENU_PREFIX_ROUTE, updatedMenu, { headers: headers })
            .then((response) => ShowResponse(response))
            .catch((error) => ShowError(error, () => { redirectToLogin() }));
    }

    async function sendDeleteRequest(menuId) {
        await axios.delete(`${MENU_PREFIX_ROUTE}/${menuId}`, { headers: headers })
            .then((response) => ShowResponse(response))
            .catch((error) => ShowError(error, () => { redirectToLogin() }));
    }

    async function getFoodMenuDetails(menuId) {
        // Check if user is logged in, and if it is send get request
        return await axios.get(LOGIN_PREFIX_ROUTE, { headers: headers })
            .then(async (response) => await axios.get(`${MENU_PREFIX_ROUTE}/${menuId}`)
                .then((response) => {
                    setSelectedMenu(response.data);
                }))
            .catch((error) => redirectToLogin());
    }

    const renderTableData = () => {
        return (
            menus.map(x => {
                return (
                    <tr className='admin-index-table-rows' key={x.menuId} data-item={x}
                        onClick={async () => await getFoodMenuDetails(x.menuId)} >
                        <td>{x.name}</td>
                        <td>{x.description}</td>
                        <td>
                            <BsInfoSquare className='admin-index-options-button'
                                onClick={() => show((el) => setShowDetails(el))} />
                        </td>
                        <td>
                            <FiEdit className='admin-index-options-button' onClick={() => show((el) => setShowUpdate(el))} />
                        </td>
                        <td>
                            <MdDeleteForever className='admin-index-options-button'
                                onClick={(() => setModalData({ 'menuId': x.menuId, 'show': true }))} />
                        </td>
                    </tr >);
            })
        )
    };

    useEffect(() => {
        const cancelTokenForMenus = axios.CancelToken;
        const sourceForMenus = cancelTokenForMenus.source();

        const cancelTokenForProductServings = axios.CancelToken;
        const sourceForProductServings = cancelTokenForProductServings.source();

        async function fetchData(headers) {
            // Check if user is logged in, and if it is send get request
            await axios.get(LOGIN_PREFIX_ROUTE, { headers: headers })
                .then(async (response) => {
                    await axios.get(MENU_PREFIX_ROUTE, { cancelToken: sourceForMenus.token })
                        .then((async response => {
                            setMenus(response.data);
                        }));
                    await axios.get(PRODUCT_SERVINGS_PREFIX_ROUTE, { cancelToken: sourceForProductServings.token })
                        .then((response) => setProductServings(response.data));
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
        } else redirectToLogin();
        return () => {
            setToken('');
            setMenus([]);
            setProductServings([]);
            setHeaders({ 'Authorize': '' });
            sourceForProductServings.cancel();
            sourceForMenus.cancel();
        }
    }, [location, showTable, modalData, redirectToLogin])

    return (
        <>
            {modalData.show && <ConfirmDelete onClose={closeModal} onDelete={async () => await sendDeleteRequest(modalData.menuId)} />}
            <AdminLayout token={token} add={() => show((el) => setShowAdd(el))}>
                <ToastContainer
                    position="bottom-right"
                    autoClose={false}
                    newestOnTop={false}
                    closeOnClick
                    rtl={false}
                    pauseOnFocusLoss
                    draggable
                />
                {showTable && <Container fluid className='admin-index-container'>
                    <Table bordered className='admin-index-table'>
                        <thead className='admin-index-table-header'>
                            <tr>
                                <th>Naziv</th>
                                <th>Opis</th>
                                <th colSpan={3}>Opcije</th>
                            </tr>
                        </thead>
                        <tbody>
                            {renderTableData()}
                        </tbody>
                    </Table>
                </Container>}
                {showAdd && <AdminActionFoodMenu productServings={productServings}
                    modifyEnabled={true}
                    hide={() => hide()}
                    modify={async (newMenu) => await sendPostRequest(newMenu)} />}
                {showDetails && <AdminActionFoodMenu productServings={productServings}
                    modifyEnabled={false}
                    existingData={selectedMenu}
                    hide={() => hide()} />}
                {showUpdate && <AdminActionFoodMenu productServings={productServings}
                    modifyEnabled={true}
                    existingData={selectedMenu}
                    hide={() => hide()}
                    modify={async (updatedMenu) => await sendPutRequest(updatedMenu)} />}
            </AdminLayout >
        </>
    )
}

export default AdminFoodMenu
