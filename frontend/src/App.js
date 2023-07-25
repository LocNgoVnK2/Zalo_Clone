import React, { Component } from "react";
import { Router, Routes, Route, useNavigate } from "react-router-dom";
import "./App.scss";
import Login from "./Components/Login";
import Home from "./Components/Home";
import Signup from "./Components/Signup";
import Header from "./Components/Header";
import Validation from "./Components/Validation"; 
import ForgotPassword from "./Components/ForgotPassword";
import RenewPassword from "./Components/RenewPassword"; 
import UpdatePassword from "./Components/UpdatePassword";
import NoPage from "./Components/NoPage";
class App extends Component {
  render() {
    return (
      <div>
     
          <Routes>
            <Route path="/" element={<Login navigate={this.props.navigate}/>} />
            <Route path="/home" element={<Home />} />
            <Route path="/signup" element={<Signup navigate={this.props.navigate}/>}/>
            <Route path="/validation" element={<Validation />} />
            <Route path="/forgotPassword" element={<ForgotPassword navigate={this.props.navigate} />} />
            <Route path="/renewPassword" element={<RenewPassword />} />
            <Route path="/updatePassword" element={<UpdatePassword navigate={this.props.navigate}/>} />
            <Route path="/404" element={<NoPage />} />
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
