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
const AddSearchLog = (userSrc,userDes) =>{
    return axios.post(`/api/SearchLog/AddSearchLog?userSrc=${userSrc}&userDes=${userDes}`);
}
const RemoveSearchLog =  (userSrc,userDes) =>{
    return axios.post(`/api/SearchLog/RemoveSearchLog?userSrc=${userSrc}&userDes=${userDes}`);
}
const GetRecentSearch =(userSrc)=>{
    return axios.get(`/api/SearchLog/GetRecentSearch?userSrc=${userSrc}`);
}
export{SendFriendRequest,
    CheckIsFriend,
    RecommandFriend,
    CheckFriendRequesting,
    AddSearchLog,
    RemoveSearchLog,
    GetRecentSearch
    };   
