import React, { Component } from "react";
import Sidebar from "./Sidebar";
import ConversationList from "./ConversationList";
import Header from "./Header";
import jwtDecode from 'jwt-decode';
const HomeState = {
  None: "none",
  Message: "Message",
};
class Home extends Component {
  constructor(props) {
    super(props);
     this.state = {
       email:''
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
      console.log(this.state.email);
    }
  }
 
  changeState = (state) => {
    // this.setState( { currentState: state } );
    this.currentState = state;
    alert(this.currentState);
  };
  render() {
    return (
      <div>
        
        <Sidebar changeState={this.changeState} />
        <ConversationList/>
      </div>
    );
  }
}
export default Home;
export const { None, Message } = HomeState;
