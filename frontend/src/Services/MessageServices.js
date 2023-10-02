import axios from "./customize-axios";
const GetMessagesFromContactOfUser = (userId, contactId, atIndex) => {

    return axios.get(`/api/Message/GetMessagesFromContactOfUser?userId=${userId}&contactId=${contactId}&atIndex=${atIndex}`);
}
const GetUnNotifiedMessagesFromContactOfUser = (userId, contactId,) => {

    return axios.get(`/api/Message/GetUnNotifiedMessagesFromContactOfUser?userId=${userId}&contactId=${contactId}`);
}
const PollingForNewMessage = (userId) => {

    return axios.get(`/api/Message/GetContactsOfUnNotifiedMessage?userId=${userId}`);
}
const SendMessageToContact = (userId,contactId,content,idMessageSrc,messageAttachments) => {
    const data = {
        sender : userId,
        contactId : contactId,
        idMessageSrc : idMessageSrc,
        messageAttachments : messageAttachments,
        content : content
    };
    return axios.post("/api/Message/SendMessageToContact",data);
}
export{GetMessagesFromContactOfUser,
PollingForNewMessage,
SendMessageToContact,
GetUnNotifiedMessagesFromContactOfUser};   
