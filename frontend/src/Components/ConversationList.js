import React, { useState } from "react";
import Test from "./assets/test.png";
import {
  Button,
  InputGroup,
  ListGroup,
  ListGroupItem,
} from "react-bootstrap";
import Form from "react-bootstrap/Form";
import SearchIcon from "./assets/icon/searchIcon.png";
import AddUserIcon from "./assets/icon/addUserIcon.png";
import AddGroupUserIcon from "./assets/icon/add-friend.png";
import AddFriendDialog from "./AddFriendDialog";
import CreateGroupDialog from "./CreateGroupDialog";

function ConversationList(props) {
  const [isFinishLoading, setIsFinishLoading] = useState(false);
  const [contactChosenId, setContactChosenId] = useState("");
  const [showAddFriendDialog, setShowAddFriendDialog] = useState(false);
  const [showCreateGroupDialog, setShowCreateGroupDialog] = useState(false);

  const handleOpenCreateGroupDialog = () => {
    setShowCreateGroupDialog(true);
  };

  const handleCloseCreateGroupDialog = () => {
    setShowCreateGroupDialog(false);
  };

  const handleOpenAddFriendDialog = () => {
    setShowAddFriendDialog(true);
  };

  const handleCloseAddFriendDialog = () => {
    setShowAddFriendDialog(false);
  };

  const updateChatView = props.updateChatView;

  let rows = [];
  if (props.contacts) {
    let contacts = props.contacts;
    for (let i = 0; i < contacts.length; i++) {
      let name = contacts[i].contactName;
      rows.push(
        <ListGroupItem
          key={contacts[i].id}
          action
          onClick={(e) => {
            if (contactChosenId !== contacts[i].id) {
              updateChatView(contacts[i].id, e);
              props.setContactInformation(contacts[i], e);
            }

            setContactChosenId(contacts[i].id);
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
            <div className="text-muted">{contacts[i].lastMessageContent}</div>
          </div>
        </ListGroupItem>
      );
    }
  }

  return (
    <>
      <CreateGroupDialog
        show={showCreateGroupDialog}
        handleClose={handleCloseCreateGroupDialog}
        userId={props.id}
      />
      <AddFriendDialog
        show={showAddFriendDialog}
        handleClose={handleCloseAddFriendDialog}
        userId={props.id}
      />
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
            <Button variant="light" onClick={handleOpenAddFriendDialog}>
              <img src={AddUserIcon} alt="" width="14 px" height="14 px" />
            </Button>
          </span>
          <span className="float-start ms-1">
            <Button variant="light" onClick={handleOpenCreateGroupDialog}>
              <img
                src={AddGroupUserIcon}
                alt=""
                width="14 px"
                height="14 px"
              />
            </Button>
          </span>
        </div>
        <div className="conversation-list">
          <ListGroup variant="pills">{rows}</ListGroup>
        </div>
      </div>
    </>
  );
}

export default ConversationList;
