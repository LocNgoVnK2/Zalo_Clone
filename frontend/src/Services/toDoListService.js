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


export { createToDoList,GetAllTasksDoneByUserCreationAPI,GetAllTasksNotDoneByUserCreationAPI};