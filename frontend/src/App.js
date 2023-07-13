import React, { Component } from 'react';
import { Router, Routes, Route } from 'react-router-dom';
import './App.css';
import Login from './Components/Login';
import Header from './Components/Header';

class App extends Component {
  render() {
    return (
     
        <div>
          
          <Routes>
            <Route path="/" element={<Login />} />
            <Route path="/home" element={<Header />} />
          </Routes>
        </div>
      
    );
  }
}

export default App;