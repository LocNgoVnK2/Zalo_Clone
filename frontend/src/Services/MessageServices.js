import axios from "./customize-axios";
const GetMessagesFromContactOfUser = (userId, contactId) => {

    return axios.get(`/api/Message/GetMessagesFromContactOfUser?userId=${userId}&contactId=${contactId}`);
}
export{GetMessagesFromContactOfUser};   
