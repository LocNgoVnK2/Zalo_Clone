import React, { useEffect, useLayoutEffect, useRef, useState } from "react";
import Test from "../assets/test.png";
import Message from "./Message";
import {
  PollingForNewMessage,
  GetMessagesFromContactOfUser,
  SendMessageToContact,
} from "../../Services/MessageServices";
import { fileToBase64 } from "../../Services/Util";
import ImageIcon from "../assets/icon/imageIcon.png";
import {
  GetUserContacts,
  GetContactInformationById,
} from "../../Services/userService";
import ConversationList from "../ConversationList";
import { Button, Form, FormControl } from "react-bootstrap";
import MessageAttachment from "./MessageAttachment";
function MessageView(props) {
  const [contacts, setContacts] = useState();
  const [contactInformation, setContactInformation] = useState();
  const [message, setMessage] = useState();
  const [isChatting, setIsChatting] = useState(false);
  const [chattingFrame, setChattingFrame] = useState();
  const [idMessageSrc, setIdMessageSrc] = useState(null);
  const messageInput = useRef();
  const bottomRef = useRef();
  const userId = props.userId;
  const [isThereNewMessage, setIsThereNewMessage] = useState(false);

  const inputFile = useRef(null);
  const openFileDialog = () => {
    inputFile.current?.click();
  };
  const submitImage = (file) => {
    console.log(file.type);
    const avatarImage = file;
    // if (avatarImage && avatarImage.type.startsWith('image/')) {
    //     fileToBase64(avatarImage, (callback) => console.log(callback));
    // }
    fileToBase64(avatarImage, (fileByBase64) => {
      let stringBase64 = fileByBase64.split(",")[1];
      let messageAttachment = [
        {
          fileName: file.name,
          fileType: file.type,
          attachmentByBase64: stringBase64,
        },
      ];

      SendMessageToContact(
        userId,
        contactInformation.id,
        null,
        idMessageSrc,
        messageAttachment
      ).then((response) => {
        if (response.status === 200) {
          setIsThereNewMessage(true);
        }
      });
    });
  };
  const scrollToBottom = () => {
    bottomRef.current?.scrollIntoView({
      behavior: "smooth",
      block: "end",
      inline: "nearest",
    });
  };
  const updateConversationList = () => {
    GetUserContacts(userId).then((response) => {
      setContacts(response.data);
    });
  };
  const handleMessageInputChange = (value) => {
    messageInput.current.value = value;
  };
  const handleSendMessage = () => {
    SendMessageToContact(
      userId,
      contactInformation.id,
      messageInput.current.value,
      idMessageSrc,
      null
    ).then((response) => {
      if (response.status === 200) {
        setIsThereNewMessage(true);

        messageInput.current.value = "";
      } else {
        alert("Can't send message to user " + contactInformation.contactName);
      }
    });
  };
  const updateChatView = (contactId) => {
    if (!userId || !contactId) return;
    GetMessagesFromContactOfUser(userId, contactId).then((response) => {
      let messageTemp = [];

      response.data.forEach((element) => {
        if(element.content){
          messageTemp.push(<Message message={element} userId={userId} />);
        }

        if (element.messageAttachments) {
          element.messageAttachments.forEach((messageAttachment) => {
            messageTemp.push(
              <MessageAttachment
                message={element}
                userId={userId}
                messageAttachment={messageAttachment}
              />
            );
          });
        }
      });
      setMessage(messageTemp);
    });
  };
  useEffect(() => {
    updateConversationList();
    let apiTimeout = setTimeout(pollingFunc, 1000);
    function pollingFunc() {
      PollingForNewMessage(userId)
        .then((res) => {
          if (res.status === 200) {
            setIsThereNewMessage(true);
            apiTimeout = setTimeout(pollingFunc, 1000);
          } else if (res.status === 204) {
            apiTimeout = setTimeout(pollingFunc, 1000);
          } else {
            clearTimeout(apiTimeout);
          }
        })
        .catch(() => {
          clearTimeout(apiTimeout);
        });
    }
  }, []);
  useEffect(() => {
    if (!isThereNewMessage) return;
    updateConversationList();
    if (!contactInformation) return;
    updateChatView(contactInformation.id);
    setIsThereNewMessage(false);
  }, [isThereNewMessage]);
  useEffect(() => {
    if (contactInformation && Object.keys(contactInformation).length !== 0) {
      setChattingFrame(
        <div className="message-view float-start">
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
              <div className="contact-name">
                {contactInformation.contactName}
              </div>
              <div className="subtitle">subtitle</div>
            </div>
          </header>
          <article className="chat-view" id="chat-view">
            {message}
            <div ref={bottomRef} />
          </article>
          <div className="tool-bar">
            <div className="open-file">
              <input
                id="file-input"
                type="file"
                //    accept="image/*"
                style={{ display: "none" }}
                ref={inputFile}
                onChange={(event) => {
                  submitImage(event.target.files[0]);
                  event.target.value = null;
                }}
            
              />
              <button onClick={openFileDialog}>
                <img src={ImageIcon} alt="" />
              </button>
            </div>
          </div>
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
        </div>
      );
      setIsChatting(true);
      setTimeout(() => {
        scrollToBottom();
      }, 1000);
    }
  }, [contactInformation, message]);
  return isChatting ? (
    <>
      <ConversationList
        contacts={contacts}
        updateChatView={updateChatView}
        id={userId}
        setContactInformation={setContactInformation}
      />
      <div>{chattingFrame}</div>
    </>
  ) : (
    <>
      <ConversationList
        contacts={contacts}
        updateChatView={updateChatView}
        id={userId}
        setContactInformation={setContactInformation}
      />
    </>
  );
}
export default MessageView;
