import axios from "./customize-axios";
const CreateGroupChat = (name,leaderId,image ) => {

    const data = {
        name: name,
        imageByBase64: image,
        leader: leaderId
      };
      
      return axios.post("/api/GroupChat/CreateGroupChat", data);
}
const AddManyGroupUser = (users) => {
    const data = users;
    
    return axios.post("/api/GroupUser/AddManyGroupUser", data);
}
//
const GetAllGroupChatsOfUserByUserIdAPI =  (userSrc) =>{
    return axios.get(`/api/GroupChat/GetAllGroupChatsOfUserByUserId?userId=${userSrc}`);
}
const RemoveGroupUserAPI=  (userSrc,idGroup) =>{
    return axios.post(`/api/GroupUser/RemoveGroupUser?idGroup=${idGroup}&&idUser=${userSrc}`);
}
//api/GroupUser/GetAllUsersInGroup
const GetAllUsersInGroupAPI =  (groupId) =>{
    return axios.get(`/api/GroupUser/GetAllUsersInGroup?groupId=${groupId}`);
}
const UpdateChatLeaderAPI = (idGroup,newLeaderId) =>{
    return axios.put(`/api/GroupChat/UpdateGroupChatLeader?id=${idGroup}&&newLeader=${newLeaderId}`);
}
export{CreateGroupChat,
    AddManyGroupUser,
    GetAllGroupChatsOfUserByUserIdAPI,
    RemoveGroupUserAPI,
    GetAllUsersInGroupAPI,
    UpdateChatLeaderAPI
    };   
