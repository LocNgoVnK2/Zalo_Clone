import React, { useEffect, useRef, useState } from "react";
import Test from "./assets/test.png";
import { GetContactInformationById } from "../Services/userService";
import { b64toBlob } from "../Services/Util";
import { Button } from "react-bootstrap";
function Message(props) {
  const [sendTime, setSendTime] = useState();
  const [isFinishLoading, setIsFinishLoading] = useState();
  const [sender, setSender] = useState();
  const [contactMessage, setContactMessage] = useState();
  const [userMessage, setUserMessage] = useState();
  const [isUserMessage, setIsUserMessage] = useState();
  const [messageAttachment, setMessageAttachment] = useState(null);
  const [attachmentUrl, setAttachmentUrl] = useState();
  const downloadRef = useRef(null);
  const [fileName, setFileName] = useState(null);
  const [fileType, setFileType] = useState(null);
  const [isImage, setIsImage] = useState(null);
  useEffect(() => {
    setSender(props.message.senderName);
    if (props.messageAttachment) {
      let blob = b64toBlob(
        props.messageAttachment.attachmentByBase64,
        props.messageAttachment.fileType
      );
      let blobUrl = window.URL.createObjectURL(blob);
      setAttachmentUrl(blobUrl);
      setFileName(props.messageAttachment.fileName);
      setFileType(props.messageAttachment.fileType);
      if (fileType?.includes("image/")) {
        setIsImage(true);
      }
    }
    let time = new Date(props.message.sendTime);
    let hours = time.getHours();
    let minutes = time.getMinutes();
    minutes = minutes < 10 ? "0" + minutes : minutes;
    setSendTime(hours + ":" + minutes);
    setIsUserMessage(props.message.sender === props.userId);
    setIsFinishLoading(true);
  }, []);
  const downLoadFile = () => {
    downloadRef?.current?.click();
  };

  const handleAttachment = () => {
    let messageAttachment;
    let imageUrl =
      "data:" +
      fileType +
      ";base64," +
      props.messageAttachment.attachmentByBase64;
    messageAttachment = isUserMessage ? (
      <>
        <Button
          variant="light"
          className="message message-user"
          onClick={downLoadFile}
        >
          <div className="content">
            <img src={imageUrl} style={{ maxWidth: "500px" }} alt="" />
            {fileName}
            <a
              style={{ display: "none" }}
              download={fileName}
              href={attachmentUrl}
              ref={downloadRef}
            >
              a
            </a>
          </div>
          <div className="send-time">{sendTime}</div>
        </Button>
      </>
    ) : (
      <>
        <Button
          variant="light"
          className="message message-contact"
          onClick={downLoadFile}
        >
          <div className="contact-name">{sender}</div>
          <div className="content">
            <img src={imageUrl} style={{ maxWidth: "500px" }} alt="" />
            {fileName}
            <a
              style={{ display: "none" }}
              download={fileName}
              href={attachmentUrl}
              ref={downloadRef}
            >
              a
            </a>
          </div>
          <div className="send-time">{sendTime}</div>
        </Button>
      </>
    );
    setMessageAttachment(messageAttachment);
  };
  useEffect(() => {
    if (props.messageAttachment) {
      handleAttachment();
      return;
    }
    if (isUserMessage) {
      setUserMessage(
        <div className="message message-user">
          <div className="content">{props.message.content}</div>
          <div className="send-time">{sendTime}</div>
        </div>
      );
    } else
      setContactMessage(
        <div className="message message-contact">
          <div className="contact-name">{sender}</div>
          <div className="content">{props.message.content}</div>
          <div className="send-time">{sendTime}</div>
        </div>
      );
  }, [props.message, isUserMessage, sender, sendTime]);
  const render = () => {
    if (!isFinishLoading) {
      return null;
    }
    if (messageAttachment) {
      return messageAttachment;
    }
    if (isUserMessage) {
      return userMessage;
    }
    return contactMessage;
  };
  return render();
}

export default Message;
