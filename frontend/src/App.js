import React, { Component } from 'react';
import { Router, Routes, Route } from 'react-router-dom';
import './App.scss';
import Login from './Components/Login';
import Home from './Components/Home';
import Header from './Components/Header';
class App extends Component {
  render() {
    return (
     
        <div className='app-container'>
          <Routes>
       
            <Route path="/" element={<Login />} />
            <Route path="/home" element={<Home />} />
          </Routes>
        </div>
      
    );
  }
}

export default App;