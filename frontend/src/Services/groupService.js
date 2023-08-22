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
export{CreateGroupChat,
    AddManyGroupUser,
    GetAllGroupChatsOfUserByUserIdAPI
    };   
