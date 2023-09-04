import React, { useEffect, useRef, useState } from "react";
import MessageUser from "./MessageUser";
import MessaeContact from "./MessageContact";
import MessageContent from "./MessageContent";
import MessageContact from "./MessageContact";
import { Button } from "react-bootstrap";
function Message(props) {
  const [isFinishLoading, setIsFinishLoading] = useState();
  const [message, setMessage] = useState();
  useEffect(() => {
    let isUserMessage = props.message.sender === props.userId;
    let message = isUserMessage ? (
      <MessageUser>
        <MessageContent
          sendTime={props.message.sendTime}
          content={props.message.content}
        />
      </MessageUser>
    ) : (
      <MessageContact sender={props.message.senderName}>
        <MessageContent
          sendTime={props.message.sendTime}
          content={props.message.content}
        />
      </MessageContact>
    );
    setMessage(message);
    setIsFinishLoading(true);
  }, []);
  const render = () => {
    if (!isFinishLoading) {
      return null;
    }
    return message;
  };
  return render();
}

export default Message;
