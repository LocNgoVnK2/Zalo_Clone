import React, { Component } from "react";
import { Router, Routes, Route, useNavigate } from "react-router-dom";
import "./App.scss";
import Login from "./Components/Login";
import Home from "./Components/Home";
import Signup from "./Components/Signup";
import Header from "./Components/Header";
import EmailAuthentication from "./Components/EmailAuthentication";

class App extends Component {
  render() {
    return (
      <div>
     
          <Routes>
            <Route path="/" element={<Login navigate={this.props.navigate}/>} />
            <Route path="/home" element={<Home />} />
            <Route path="/signup" element={<Signup navigate={this.props.navigate}/>}/>
            <Route path="/signup/email_authentication" element={<EmailAuthentication />} />
           
          </Routes>

      </div>
    );
  }
}
export function AppWithRouter(props){
  const navigate = useNavigate();
  return (<App {...props} navigate={navigate}/>);
}

export default App;
