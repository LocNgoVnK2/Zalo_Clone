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
  const  getuserApi =  (email) => {
    const response = axios.get(`/api/User/GetUserInformation?email=${email}`);
    return response;
  };


  const getuserDataApi = (id) => {
    return axios.get(`/api/UserRole?userId?a=${id}`);
  };
  const ValidateSignUp = (token) => {

    return axios.post(`/api/User/ValidateSignUp?token=${token}`);
  };
  const RenewToken = (token) => {

    return axios.post(`/api/User/ReSendToken?token=${token}`);
  };

  const SendTokenForForgotPassword = (email) => {

    return axios.post(`/api/User/ResetPassword?email=${email}`);
  };
  const GetUserContacts = (id) =>
  {
    return axios.get(`/api/User/GetContactsOfUser?userID=${id}`);
  }
 
  const RenewTokenResetPassword = (token) => {

    return axios.post(`/api/User/ReSendTokenResetPassword?token=${token}`);
  };
  const ValidateResetPassword = (token) => {

    return axios.post(`/api/User/ValidateResetPassword?token=${token}`);
  };
  const UpdatePasswordApi = (token,password) => {

    return axios.post(`/api/User/UpdatePassword?token=${token}&password=${password}`);
  };
  const  GetContactInformationById =  (id) => {
    const response = axios.get(`/api/User/GetContactInformationById?id=${id}`);
    return response;
  };
export{loginApi,signupApi,getuserApi,getuserDataApi,ValidateSignUp,RenewToken,SendTokenForForgotPassword,RenewTokenResetPassword,ValidateResetPassword,UpdatePasswordApi,
GetUserContacts,
GetContactInformationById};   
