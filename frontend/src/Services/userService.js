import axios from "./customize-axios";
const loginApi = (email, password) => {
    const data = {
      email: email,
      password: password
    };
  
    return axios.post("/api/User/signin", data);
  };
  const signupApi = (email, password,username,Phonenumber,gender,dateOfBirth) => {
    const data = {
      email: email,
      password: password,
      username: username,
      Phonenumber : Phonenumber,
      gender: gender,
      dateOfBirth: dateOfBirth
    };
  
    return axios.post("/api/User/signup", data);
  };
  const getuserApi = (email) => {
    return axios.get(`/api/User/GetUserInformation?email=${email}`);
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

  const RenewTokenResetPassword = (token) => {

    return axios.post(`/api/User/ReSendTokenResetPassword?token=${token}`);
  };
  const ValidateResetPassword = (token) => {

    return axios.post(`/api/User/ValidateResetPassword?token=${token}`);
  };
  const UpdatePasswordApi = (token,password) => {

    return axios.post(`/api/User/UpdatePassword?token=${token}&password=${password}`);
  };
  const GetUserContacts = (id) =>
  {
    return axios.get(`/api/User/GetContactsOfUser?userID=${id}`)
  }
  ///api/User/UpdateUserInformation
  const UpdateUserInformationApi = (id,email,username,gender,dateOfBirth,avatar,background) => {
    const data = {
      id:id,
      userName: username,
      email: email,
      gender: gender,
      dateOfBirth: dateOfBirth,
      avatar: avatar,
      background: background
    };
  
    return axios.post("api/User/UpdateUserInformation", data);
  };
export{
  loginApi,
  signupApi,
  getuserApi,
  getuserDataApi,
  ValidateSignUp,
  RenewToken,
  SendTokenForForgotPassword,
  RenewTokenResetPassword,
  ValidateResetPassword,
  UpdatePasswordApi,
  GetUserContacts,
  UpdateUserInformationApi};   


