import React, { useEffect, useRef, useState } from "react";
import MessageUser from "./MessageUser";
import Test from "../assets/test.png";
import MessaeContact from "./MessageContact";
import MessageContent from "./MessageContent";
import MessageContact from "./MessageContact";
import { Button } from "react-bootstrap";
import { fileToBase64 } from "../../Services/Util";
import ImageIcon from "../assets/icon/imageIcon.png";
function MessageToolBar(props) {
  
  const inputFile = useRef(null);
  const openFileDialog = () => {
    inputFile.current?.click();
  };
  const submitImage = (file) => {
    let sendAttachmentToCurrentContact = props.sendAttachmentToCurrentContact;
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

      sendAttachmentToCurrentContact(messageAttachment);
    });
  };
  return (
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
  );
}

export default MessageToolBar;
