import { React, useState, useEffect } from 'react';
import { Badge, Col, Container, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Row } from 'reactstrap';
import { IoMdClose } from 'react-icons/io';
import { AiOutlineCloseCircle } from 'react-icons/ai';
import { IsValidString } from '../error-handling/InputValidator';
import './AdminIndex.css';
import './AdminActions.css';
import './AdminActionFoodMenu.css';
import { INVALID_DESCRIPTION, INVALID_NAME } from '../constants/Messages';

function AdminActionFoodMenu(props) {
    const settings = {
        'modifyEnabled': props.modifyEnabled,
        'existingData': props.existingData,
        'isValidData': false
    };

    /// Data to send on request
    const [menuItems, setMenuItems] = useState([]);
    const [menuId, setMenuId] = useState(0);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const [nameError, setNameError] = useState('');
    const [descriptionError, setDescriptionError] = useState('');

    const [dropdownOpen, setDropdownOpen] = useState(false);
    const toggle = () => setDropdownOpen(prevState => !prevState);
    const hideActions = () => props.hide();

    const isValidModel = () => {
        let errorIndicator = true;
        if (!IsValidString(name)) {
            setNameError(INVALID_NAME);
            errorIndicator = false;
        }
        else
            setNameError('');

        if (!IsValidString(description)) {
            setDescriptionError(INVALID_DESCRIPTION);
            errorIndicator = false;
        }
        else
            setDescriptionError('');
        return errorIndicator;
    }

    const addProduct = (productServing, e) => {
        if (settings.existingData !== undefined || settings.existingData !== null)
            menuItems.push({
                'menuItemId': 0,
                'productServingId': productServing.productServingId,
                'menuId': 0,
                'name': e.target.innerText,
                'servingName': e.target.nextSibling.innerText
            });
    }

    const removeProduct = (x) => {
        setMenuItems(menuItems.filter(item => item.name !== x.name && item.servingName !== x.servingName));
    }

    async function modify() {
        if (isValidModel()) {
            let menuData = {
                'menuId': menuId,
                'name': name,
                'description': description,
                'menuItems': menuItems.map(detailed => {
                    return {
                        'menuItemId': detailed.menuItemId,
                        'productServingId': detailed.productServingId,
                        'menuId': detailed.menuId
                    }
                })
            }
            await props.modify(menuData);
            hideActions();
        }
    }

    useEffect(() => {
        if (settings.existingData !== undefined && settings.existingData != null) {
            setMenuItems(settings.existingData
                .menuItems
                .map(x => x.servings.map(serving => {
                    return {
                        'menuItemId': serving.menuItemId,
                        'productServingId': serving.productServingId,
                        'menuId': settings.existingData.menuId,
                        'servingName': serving.name,
                        'name': x.name
                    }
                })
                ).flat());
            setMenuId(settings.existingData.menuId);
            setName(settings.existingData.name);
            setDescription(settings.existingData.description);
        }
        return () => {
            setMenuItems([]);
            setMenuId(0);
            setName('');
            setDescription('');
        }
    }, [settings.existingData])

    return (
        <Container className='monteserrat-font admin-action-container'>
            <Row className='row-margin'>
                <Col lg={11}>
                </Col>
                <Col>
                    <AiOutlineCloseCircle className='admin-close-actions-icon' onClick={() => hideActions()} />
                </Col>
            </Row>
            <Row>
                <Col>
                    <Row className='margin-0'>
                        <Col>
                            <div className='action-label'>
                                Naziv
                            </div>
                            <input value={name} className='action-input' type=' text'
                                onInput={e => setName(e.target.value)} />
                            <label className="action-input-error">{nameError}</label>
                        </Col>
                    </Row>
                    <Row className='margin-0'>
                        <Col>
                            <div className='action-label'>
                                Opis
                            </div>
                            <textarea value={description} className="action-textarea" type='text'
                                onInput={e => setDescription(e.target.value)} />
                            <label className="action-input-error">{descriptionError}</label>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            {
                                settings.modifyEnabled && <div className='action-buttons'>
                                    <button className='action-button submit-button' onClick={async () => await modify()}>Sačuvaj</button>
                                    <button className='action-button cancel-button' onClick={() => hideActions()} > Otkaži</button>
                                </div>
                            }
                        </Col>
                    </Row>
                </Col>
                <Col>
                    <div className='action-label'>
                        Proizvodi
                    </div>
                    <div className='items-container'>
                        <ul className='items-list'>
                            {menuItems
                                .map(x => {
                                    return (
                                        <li key={1 + Math.random() * (1000 - 1)} >
                                            <p className='margin-0'>{x.name}</p>
                                            <Badge color="dark">{x.servingName}</Badge>
                                            {settings.modifyEnabled &&
                                                <IoMdClose className='items-list-delete' onClick={() => removeProduct(x)} />}
                                        </li>);
                                })
                            }
                        </ul>
                        {settings.modifyEnabled &&
                            <Dropdown isOpen={dropdownOpen} toggle={toggle} className='items-dropdown'>
                                <DropdownToggle>
                                    Dodaj novi proizvod
                                </DropdownToggle>
                                <DropdownMenu className='items-dropdown-menu'>
                                    <DropdownItem header>Proizvodi</DropdownItem>
                                    {
                                        // Offer only does product servings that are not already added
                                        props.productServings.filter(x => {
                                            if (settings.existingData !== undefined && settings.existingData !== null) {
                                                return !settings.existingData.menuItems
                                                    .some(menuItem => menuItem.name === x.productName && menuItem.servings
                                                        .some(serving => serving.name === x.servingName))
                                            }
                                            else return true;
                                        })
                                            // Map to appropriate dropdown item
                                            .map(x => {
                                                return (
                                                    < DropdownItem key={1 + Math.random() * (1000 - 1)} id={x.productServingId}
                                                        onClick={(e) => addProduct(x, e)}>
                                                        <p className='margin-0'>{x.productName}</p>
                                                        <Badge color="dark">{x.servingName}</Badge></DropdownItem>
                                                )
                                            })
                                    }
                                </DropdownMenu>
                            </Dropdown>}
                    </div>
                </Col>
            </Row>
        </Container >
    )
}

export default AdminActionFoodMenu
