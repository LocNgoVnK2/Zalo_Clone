import React, { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { ValidateSignUp } from "../Services/userService";
import { useRef } from "react";
const Validation = () => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const token = queryParams.get("token");
  const navigate = useNavigate();

  const isApiCalledRef = useRef(false);

  useEffect(() => {
    if (token && !isApiCalledRef.current) {
      isApiCalledRef.current = true;

      const validateToken = async () => {
        try {
          await ValidateSignUp(token);
          navigate("/");
        } catch (error) {
          console.error("Validation Error:", error);
        }
      };

      validateToken();
    }
  }, [token, navigate]);

  return <div></div>;
};

export default Validation;
