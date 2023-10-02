import React, { useEffect, useRef, useState } from "react";
import MessageUser from "./MessageUser";
import Test from "../assets/test.png";
import MessaeContact from "./MessageContact";
import MessageContent from "./MessageContent";
import MessageContact from "./MessageContact";
import { Button } from "react-bootstrap";
function MessageHeader(props) {
  const [contactName, setContactName] = useState();
  useEffect(() =>{
    setContactName(props.contactName);
  },[props.contactName])
  return (
    <header className="header">
      <div className="contact-avatar float-start">
        <img
          src={Test}
          alt=""
          width="100%"
          height="100%"
          className="rounded-circle"
        />
      </div>
      <div className="chat-title">
        <div className="contact-name">{contactName}</div>
        <div className="subtitle">subtitle</div>
      </div>
    </header>
  );
}

export default MessageHeader;
