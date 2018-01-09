import React from 'react';
import { render } from 'react-dom';
import AppPage from './components/appPage.js';

render (
    <div>
            {console.log("in react-dom rendering window app initial state : "+ window.__APP_INITIAL_STATE__ )}
            {console.log(window.__APP_INITIAL_STATE__ )}
            <AppPage appProps = {window.__APP_INITIAL_STATE__}   />
    </div>,
    document.getElementById("app")
)