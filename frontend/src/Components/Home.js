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
      userId: ''

    }
    this.currentState = HomeState.None;
  }
  CallApiDataforUser = async (email) => {
    let res = await getuserApi(email);
    if (res) {
      this.setState({userId : res.id}, this.render);
    }
  }
  componentDidMount = async () =>{
    const token = localStorage.getItem('token');
    if (token) {
      const user = jwtDecode(token);
      const emailU = user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
      this.setState({ email: emailU,
      userId : (await getuserApi(emailU)).id});

    }
  }
   componentDidUpdate = async(prevProps, prevState) => {
    if (prevState.email !== this.state.email) { 
      await this.CallApiDataforUser(this.state.email);
    }
  }


  changeState = (state) => {
    // this.setState( { currentState: state } );
    this.currentState = state;
    //alert(this.currentState);
  };
  render = () => {
    if(!this.state.userId)
      return;
    return (
      
      <div>
        <button onClick={this.testFunction()}></button>
        <Sidebar changeState={this.changeState} />
      
         <ConversationList id={this.state.userId}/>
        
       
      </div>
    );
  }
}
export default Home;
export const { None, Message } = HomeState;
