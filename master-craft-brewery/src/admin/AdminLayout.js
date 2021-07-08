import { React, useState, useEffect } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { IoMdReturnLeft, IoMdAdd } from 'react-icons/io';
import { BiLogOut } from 'react-icons/bi';
import { useHistory, useLocation } from 'react-router-dom';
import './AdminLayout.css';

function AdminLayout(props) {
    const [token, setToken] = useState('');
    const history = useHistory();
    const location = useLocation();

    const redirectToLogin = () => {
        history.push({ pathname: `/admin` });
    }
    const redirectToHome = () => {
        history.push({ pathname: `/admin/home` }, { token: token });
    }

    useEffect(() => {
        if (location.state !== undefined)
            setToken(location.state.token);
        else
            setToken(props.token);
        return () => {
            setToken('');
        }
    }, [location, props.token])

    return (
        <Container fluid className='admin-layout-container'>
            <Row className='quote-font admin-layout-navigation'>
                <Col >
                    {
                        props.add &&
                        <button className='admin-layout-button' onClick={() => props.add()}><IoMdAdd /></button>
                    }
                    <button className='admin-layout-button' onClick={redirectToHome}><IoMdReturnLeft /></button>
                    <button className='admin-layout-button' onClick={redirectToLogin}><BiLogOut /></button>
                </Col>
            </Row>
            <Row className='admin-layout'>
                {props.children}
            </Row>

        </Container >
    )
}

export default AdminLayout
