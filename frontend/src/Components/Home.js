import React, { Component } from "react";
import Sidebar from "./Sidebar";
import ConversationList from "./ConversationList";
import Header from "./Header";
import jwtDecode from 'jwt-decode';
import { getuserApi } from "../Services/userService";
const HomeState = {
  None: "none",
  Message: "Message",
};
class Home extends Component {
  constructor(props) {
    super(props);
    this.state = {
      email: '',
      id:''
    }
    this.currentState = HomeState.None;

  }
  componentDidMount() {
    const token = localStorage.getItem('token');
    if (token) {
      const user = jwtDecode(token);
      const emailU = user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
      this.setState({ email: emailU });

    }
  }
  componentDidUpdate(prevProps, prevState) {
    if (prevState.email !== this.state.email) { 
      this.CallApiDataforUser(this.state.email);
    }
  }

  CallApiDataforUser = async (email) => {
    let res = await getuserApi(email);
    if (res) {
      this.setState({ id: res.id });
    }
   
  }
  testFunction = () => {
    const idValue = this.state.id;
    console.log(idValue);
  }

  changeState = (state) => {
    // this.setState( { currentState: state } );
    this.currentState = state;
    alert(this.currentState);
  };
  render() {
    return (
      <div>
        <button onClick={this.testFunction()}></button>
        <Sidebar changeState={this.changeState} />
        <ConversationList />
      </div>
    );
  }
}
export default Home;
export const { None, Message } = HomeState;
