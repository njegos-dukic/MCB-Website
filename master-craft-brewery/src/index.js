import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import axios from 'axios';

axios.defaults.baseURL = 'https://localhost:44365/api/v0.1';
axios.defaults.headers.get['Accept'] = 'application/json';
axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.headers.common['x-api-key'] = process.env.REACT_APP_MCB_API_KEY;

ReactDOM.render(
  <BrowserRouter basename={document.getElementsByTagName('base')[0].getAttribute('href')}>
    <App />
  </BrowserRouter>,
  document.getElementById('root')
);
