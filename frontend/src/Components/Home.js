import React, { Component } from "react";
import Sidebar from "./Sidebar";
import ConversationList from "./ConversationList";
import Header from "./Header";
const HomeState = {
  None: "none",
  Message: "Message",
};
class Home extends Component {
  constructor(props) {
    super(props);
    // this.state = {
    //   currentState: HomeState.Message
    // }
    this.currentState = HomeState.None;
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
