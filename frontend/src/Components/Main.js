import React, { useEffect, useState } from "react";
import Test from "./assets/test.png";
import Message from "./Message";
import {
  PollingForNewMessage,
  GetMessagesFromContactOfUser,
} from "../Services/MessageServices";

function Main(props) {
  const [contactName, setContactName] = useState();
  const [contactInformation, setContactInformation] = useState();
  const [message, setMessage] = useState();
  const [isThereNewMessage, setIsThereNewMessage] = useState(false);
  const [isChatting, setIsChatting] = useState(false);
  const [chattingFrame, setChattingFrame] = useState();

  useEffect(() => {
    console.log(props);
    // if (isChatting && props.messageContact.length !== 0) {
    let apiTimeout = setTimeout(pollingFunc, 1000);
    function pollingFunc() {
      PollingForNewMessage(props.userId).then((res) => {
        if (res.status === 200) {
          props.updateConversationList();
          props.updateChatView();
          apiTimeout = setTimeout(pollingFunc, 1000);
          setIsThereNewMessage(true);
        } else if (res.status === 204) {
          apiTimeout = setTimeout(pollingFunc, 1000);
          setIsThereNewMessage(true);
        } else {
          clearTimeout(apiTimeout);
        }
      });
    }

    // }
  }, []);
  useEffect(() => {
    if (props.messageContact.length !== 0) {
      let messageTemp = [];
      props.messageContact.forEach((element) => {
        messageTemp.push(
          <Message
            message={element}
            userId={props.userId}
            contactName={props.contactInformation.contactName}
          />
        );
      });
      setMessage(messageTemp);
    }
  }, [props.messageContact]);
  useEffect(() => {
    if (Object.keys(props.contactInformation).length !== 0) {
      setContactName(props.contactInformation.contactName);
      setChattingFrame(
        <div className="main float-start">
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
          <article className="chat-view" id="chat-view">
            {message}
          </article>
          <div className="text-input"></div>
        </div>
      );
      setIsChatting(true);
    }
  }, [props.contactInformation, isChatting, contactName, message]);

  return isChatting ? chattingFrame : null;
}

export default Main;
