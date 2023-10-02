import React, { useEffect, useRef, useState } from "react";
import MessageUser from "./MessageUser";
import Test from "../assets/test.png";
import MessaeContact from "./MessageContact";
import MessageContent from "./MessageContent";
import MessageContact from "./MessageContact";
import { Button } from "react-bootstrap";
import { fileToBase64 } from "../../Services/Util";
import ImageIcon from "../assets/icon/imageIcon.png";
function MessageInput(props) {
  const messageInput = useRef();
  const handleMessageInputChange = (value) => {
    messageInput.current.value = value;
  };
  const handleSendMessage = () => {
    props.handleSendMessage(messageInput.current.value);
    console.log(messageInput.current.value);
    messageInput.current.value = "";
   
  }
  return (
    <div className="text-input">
    <input
      id="input-line"
      className="input-line"
      type="text"
      placeholder="Nhập tin nhắn"
      ref={messageInput}
      onChange={(event) => {
        handleMessageInputChange(event.target.value);

      }}
    />
    <button className="submit" onClick={handleSendMessage}>
      GỬI
    </button>
  </div>
  );
}

export default MessageInput;
