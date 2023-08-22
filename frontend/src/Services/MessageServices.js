import axios from "./customize-axios";
const GetMessagesFromContactOfUser = (userId, contactId) => {

    return axios.get(`/api/Message/GetMessagesFromContactOfUser?userId=${userId}&contactId=${contactId}`);
}

const PollingForNewMessage = (userId) => {

    return axios.get(`/api/Message/GetContactsOfUnNotifiedMessage?userId=${userId}`);
}
export{GetMessagesFromContactOfUser,
PollingForNewMessage};   
