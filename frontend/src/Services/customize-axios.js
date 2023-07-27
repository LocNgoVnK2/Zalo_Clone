import axios from "axios";
const instance = axios.create({
    baseURL: 'https://localhost:5001',
});


instance.interceptors.response.use(function(response){
    return response.data;
},function(error){
    console.log(">> error :"+error.name+"|"+error.response);
    
    return Promise.reject(error);
});
export default instance;