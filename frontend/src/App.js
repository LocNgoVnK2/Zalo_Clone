import React, { Component } from 'react';

import './App.css';
import Login from './Components/Login';
import Header from './Components/Header';
class App extends Component {
  handleSelect = (eventKey) =>{ alert(`selected ${eventKey}`)};


  render() {
    return (

   <div>

      <Login/>
   </div>
);
  }
}

export default App;