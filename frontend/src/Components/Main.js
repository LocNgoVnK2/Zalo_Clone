import React, { useEffect, useState } from "react";
import Test from "./assets/test.png";
import Message from "./Message";
import Home, {HomeState} from "./Home";
import MessageView from "./MessageView";
function Main(props) {
  const [contactName, setContactName] = useState();
  const [contactInformation, setContactInformation] = useState();
  const [message, setMessage] = useState();
  const [isThereNewMessage, setIsThereNewMessage] = useState(false);
  const [isChatting, setIsChatting] = useState(false);
  const [chattingFrame, setChattingFrame] = useState();
  const [objectRender, setObjectRender] = useState();
  let state = props.state;

  useEffect(() => {
    if(state === HomeState.Message){
      setObjectRender(<MessageView userId={props.userId}/>);
    }

  }, [state]);
  return objectRender; 
}

export default Main;
