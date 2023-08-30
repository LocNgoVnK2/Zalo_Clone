import axios from "./customize-axios";

const createToDoList = (userSrc, content, endDate, title, userToDoTask) => {

    const data = {
        id: 0,
        userSrc: userSrc,
        content: content,
        endDate: endDate,
        title: title,
        userToDoTask: userToDoTask,
        remindCount: 0,
        isDone: false
    };

    return axios.post("/api/ToDoList/CreateToDoList", data);
};
const GetAllTasksDoneByUserCreationAPI = (userId) => {

    return axios.get(`/api/ToDoList/GetAllTasksDoneByUserCreation?userId=${userId}`);
}
const GetAllTasksNotDoneByUserCreationAPI = (userId) => {

    return axios.get(`/api/ToDoList/GetAllTasksNotDoneByUserCreation?userId=${userId}`);
}
const GetAllTasksDoneByUserDoAPI = (userId) => {

    return axios.get(`/api/ToDoList/GetAllTasksDoneByUserDo?userId=${userId}`);
}
const GetAllTasksNotDoneByUserDoAPI = (userId) => {

    return axios.get(`/api/ToDoList/GetAllTasksNotDoneByUserDo?userId=${userId}`);
}
const GetAllTasksAndUserNotCompleteOfUserDesAPI = (userId) => {

    return axios.get(`/api/ToDoList/GetAllTasksAndUserNotCompleteOfUserDes?userId=${userId}`);
}


const GetTaskByTaskIdAPI = (taskId) => {

    return axios.get(`/api/ToDoList/GetTaskByTaskId?taskId=${taskId}`);
}
const UpdateRemindCountAPI = (taskId) => {

    return axios.post(`/api/ToDoList/UpdateRemindCount?taskId=${taskId}`);
}
//​ api​/ToDoList​/UpdateToDoList
const UpdateToDoListAPI = (id, content, endDate, title, userToDoTask) => {

    const data = {
        id: id,
        content: content,
        endDate: endDate,
        title: title,
        userToDoTask: userToDoTask,
    };

    return axios.put("/api/ToDoList/UpdateToDoList", data);
};
export { createToDoList,
        GetAllTasksDoneByUserCreationAPI,
        GetAllTasksNotDoneByUserCreationAPI,
        GetAllTasksDoneByUserDoAPI,
        GetAllTasksNotDoneByUserDoAPI,
        GetAllTasksAndUserNotCompleteOfUserDesAPI,
        GetTaskByTaskIdAPI,
        UpdateRemindCountAPI,
        UpdateToDoListAPI};