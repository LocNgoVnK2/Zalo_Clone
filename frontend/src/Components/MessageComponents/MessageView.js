import React, {
  createRef,
  useEffect,
  useLayoutEffect,
  useRef,
  useState,
} from "react";
import Message from "./Message";
import {
  PollingForNewMessage,
  GetMessagesFromContactOfUser,
  SendMessageToContact,
  GetUnNotifiedMessagesFromContactOfUser,
} from "../../Services/MessageServices";
import {
  GetUserContacts,
  GetContactInformationById,
} from "../../Services/userService";
import ConversationList from "../ConversationList";
import MessageAttachment from "./MessageAttachment";
import MessageHeader from "./MessageHeader";
import MessageToolBar from "./MessageToolBar";
import MessageInput from "./MessageInput";
function MessageView(props) {
  const [contacts, setContacts] = useState();
  const [contactInformation, setContactInformation] = useState();
  const [message, setMessage] = useState([]);
  const [isChatting, setIsChatting] = useState(false);
  const [chattingFrame, setChattingFrame] = useState();
  const [idMessageSrc, setIdMessageSrc] = useState(null);
  const [topMessageIndex, setTopMessageIndex] = useState(0);
  const bottomRef = useRef();
  const userId = props.userId;
  const [isThereNewMessage, setIsThereNewMessage] = useState(false);
  const messageViewRef = useRef();
  const messageLoadTimes = useRef(0);
  const isSendingMessage = useRef(false);
  const isContactChanged = useRef(false);
  const isContactHasNewMessage = useRef(false);
  const useCustomRefArray = () => {
    const refArray = useRef([]);
    const addRef = (ele) => {
      if (!ele) return;
      refArray.current.push(ele);
    };
    return [refArray.current, addRef];
  };
  const [messageRefArray, addMessageRefArray] =
    useCustomRefArray();
  const parsingMessage = (messages) => {
    if (messages.length === 0) return;

    let messageTemp = [];
    messageTemp.push(
      <>
        <div ref={(ele) => addMessageRefArray(ele)}/>
      </>
    )

    for (let i = 0; i < messages.length; i++) {
      
      let element = messages[i];
      if (element.content) {
        messageTemp.push(
          <>
            <Message
              key={element.id} 
              message={element}
              userId={userId}
            />
          </>
        );
      }

      if (element.messageAttachments) {
        element.messageAttachments.forEach((messageAttachment) => {
          messageTemp.push(
            <MessageAttachment
              key={element.id}
              message={element}
              userId={userId}
              messageAttachment={messageAttachment}
            />
          );
        });
      }
    }
    return messageTemp;
  };
  const sendAttachmentToCurrentContact = (messageAttachment) => {
    if (!userId || !contactInformation) return;
    SendMessageToContact(
      userId,
      contactInformation.id,
      null,
      idMessageSrc,
      messageAttachment
    ).then((response) => {
      if (response.status === 200) {
        setTopMessageIndex(0);
        message.length = 0;
        updateConversationList();
        loadMessageOfContact();
        messageLoadTimes.current = 0;
        isSendingMessage.current = true;
      }
    });
  };

  const handleScrollToTop = (event) => {
    if (event.currentTarget.scrollTop === 0) {
      setTopMessageIndex(topMessageIndex + 20);
    }
  };
  useEffect(() => {
    if(isContactChanged.current){
      scrollToBottom();
      isContactChanged.current = false;
      return;
    }
    if(isSendingMessage.current){
      scrollToBottom('instant');
      isSendingMessage.current = false;
      return;
    }
    if(isContactHasNewMessage.current){
      scrollToBottom();
      isContactHasNewMessage.current = false;
      return;
    }
    if(!messageRefArray || !messageRefArray[messageRefArray.length-messageLoadTimes.current])
      return; 
    
    setTimeout(() => {
      messageRefArray[messageRefArray.length-messageLoadTimes.current].scrollIntoView({
        behavior: "smooth",
        block: "end",
        inline: "nearest",
      });
    },500); 
  }, [message]);
  useEffect(() => {
    if (!contactInformation) return;
    if (topMessageIndex === 0) return;
  
    messageLoadTimes.current++;
    loadMessageOfContact(topMessageIndex);
  }, [topMessageIndex]);
  const scrollToBottom = (behavior) => {
    setTimeout(() => {
      behavior = behavior ? behavior : "smooth";
      bottomRef.current?.scrollIntoView({
        behavior: behavior,
        block: "end",
        inline: "nearest",
      });
    }, 100);
  };
  const updateConversationList = () => {
    GetUserContacts(userId).then((response) => {
      setContacts(response.data);
    });
  };
  const handleSendMessage = (content) => {
    SendMessageToContact(
      userId,
      contactInformation.id,
      //  messageInput.current.value,
      content,
      idMessageSrc,
      null
    ).then((response) => {
      if (response.status === 200) {
        setTopMessageIndex(0);
        message.length = 0;
        updateConversationList();
        loadMessageOfContact();
        messageLoadTimes.current = 0;
        isSendingMessage.current = true;
      } else {
        alert("Can't send message to user " + contactInformation.contactName);
      }
    });
  };
  const loadNewMessage = (contactId) => {
    if (!userId || !contactId) return;
    GetUnNotifiedMessagesFromContactOfUser(userId, contactId).then(
      (response) => {
        if (response.status === 200) {
          let messageTemp = [];
          if (response.data) {
            messageTemp = parsingMessage(response.data);
          }
            setMessage([...message,...messageTemp]);
        }
      });
  };
  const loadMessageOfContact = (topMessageIndex = 0) => {
    if (!userId || !contactInformation) return;
    GetMessagesFromContactOfUser(
      userId,
      contactInformation.id,
      topMessageIndex
    ).then((response) => {
      if (response.status === 200) {
        let messageTemp = [];
        if (response.data) {
          messageTemp = parsingMessage(response.data);
        }
        setMessage([...messageTemp, ...message]);
      }
    });
  };
  useEffect(() => {
    message.length = 0;
    messageRefArray.length = 0;
    messageLoadTimes.current = 0;
    setTopMessageIndex(0);
    isContactChanged.current = true;
    loadMessageOfContact();
  }, [contactInformation]);
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
    console.log("Cos messsage");
    loadNewMessage(contactInformation.id);
    isContactHasNewMessage.current = true;
    setIsThereNewMessage(false);
  }, [isThereNewMessage]);
  useEffect(() => {
    if (contactInformation && Object.keys(contactInformation).length !== 0) {
      setChattingFrame(
        <div className="message-view float-start">
          <MessageHeader contactName={contactInformation.contactName} />
          <article
            className="chat-view"
            id="chat-view"
            onScroll={(e) => handleScrollToTop(e)}
            ref={messageViewRef}
          >
            {message}

            <div ref={bottomRef} />
          </article>
          <MessageToolBar
            sendAttachmentToCurrentContact={sendAttachmentToCurrentContact}
          />
          <MessageInput handleSendMessage={handleSendMessage} />
        </div>
      );
      setIsChatting(true);
    }
  }, [contactInformation, message]);
  return isChatting ? (
    <>
      <ConversationList
        contacts={contacts}
        id={userId}
        setContactInformation={setContactInformation}
      />
      <div>{chattingFrame}</div>
    </>
  ) : (
    <>
      <ConversationList
        contacts={contacts}
        id={userId}
        setContactInformation={setContactInformation}
      />
    </>
  );
}
export default MessageView;
