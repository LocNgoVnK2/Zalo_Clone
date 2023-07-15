import React, { Component } from "react";
import { Router, Routes, Route, useNavigate } from "react-router-dom";
import "./App.scss";
import Login from "./Components/Login";
import Home from "./Components/Home";
import Header from "./Components/Header";
class App extends Component {
  render() {
    return (
      <div>
     
          <Routes>
            <Route path="/" element={<Login navigate={this.props.navigate}/>} />
            <Route path="/home" element={<Home />} />
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
