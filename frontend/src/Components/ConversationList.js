import React, { Component } from "react";
import Nav from "react-bootstrap/Nav";

import NavDropdown from "react-bootstrap/NavDropdown";
import Test from "./assets/test.png";
import { Button, InputGroup, ListGroup, ListGroupItem } from "react-bootstrap";
import Form from "react-bootstrap/Form";
import SearchIcon from "./assets/icon/searchIcon.png";
import AddUserIcon from "./assets/icon/addUserIcon.png";
import { GetUserContacts, getuserApi } from "../Services/userService";
class ConversationList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isFinishLoading: false,
      contacts: [],
      contactChosenId: "",
    };
  }
  componentDidMount = () => {
    this.props.id &&
      GetUserContacts(this.props.id).then((response) => {
        this.setState({
          contacts: response,
          isFinishLoading: true,
        });
      });
  };
  componentDidUpdate(prevProps, prevState) {
    if (prevProps.id !== this.props.id) {
      GetUserContacts(this.props.id).then((response) => {
        this.setState({
          contacts: response,
          isFinishLoading: true,
        });
      });
    }
  }
  render = () => {
    let updateChatView = this.props.updateChatView;
    let rows = [];
    if (this.state.contacts) {
      let contacts = this.state.contacts;
      for (let i = 0; i < contacts.length; i++) {
        let name = contacts[i].contactName;
        rows.push(
          <ListGroupItem
            key={contacts[i].id}
            //disabled={this.state.contactChosenId === contacts[i].id}
            action
            onClick={(e) => {
              if (this.state.contactChosenId !== contacts[i].id)
                updateChatView(contacts[i].id, e);
              this.setState({ contactChosenId: contacts[i].id });
            }}
          >
            <div className="float-start">
              <img
                src={Test}
                className="rounded-circle"
                width="48 px"
                height="48 px"
                alt=""
              />
            </div>
            <div className="float-start ms-3">
              <span className="float-right">{name}</span>
              <div className="text-muted">
                {this.state.contacts[i].lastMessageContent}
              </div>
            </div>
          </ListGroupItem>
        );
      }
    }

    return (
      <>
        <div className="conversation-list-container">
          <div className="contact-search">
            <span className="contact-search-box">
              <InputGroup size="sm" className="mb-3 float-start">
                <InputGroup.Text>
                  <img src={SearchIcon} alt="" />
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
                <img src={AddUserIcon} alt="" width="14 px" height="14 px" />
              </Button>
            </span>
            <span className="float-start ms-1">
              <Button variant="light">
                <img src={AddUserIcon} alt="" width="14 px" height="14 px" />
              </Button>
            </span>
          </div>
          <div className="conversation-list">
            <ListGroup variant="pills">{rows}</ListGroup>
          </div>
        </div>
      </>
    );
  };
}

export default ConversationList;
