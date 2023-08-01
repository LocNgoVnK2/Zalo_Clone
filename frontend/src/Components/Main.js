import React, { useEffect, useState } from "react";
import Test from "./assets/test.png";
import Message from "./Message";
function Main(props) {
  const [contactName, setContactName] = useState();
  const [message, setMessage] = useState();
  useEffect(() => {
    setContactName(props.contactInformation.contactName);
    let messageTemp = [];
    props.messageContact.forEach((element) => {
      messageTemp.push(<Message message={element} userId={props.userId}/>);
    });
    setMessage(messageTemp);
  }, [props.contactInformation, props.messageContact, props.userId]);

  return (

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
      <article className="chat-view">
        {message}
        </article>
      <div className="text-input"></div>
    </div>
  );
}

export default Main;
