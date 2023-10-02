import React, { useEffect, useRef, useState } from "react";
function MessageContact(props) {
  const [sender, setSender] = useState();
  const [type, setType] = useState("div");
  const [isFinishLoading, setIsFinishLoading] = useState(false);
  const [properties, setProperties] = useState(null);
  const [additionalClassName, setAdditionalClassName] = useState("");
  const initMessageInformation = () => {
    if (props.type) {
      setType(props.type);
    }
    if (props.properties) setProperties(props.properties);
    if (props.additionalClassName)
      setAdditionalClassName(props.additionalClassName);
    setSender(props.sender);
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
        <div className="message-contact">
          {React.createElement(
            type,
            { className: "message " + additionalClassName, ...properties },
            <>
              <div className="contact-name">{sender}</div>
              {props.children}
            </>
          )}
          {/* <div className="message">{props.children}</div> */}
        </div>
      </>
    );
  };
  return render();
}
export default MessageContact;
