import React, { Component } from "react";
import Sidebar from "./Sidebar";
import ConversationList from "./ConversationList";
import Header from "./Header";
import jwtDecode from "jwt-decode";
import { getuserApi, GetContactInformationById, GetUserContacts } from "../Services/userService";
import { GetMessagesFromContactOfUser } from "../Services/MessageServices";
import Main from "./Main";
const HomeState = {
  None: "none",
  Message: "Message",
};
class Home extends Component {
  constructor(props) {
    super(props);
    this.state = {
      email: "",
      userId: "",
      messageContact: [],
      contactInformation: {},
      contacts: []
    };
    this.currentState = HomeState.None;
    //this.GetUserData();
  }
  // GetUserData =  async() => {
  //   const token = localStorage.getItem("token");
  //   if (token) {
  //     const user = jwtDecode(token);
  //     const emailU =
  //       user[
  //         "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
  //       ];

  //     this.setState({ email: emailU, userId: await getuserApi(emailU).id }, this.render);
  //   }
  // };
  // CallApiDataforUser = async (email) => {
  //   let res = await getuserApi(email);
  //   if (res) {
  //     this.setState({ userId: res.id }, this.render);
  //   }
  // };
  componentDidMount = () => {
    const token = localStorage.getItem("token");
    if (token) {
      const user = jwtDecode(token);
      const emailU =
        user[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        ];

      getuserApi(emailU).then((response) => {
        this.setState({ email: emailU, userId: response.data.id });
      });
      // this.setState({ email: emailU, userId: (await getuserApi(emailU)).id });
      this.state.userId && this.updateConversationList();
    }
  };
  componentDidUpdate = async (prevProps, prevState) => {
     if (prevState.userId !== this.state.userId) {
       this.updateConversationList();
     }
  };

  updateChatView = (contactId) => {
    if (!this.state.userId || !contactId) return;
    GetMessagesFromContactOfUser(this.state.userId, contactId).then(
      (response) => {
        this.setState({
          messageContact: response.data,
        });
      }
    );
    GetContactInformationById(contactId).then((response) => {
      this.setState({ contactInformation: response.data });
    });
  };
  updateConversationList = () => {
    GetUserContacts(this.state.userId).then((response) => {
      this.setState({
        contacts: response.data,
      });})

  }
  changeState = (state) => {
    // this.setState( { currentState: state } );
    this.currentState = state;
    alert(this.currentState);
  };
  render = () => {
    let conversation = (
      <ConversationList
        contacts={this.state.contacts}
        updateChatView={this.updateChatView}
      />
    );
    return (
      <div>
        {/* <button onClick={this.testFunction()}></button> */}
        <Sidebar changeState={this.changeState} />
        {conversation}
        <Main
          messageContact={this.state.messageContact}
          contactInformation={this.state.contactInformation}
          userId={this.state.userId}
          updateConversationList = {this.updateConversationList}
        />
      </div>
    );
  };
}
export default Home;
export const { None, Message } = HomeState;
