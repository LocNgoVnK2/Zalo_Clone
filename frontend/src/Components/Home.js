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
    this.loadDataUser = this.loadDataUser.bind(this);
  }
  componentDidMount() {
    this.loadDataUser();
  }
  loadDataUser=(e)=>{
    const res = { token: localStorage.getItem('token') };
    if (res.token) {
      const user = jwtDecode(res.token);
      const emailU = user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
      this.setState({ email: emailU }, () => {
        console.log(this.state.email); // Access the updated state here
      });
      
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
