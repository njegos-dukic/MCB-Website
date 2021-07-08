import { React, useState } from 'react';
import { Col, Container, Row } from 'reactstrap';
import { FaKey, FaUser } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { LOGIN_PREFIX_ROUTE } from '../constants/ApiRoutes';
import axios from 'axios';
import logo from '../assets/Logo.png';
import './Login.css';
import { INVALID_EMAIL, INVALID_PASSWORD, LOGIN_FAILED } from '../constants/Messages';
import { IsValidString } from '../error-handling/InputValidator';


function Login() {
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');

    const [emailError, setEmailError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [loginFailed, setLoginFailed] = useState('');

    const history = useHistory();

    const redirectToHome = (tokenResponse) => {
        history.push({ pathname: `/admin/home`, state: { token: tokenResponse.token } });
    }

    const isValidModel = () => {
        let errorIndicator = true;
        if (!IsValidString(email)) {
            setEmailError(INVALID_EMAIL);
            errorIndicator = false;
        }
        else
            setEmailError('');

        if (!IsValidString(password)) {
            setPasswordError(INVALID_PASSWORD);
            errorIndicator = false;
        }
        else
            setPasswordError('');
        return errorIndicator;
    }

    async function sendLoginRequest() {
        if (isValidModel()) {
            let credentials = {
                'email': email,
                'password': password
            }
            await axios.post(LOGIN_PREFIX_ROUTE, credentials)
                .then((response) => {
                    setLoginFailed('');
                    redirectToHome(response.data);
                })
                .catch((error) => {
                    setLoginFailed(LOGIN_FAILED);
                });
        }
    }

    return (
        <>
            < Container fluid className='login-container monteserrat-font'>
                <Container className='login-form-container'>
                    <div className='login-logo-image'>
                        <img src={logo} alt='logo' />
                    </div>
                    <Container className='login-form'>
                        <Row className='margin-0'>
                            <Col>
                                <div className='login-label'>
                                    Email
                                </div>
                                <div className='login-row'>
                                    <div className='login-icon-container'>
                                        <FaUser className='login-icon' />
                                    </div>
                                    <input value={email} className="login-input" type="text" onInput={e => setEmail(e.target.value)} />
                                </div>
                                <label className="login-input-error">{emailError}</label>
                            </Col>
                        </Row>
                        <Row className='margin-0'>
                            <Col>
                                <div className='login-label'>
                                    Lozinka
                                </div>
                                <div className='login-row'>
                                    <div className='login-icon-container'>
                                        <FaKey className='login-icon' />
                                    </div>
                                    <input value={password} className="login-input" type="password" onInput={e => setPassword(e.target.value)} />
                                </div>
                                <label className="login-input-error">{passwordError}</label>
                            </Col>
                        </Row>
                        <Row className='margin-0'>
                            <Col>
                                <label className="login-failed">{loginFailed}</label>
                            </Col>
                        </Row>
                        <Row className='margin-0'>
                            <Col>
                                <button className="login-submit-button" onClick={sendLoginRequest} >Prijavi se</button>
                            </Col>
                        </Row>
                    </Container>
                </Container>
            </Container >
        </>
    )
}

export default Login
