import axios from "./customize-axios";
const GetMessagesFromContactOfUser = (userId, contactId) => {

    return axios.get(`/api/Message/GetMessagesFromContactOfUser?userId=${userId}&contactId=${contactId}`);
}

const PollingForNewMessage = (userId) => {

    return axios.get(`/api/Message/GetContactsOfUnNotifiedMessage?userId=${userId}`);
}
const SendMessageToContact = (userId,contactId,content,idMessageSrc,attachmentByBase64) => {
    const data = {
        sender : userId,
        contactId : contactId,
        idMessageSrc : idMessageSrc,
        attachmentByBase64 : attachmentByBase64,
        content : content
    };
    return axios.post("/api/Message/SendMessageToContact",data);
}
export{GetMessagesFromContactOfUser,
PollingForNewMessage,
SendMessageToContact};   
