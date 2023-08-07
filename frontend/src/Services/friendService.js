import axios from "./customize-axios";
const SendFriendRequest = (userSenderId,userReceiverId ) => {

    return axios.post(`/api/FriendRequest/SendFriendRequest?userSenderId=${userSenderId}&userReceiverId=${userReceiverId}`);
}
const CheckIsFriend = ( id, emailSearched) =>{
    return axios.get(`/api/Friend/CheckIsFriend?id=${id}&emailSearched=${emailSearched}`);
}
const RecommandFriend = ( id) =>{
    return axios.get(`/api/Friend/RecommandFriend?id=${id}`);
}
const CheckFriendRequesting =  (userSrc,userDes)=>{
    return axios.get(`/api/FriendRequest/CheckRequestingAddFriend?userSrc=${userSrc}&userDes=${userDes}`);
}
export{SendFriendRequest,
    CheckIsFriend,
    RecommandFriend,
    CheckFriendRequesting};   
