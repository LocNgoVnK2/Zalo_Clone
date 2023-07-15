import React, { Component } from "react";
import Nav from "react-bootstrap/Nav";
import NavDropdown from "react-bootstrap/NavDropdown";
import Test from "./assets/test.png";
import { ListGroup, ListGroupItem } from "react-bootstrap";

class ConversationList extends Component {


  render() {
    let rows = [];
    for(let i = 0;i < 50;i++){
      rows.push(<ListGroupItem action>
        abcv
      </ListGroupItem>);
    }
    return (
      <>
        <div className="conversation-list-container">
          <div className="contact-search">
            <span className="contact-search-box"></span>
            <span className="contact-search-button"></span>
          </div>
          <div className="conversation-list">
            <ListGroup variant="pills">
              {rows}
            </ListGroup>
          </div>
        </div>
      </>
    );
  }
}

export default ConversationList;
