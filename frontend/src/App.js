import React, { Component } from 'react';
import { Router, Routes, Route } from 'react-router-dom';
import './App.scss';
import Login from './Components/Login';
import Home from './Components/Home';
import Header from './Components/Header';
class App extends Component {
  render() {
    return (
     
<<<<<<< HEAD
        <div>
          
=======
        <div className='app-container'>
>>>>>>> e425baa3fb31ed6e0a86b688b8a392de14e20845
          <Routes>
       
            <Route path="/" element={<Login />} />
            <Route path="/home" element={<Home />} />
          </Routes>
        </div>
      
    );
  }
}

export default App;