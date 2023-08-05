import axios from "axios";
// import http from 'http';
// import https from 'https';

const instance = axios.create({
    baseURL: 'https://localhost:5001',
    timeout: 20000,
    // httpAgent: new http.Agent({ keepAlive: true }),
    // httpsAgent: new https.Agent({ keepAlive: true }),
});


instance.interceptors.response.use(function(response){
    return response;
    //return response.data;
},function(error){
    console.log(">> error :"+error.name+"|"+error.response);
    
    return Promise.reject(error);
});
export default instance;