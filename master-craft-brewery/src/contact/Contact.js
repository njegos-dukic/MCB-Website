import React, { useState, useEffect } from "react";
import * as Messages from '../constants/Messages';
import './Contact.css'
import GoogleApiWrapper from './WholesaleLocation.js'
import { Row, Col, Container } from 'reactstrap'
import axios from 'axios'
import { IsValidEmail, IsValidString } from "../error-handling/InputValidator";
import { COMPANY_INFO_PREFIX } from "../constants/ApiRoutes";
import Reaptcha from 'reaptcha';
import { ToastContainer } from 'react-toastify';
import { ShowError, ShowResponse } from "../admin/Notifier";

export function Contact() {
  const [wholesaleLocations, setWholesaleLocations] = useState([])
  const [verified, setVerified] = useState(false)

  const [nameInput, setNameInput] = useState('')
  const [nameError, setNameError] = useState('')

  const [surnameInput, setSurnameInput] = useState('')
  const [surnameError, setSurnameError] = useState('')

  const [emailInput, setEmailInput] = useState('')
  const [emailError, setEmailError] = useState('')

  const [contentInput, setContentInput] = useState('')
  const [contentError, setContentError] = useState('')

  const [captchaError, setCaptchaError] = useState('')

  const submitMessage = async () => {
    setCaptchaError('')

    if (!verified) {
      setCaptchaError(Messages.INVALID_CAPTCHA)
      return;
    }

    var valid = true

    setNameError('')
    if (!IsValidString(nameInput)) {
      setNameError(Messages.INVALID_NAME)
      valid = false
    }

    setSurnameError('')
    if (!IsValidString(surnameInput)) {
      setSurnameError(Messages.INVALID_SURNAME)
      valid = false
    }

    setEmailError('')
    if (!IsValidEmail(emailInput)) {
      setEmailError(Messages.INVALID_EMAIL)
      valid = false
    }

    setContentError('')
    if (!contentInput.length > 0) {
      setContentError(Messages.INVALID_CONTENT)
      valid = false
    }

    if (!valid)
      return

    else {
      let contactMessage = {
        firstName: nameInput,
        lastName: surnameInput,
        email: emailInput,
        message: contentInput
      }
      await axios.post('/contact', contactMessage)
        .then((response) => {
          ShowResponse(response);
          if (!response.data.isSent) {
            setCaptchaError(Messages.UNSUCCESSFUL_SENDING);
            return;
          }

          setNameInput('')
          setSurnameInput('')
          setEmailInput('')
          setContentInput('')
        }).catch((error) => { ShowError(error) });;
    }
  }

  const onVerify = (recaptchaResponse) => {
    setVerified(true);
  };

  async function fetchWholesales(source) {
    let localWholesales = null
    await axios.get(COMPANY_INFO_PREFIX, { cancelToken: source.token }).then((response) => {
      localWholesales = response.data.wholesales.map((wholesale, index) => {
        let marker =
        {
          "key": index,
          "name": wholesale.name,
          "title": wholesale.name,
          "position":
          {
            "lat": wholesale.coordinates.split(',')[0] - 0,
            "lng": wholesale.coordinates.split(' ')[1] - 0
          }
        }
        return marker
      })
    })

    return localWholesales
  }

  useEffect(() => {
    const cancelToken = axios.CancelToken
    const source = cancelToken.source()

    fetchWholesales(source).then(result => {
      setWholesaleLocations(result)
    })
    return () => {
      setWholesaleLocations([])
      source.cancel()
    }
  }, []);

  return (
    <div className="base-container">
      <div className="contact-container" >
        <form className="contact-form">
          <input value={nameInput} className="contact-input" onInput={e => setNameInput(e.target.value)} type="text" placeholder="Ime" />
          <label className="input-error"> {nameError} </label>

          <input value={surnameInput} className="contact-input" onInput={e => setSurnameInput(e.target.value)} type="text" placeholder="Prezime" />
          <label className="input-error"> {surnameError} </label>

          <input value={emailInput} className="contact-input" onInput={e => setEmailInput(e.target.value)} type="text" placeholder="e-mail" />
          <label className="input-error"> {emailError} </label>

          <textarea value={contentInput} className="contact-text-area" onInput={e => setContentInput(e.target.value)} placeholder="Poruka" />
          <label className="input-error"> {contentError} </label>

          <Reaptcha className="captcha" sitekey={'6LcyxXAbAAAAAFa2ho1OcCs_J48t_cj8DGtm4nAl'} onVerify={onVerify} />

          <button className="contact-submit-button" type="button" onClick={submitMessage}>POŠALJI PORUKU</button>
          <label className="captcha-error"> {captchaError} </label>

        </form>
      </div>

      <Container>
        <Row>
          <Col className="map-wrapper">
            <div className="wholesale-wrapper"> </div>
            <p className="wholesale">LOKACIJE MALOPRODAJE</p>
            <GoogleApiWrapper markers={wholesaleLocations} />
          </Col>
        </Row>
      </Container>
      <ToastContainer
        position="bottom-right"
        autoClose={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
      />
    </div>
  )
}