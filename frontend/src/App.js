import React, { Component } from 'react';

import './App.css';
import Login from './Components/Login';

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