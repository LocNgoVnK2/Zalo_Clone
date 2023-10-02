import React, { useEffect, useRef, useState } from "react";
import Test from "../assets/test.png";
import { b64toBlob } from "../../Services/Util";
import { Button } from "react-bootstrap";
import MessageUser from "./MessageUser";
import MessageContact from "./MessageContact";
import MessageContent from "./MessageContent";
function MessageAttachment(props) {
  const [isFinishLoading, setIsFinishLoading] = useState(false);
  const [isUserMessage, setIsUserMessage] = useState();
  const [messageAttachment, setMessageAttachment] = useState(null);
  const [attachmentUrl, setAttachmentUrl] = useState();
  const downloadRef = useRef(null);
  const [fileName, setFileName] = useState(null);
  const [fileType, setFileType] = useState(null);
  const [isImage, setIsImage] = useState(null);
  const [content, setContent] = useState(null);
  const innerRef = props.innerRef;
  const initMessageInformation = () => {
    setIsUserMessage(props.message.sender === props.userId);
  };
  const initMessageFileInfor = () => {
    let blob = b64toBlob(
      props.messageAttachment.attachmentByBase64,
      props.messageAttachment.fileType
    );
    let blobUrl = window.URL.createObjectURL(blob);
    setAttachmentUrl(blobUrl);
    setFileName(props.messageAttachment.fileName);
    setFileType(props.messageAttachment.fileType);
    if (props.messageAttachment.fileType?.includes("image/")) {
      setIsImage(true);
    }
  };
  useEffect(() => {
    initMessageInformation();
    initMessageFileInfor();
    setIsFinishLoading(true);
  }, []);
  useEffect(() => {
    if (isImage) {
      renderImage();
    } else renderAttachment();
  }, [isFinishLoading]);
  const downLoadFile = () => {
    downloadRef?.current?.click();
  };
  const renderImage = () => {
    let imageUrl =
      "data:" +
      fileType +
      ";base64," +
      props.messageAttachment.attachmentByBase64;
    let content = (
      <>
        <div>
          <img src={imageUrl} alt="" style={{ maxWidth: "500px" }} />
          <a
            style={{ display: "none" }}
            download={fileName}
            href={attachmentUrl}
            ref={downloadRef}
          >
            a
          </a>
        </div>
      </>
    );
    let messageAttachment = isUserMessage ? (
      <MessageUser innerRef={innerRef}
        type="button"
        additionalClassName="attachmentButton"
        properties={{ onClick: downLoadFile }}
      >
        {content}
      </MessageUser>
    ) : (
      <MessageContact innerRef={innerRef}
        type="button"
        additionalClassName="attachmentButton"
        properties={{ onClick: downLoadFile }}
      >
        {content}
      </MessageContact>
    );
    setMessageAttachment(messageAttachment);
  };
  const renderAttachment = () => {
    let content;
    content = (
      <>
        <div>
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
      </>
    );
    setContent(content);
    let messageAttachment = isUserMessage ? (
      <MessageUser
        type="button"
        additionalClassName="attachmentButton"
        properties={{ onClick: downLoadFile }}
      >
        <MessageContent content={content} sendTime={props.message.sendTime} />
      </MessageUser>
    ) : (
      <MessageContact
        type="button"
        additionalClassName="attachmentButton"
        properties={{ onClick: downLoadFile }}
        sender={props.message.senderName}
      >
        <MessageContent content={content} sendTime={props.message.sendTime} />
      </MessageContact>
    );
    setMessageAttachment(messageAttachment);
  };
  const render = () => {
    if (!isFinishLoading) {
      return null;
    }
    return messageAttachment;
  };
  return render();
}

export default MessageAttachment;
