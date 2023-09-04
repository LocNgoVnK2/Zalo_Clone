import React, { useEffect, useRef, useState } from "react";
import { Button } from "react-bootstrap";
function MessageContent(props) {
  const [sendTime, setSendTime] = useState();
  const [isFinishLoading, setIsFinishLoading] = useState();
  const [content, setContent] = useState();
  const initMessageInformation = () => {
    let time = new Date(props.sendTime);
    let hours = time.getHours();
    let minutes = time.getMinutes();
    minutes = minutes < 10 ? "0" + minutes : minutes;
    setSendTime(hours + ":" + minutes);
    setContent(props.content);
  };
  useEffect(() => {
    initMessageInformation();
    setIsFinishLoading(true);
  }, []);
  const render = () => {
    if (!isFinishLoading) {
      return null;
    }
    return (
      <>
        <div className="content">{content}</div>
        <div className="send-time" >{sendTime}</div>
      </>
    );
  };
  return render();
}
export default MessageContent;
