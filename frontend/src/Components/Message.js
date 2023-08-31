import React, { useEffect, useState } from "react";
import Test from "./assets/test.png";
import { GetContactInformationById } from "../Services/userService";
function Message(props) {
  const [sendTime, setSendTime] = useState();
  const [isFinishLoading, setIsFinishLoading] = useState();
  const [sender, setSender] = useState();
  const [contactMessage, setContactMessage] = useState();
  const [userMessage, setUserMessage] = useState();
  const [isUserMessage, setIsUserMessage] = useState();


useEffect(() => {
  setSender(props.message.senderName);

  let time = new Date(props.message.sendTime);
  let hours = time.getHours();
  let minutes = time.getMinutes();
  minutes = minutes < 10 ? "0" + minutes : minutes;
  setSendTime(hours + ":" + minutes);
  setIsUserMessage(props.message.sender === props.userId);
  setIsFinishLoading(true);
},[])

  useEffect(() => {
    if (isUserMessage) {
      setUserMessage(
        <div className="message message-user">
          <div className="message-content">{props.message.content}</div>
          <div className="send-time">{sendTime}</div>
        </div>
      );
    } else
      setContactMessage(
        <div className="message message-contact">
          <div className="contact-name">{sender}</div>
          <div className="message-content">{props.message.content}</div>
          <div className="send-time">{sendTime}</div>
        </div>
      );

  }, [props.message, isUserMessage, sender, sendTime]);

  return isFinishLoading
    ? isUserMessage
      ? userMessage
      : contactMessage
    : null;
}

export default Message;
