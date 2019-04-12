import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
// import './index.css';

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';

import Container from './components/container';

// import App from './App';
// import registerServiceWorker from './registerServiceWorker';

import 'semantic-ui-css/semantic.min.css';
import 'semantic-ui-css/components/button.min.css';
import 'semantic-ui-css/components/form.min.css';
import 'semantic-ui-css/components/segment.min.css';
import 'semantic-ui-css/components/image.min.css';
import 'semantic-ui-css/components/input.min.css';
import 'semantic-ui-css/components/header.min.css';
import 'semantic-ui-css/components/grid.min.css';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl} >
    <Container />
  </BrowserRouter>,
  rootElement);

// registerServiceWorker();
