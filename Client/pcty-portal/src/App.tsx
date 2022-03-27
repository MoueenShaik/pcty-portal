import React from 'react';
import logo from './logo.png';
import './App.css';
import Employer from './components/Employer';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo}  alt="logo" />       
        <a
          className="App-link"
          href="https://www.paylocity.com/"
          target="_blank"
          rel="noopener noreferrer"
        >
          Paylocity Home
        </a>
      </header>
      <body>
        <b>Employer Portal</b>
      <Employer></Employer>

      </body>
    </div>
  );
}

export default App;
