import React, { useEffect, useRef, useState } from "react";
import { Button } from "react-bootstrap";
function MessageUser(props) {
  const [type, setType] = useState("div");
  const [isFinishLoading, setIsFinishLoading] = useState(false);
  const [properties, setProperties] = useState(null);
  const [additionalClassName, setAdditionalClassName] = useState("");
  useEffect(() => {
    if (props.type) {
      setType(props.type);
    }
    if (props.properties) setProperties(props.properties);
    if (props.additionalClassName) setAdditionalClassName(props.additionalClassName);
    setIsFinishLoading(true);
  }, []);
  const render = () => {
    if (!isFinishLoading) return null;
    return (
      <>
        <div className=" message-user">
          {React.createElement(
            type,
            { className: "message " + additionalClassName, ...properties },
            props.children
          )}
          {/* <div className="message">{props.children}</div> */}
        </div>
      </>
    );
  };
  return render();
}
export default MessageUser;
