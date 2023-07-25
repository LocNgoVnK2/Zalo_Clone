import React, { Component } from "react";
import Nav from "react-bootstrap/Nav";

import NavDropdown from "react-bootstrap/NavDropdown";
import Test from "./assets/test.png";
import { Button, InputGroup, ListGroup, ListGroupItem } from "react-bootstrap";
import Form from 'react-bootstrap/Form';
import SearchIcon from "./assets/icon/searchIcon.png";
import AddUserIcon from "./assets/icon/addUserIcon.png";
class ConversationList extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    let rows = [];
    for(let i = 0;i < 50;i++){
      rows.push(<ListGroupItem  key={i} action>
        <div className="float-start">
          <img src={Test} className="rounded-circle" width="48 px" height="48 px" alt=""/>
        </div>
        <div className="float-start ms-3">
          <span className="float-right">
            tên
          </span>
          <div className="text-muted"> 
            chat
          </div>
        </div>

      </ListGroupItem>);
    }
    return (
      <>
        <div className="conversation-list-container">
          <div className="contact-search">
            <span className="contact-search-box">
            <InputGroup size="sm" className="mb-3 float-start">
              <InputGroup.Text>
                <img src={SearchIcon} alt=""/>
              </InputGroup.Text>
        <Form.Control
          placeholder="Tìm kiếm"
          aria-label="Username"
          aria-describedby="basic-addon1"
          className="bg-light"
        />
      </InputGroup>
            </span>
            <span className="float-start ms-1">
              <Button variant="light">
                <img src={AddUserIcon} alt="" width="14 px" height="14 px"/>
              </Button>
            </span>
            <span className="float-start ms-1">
              <Button variant="light">
                <img src={AddUserIcon} alt="" width="14 px" height="14 px"/>
              </Button>
            </span>
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
