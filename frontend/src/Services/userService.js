import axios from "./customize-axios";
const loginApi = (email, password) => {
    const data = {
      email: email,
      password: password
    };
  
    return axios.post("/api/User/signin", data);
  };
  const signupApi = (email, password,username,Phonenumber,gender) => {
    const data = {
      email: email,
      password: password,
      username: username,
      Phonenumber : Phonenumber,
      gender: gender
    };
  
    return axios.post("/api/User/signup", data);
  };
  const getuserApi = (email) => {
    return axios.get(`/api/User/GetUserByEmail?email=${email}`);
  };
  const sendMail = (email) => {

    return axios.post('/api/User/SendEmail', [email]);
  };

  const enterValidationCodeApi = (email, validationCode) => {
    const data = {
      email: email,
      validationCode: validationCode
    };
  
    return axios.post("​/api​/User​/EnterValidationCode", data);
  };

  const getuserDataApi = (id) => {
    return axios.get(`/api/UserRole?userId?a=${id}`);
  };
export{loginApi,signupApi,getuserApi,enterValidationCodeApi,sendMail,getuserDataApi};   