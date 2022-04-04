import React from 'react';
import logo from './logo.png';
import './App.css';
import Employer from './components/Employer';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} alt="logo" />
        <a
          className="App-link"
          href="https://www.paylocity.com/"
          target="_blank"
          rel="noopener noreferrer"
        >
          Paylocity Web Site
        </a>
      </header>
      <body>
        <b>Employer Portal</b>
        <Employer></Employer>

      </body>
      <footer style={{
        content: '', display: 'block', paddingTop: '0.625rem', borderTop: '1px solid #ff8f1c',
        width: '100%'
      }} >
        <div className="info text-md-right px-4 px-md-0">         
          <p className="mb-md-0">1400 American Lane, Schaumburg, IL 60173</p>
          <p className="legal">
            Copyright © 2022 Paylocity. All Rights Reserved.
            <br className="d-md-none" />
            <br></br>
            <a href="https://www.paylocity.com/privacy-policy/">Privacy Policy</a>
            |
            <a href="https://www.paylocity.com/terms-and-conditions/">Terms and Conditions</a>
            |
            <a href="https://www.paylocity.com/bipa-policy/">BIPA Policy</a>
            |
          </p>


        </div>
      </footer>
    </div>
  );
}

export default App;
