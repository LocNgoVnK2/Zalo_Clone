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
<<<<<<< HEAD
  const RenewTokenResetPassword = (token) => {

    return axios.post(`/api/User/ReSendTokenResetPassword?token=${token}`);
  };
  const ValidateResetPassword = (token) => {

    return axios.post(`/api/User/ValidateResetPassword?token=${token}`);
  };
  const UpdatePasswordApi = (token,password) => {

    return axios.post(`/api/User/UpdatePassword?token=${token}&password=${password}`);
  };
  
export{loginApi,signupApi,getuserApi,getuserDataApi,ValidateSignUp,RenewToken,SendTokenForForgotPassword,RenewTokenResetPassword,ValidateResetPassword,UpdatePasswordApi};   
=======
  const GetUserContacts = (id) =>
  {
    return axios.get(`/api/User/GetContactsOfUser?userID=${id}`)
  }

export{loginApi,
  signupApi,
  getuserApi,
  getuserDataApi,
  ValidateSignUp,
  RenewToken,
  SendTokenForForgotPassword,
  GetUserContacts
};   
>>>>>>> 08d86bfed6a37a9de387120e536c05346b4cbe0a
