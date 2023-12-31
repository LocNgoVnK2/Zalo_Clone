import React, { useState, useEffect } from 'react';
//image
import UserAvatar from "./assets/friends.png";
import GroupAvatar from "./assets/community-3245739_640.png";
import FriendIcon from "./assets/icon/icons8-group-50.png";
import GroupIcon from "./assets/icon/icons8-group-64.png";
import LetterIcon from "./assets/icon/icons8-letter-64.png";
import ToDoListIcon from "./assets/icon/icons8-todo-list-48.png";
import ToReceivelist from "./assets/icon/icons8-edit-property-48.png";
import ToFollowlist from "./assets/icon/icons8-tasklist-48.png";
import TaskAvatar from "./assets/icon/icons8-goal-50.png";


import SearchIcon from "./assets/icon/searchIcon.png";
import { Modal, Button, Form, FormControl, InputGroup } from 'react-bootstrap';
import Swal from 'sweetalert2';
//api
import { GetListFriend } from '../Services/friendService';
import {
    createToDoList, GetAllTasksDoneByUserCreationAPI,
    GetAllTasksNotDoneByUserCreationAPI, GetAllTasksDoneByUserDoAPI,
    GetAllTasksNotDoneByUserDoAPI, GetAllTasksAndUserNotCompleteOfUserDesAPI
} from '../Services/toDoListService';
//react bootstrap
import { ListGroup } from 'react-bootstrap';
import { useCallback } from 'react';
import TaskDetailDialog from './TaskDetailDialog';
import TaskDetailForPartnerDialog from './TaskDetailForPartnerDialog';


function ToDoList(props) {
    const [selectedButton, setSelectedButton] = useState('friends');
    const [showModal, setShowModal] = useState(false);
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [selectedAssignees, setSelectedAssignees] = useState([]);
    const [deadline, setDeadline] = useState('');
    const [userList, setUserList] = useState([]);
    const [selectedStatusButton, setSelectedStatusButton] = useState('');
    const [taskDoneByUserCreatedList, setTaskDoneByUserCreatedList] = useState([]);
    const [taskNotDoneByUserCreatedList, setTaskNotDoneByUserCreatedList] = useState([]);
    const [tasksDoneByUserDoList, setTasksDoneByUserDoList] = useState([]);
    const [tasksNotDoneByUserDo, setTasksNotDoneByUserDo] = useState([]);
    const [tasksAndUserNotComplete, setTasksAndUserNotComplete] = useState([]);
    //to open task Detail view
    const [taskDetailShow, setTaskDetailShow] = useState(false);
    const [taskSelected, setTaskSelected] = useState('');
    const [taskDetailForPartnerShow, setTaskDetailForPartnerShow] = useState(false);
    const fetchData = useCallback(async () => {
        try {
            const [listFriendResponse,
                taskDoneByUserCreatedList,
                taskNotDoneByUserCreatedList,
                tasksDoneByUserDoListRes,
                tasksNotDoneByUserDoRes,
                tasksAndUserNotCompleteRes] = await Promise.all([
                    GetListFriend(props.userId),
                    GetAllTasksDoneByUserCreationAPI(props.userId),
                    GetAllTasksNotDoneByUserCreationAPI(props.userId),
                    GetAllTasksDoneByUserDoAPI(props.userId),
                    GetAllTasksNotDoneByUserDoAPI(props.userId),
                    GetAllTasksAndUserNotCompleteOfUserDesAPI(props.userId)
                ]);
            setUserList(listFriendResponse.data)
            setTaskDoneByUserCreatedList(taskDoneByUserCreatedList.data)
            setTaskNotDoneByUserCreatedList(taskNotDoneByUserCreatedList.data)
            setTasksDoneByUserDoList(tasksDoneByUserDoListRes.data)
            setTasksNotDoneByUserDo(tasksNotDoneByUserDoRes.data)
            setTasksAndUserNotComplete(tasksAndUserNotCompleteRes.data)
        } catch (error) {
            if (error.response) {
                alert(error.response.data.error);
            } else {
                alert("An error occurred");
            }
        }
    }, [props.userId]);

    useEffect(() => {
        fetchData();

    }, [fetchData]);




    const handleAssigneeChange = (e) => {
        const { value, checked } = e.target;
        if (checked) {
            setSelectedAssignees([...selectedAssignees, value]);
        } else {

            setSelectedAssignees(selectedAssignees.filter((assignee) => assignee !== value));
        }

    };
    const handleCreateToDoList = () => {
        createToDoList(props.userId, content, deadline, title, selectedAssignees).then((response) => {
            Swal.fire({
                icon: 'success',
                title: 'Tạo công việc thành công',
                showConfirmButton: false,
                timer: 1500
            });


            // Clear the state here
            fetchData();
            setContent('');
            setDeadline('');
            setTitle('');
            setSelectedAssignees([]);
        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Create fail',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }
    /* kiểm tra state change
        useEffect(() => {
            console.log(selectedAssignees);
          }, [selectedAssignees]);
    */



    const callModal = () => {
        if (showModal) {
            setTitle('');
            setContent('');
            setSelectedAssignees([]);
            setDeadline('');
        }
        setShowModal(!showModal);
    };
    const handleButtonClick = (buttonName) => {
        setSelectedStatusButton('');
        setSelectedButton(buttonName);
    }


    const handleCheckStatusButton = (buttonId) => {
        setSelectedStatusButton(buttonId);
    };



    // handle to open detail of task
    const HandleOpenDetailTask = (taskId) => {

        setTaskSelected(taskId);
        setTaskDetailShow(true);
    }

    const HandleCloseDetailTask = () => {
        setTaskSelected('');
        setTaskDetailShow(false);
    }
    const HandleOpenDetailForPartnerTask = (taskId) => {

        setTaskSelected(taskId);
        setTaskDetailForPartnerShow(true);
    }

    const HandleCloseDetailForPartnerTask = () => {
        setTaskSelected('');
        setTaskDetailForPartnerShow(false);
    }
    // render content for check status
    const renderMyTaskCreatedHasNotBeenCompleted = (MyTaskCreatedHasNotBeenCompletedList) => {
        return MyTaskCreatedHasNotBeenCompletedList.map((task) => (

            <ListGroup.Item
                key={task.id}
                className='user-Item'
                onClick={()=>HandleOpenDetailTask(task.id)}
            >
                <div className="row align-items-center">
                    <div className="col-2 d-flex justify-content-center">
                        <img
                            src={TaskAvatar}
                            className="rounded-circle user-avatar border size-Avatar"
                            alt=""
                        />
                    </div>
                    <div className="col user-details">
                        <span className="user-name">{task.title}</span>
                    </div>
                </div>
            </ListGroup.Item>
        ));
    };
    const renderMyTaskCreatedHasBeenCompleted = (MyTaskCreatedHasBeenCompletedList) => {
        return MyTaskCreatedHasBeenCompletedList.map((task) => (
            <ListGroup.Item
                key={task.id}
                className='user-Item'
                onClick={()=>HandleOpenDetailTask(task.id)}
            >
                <div className="row align-items-center">
                    <div className="col-2 d-flex justify-content-center">
                        <img
                            src={TaskAvatar}
                            className="rounded-circle user-avatar border size-Avatar"
                            alt=""
                        />
                    </div>
                    <div className="col user-details">
                        <span className="user-name">{task.title}</span>
                    </div>
                </div>
            </ListGroup.Item>
        ));
    };

    const renderMyTaskDoHasBeenCompleted = (MyTaskDoHasBeenCompletedList) => {
        return MyTaskDoHasBeenCompletedList.map((task) => (
            <ListGroup.Item
                key={task.id}
                className='user-Item'
                onClick={()=>HandleOpenDetailForPartnerTask(task.id)}
            >
                <div className="row align-items-center">
                    <div className="col-2 d-flex justify-content-center">
                        <img
                            src={TaskAvatar}
                            className="rounded-circle user-avatar border size-Avatar"
                            alt=""
                        />
                    </div>
                    <div className="col user-details">
                        <span className="user-name">{task.title}</span>
                    </div>
                </div>
            </ListGroup.Item>
        ));
    };
    const renderMyTaskDoHasNotBeenCompleted = (MyTaskDoHasNotBeenCompletedList) => {
        return MyTaskDoHasNotBeenCompletedList.map((task) => (
            <ListGroup.Item
                key={task.id}
                className='user-Item'
                onClick={()=>HandleOpenDetailForPartnerTask(task.id)}
            >
                <div className="row align-items-center">
                    <div className="col-2 d-flex justify-content-center">
                        <img
                            src={TaskAvatar}
                            className="rounded-circle user-avatar border size-Avatar"
                            alt=""
                        />
                    </div>
                    <div className="col user-details">
                        <span className="user-name">{task.title}</span>
                    </div>
                </div>
            </ListGroup.Item>
        ));
    };


    const renderUncompleteUser = (task) => {
        return task.listUserUncompleteThisTask?.map((member) => (

            <div
                key={member.id}
                className='user-Item'
            >
                <div className="row align-items-center">
                    <div className="col-2 d-flex justify-content-center">
                        <img
                            src={member.avatar ? 'data:image/jpeg;base64,' + member.avatar : UserAvatar}
                            className="rounded-circle user-avatar border size-Avatar"
                            alt=""
                        />
                    </div>
                    <div className="col user-details">
                        <span className="user-name">{member.contactName}</span>
                    </div>
                </div>
            </div>

        ));
    };





    const renderContent = () => {
        if (selectedButton === 'CreateToDo') {
            return (
                <>
                    <div className={`full-width-button`} >
                        <div className="under-buttons">
                            <button className={`option-buttons ${selectedStatusButton === 'MyTaskCreatedHasNotBeenCompleted' ? 'option-buttons-active' : ''}`} onClick={() => handleCheckStatusButton('MyTaskCreatedHasNotBeenCompleted')}>
                                Chưa xong  {'('} {taskNotDoneByUserCreatedList.length} {')'}
                            </button>
                            <button className={`option-buttons ${selectedStatusButton === 'MyCreationTaskIsCompleted' ? 'option-buttons-active' : ''}`} onClick={() => handleCheckStatusButton('MyCreationTaskIsCompleted')}>
                                Đã xong {'('} {taskDoneByUserCreatedList.length} {')'}
                            </button>
                        </div>
                    </div>
                    {selectedStatusButton === 'MyTaskCreatedHasNotBeenCompleted' ? (
                        <div className='Site-render-Items'>
                            <ListGroup className="user-list">
                                {renderMyTaskCreatedHasNotBeenCompleted(taskNotDoneByUserCreatedList)}

                            </ListGroup>
                        </div>
                    ) : null}
                    {selectedStatusButton === 'MyCreationTaskIsCompleted' ? (
                        <div className='Site-render-Items'>
                            <ListGroup className="user-list">
                                {renderMyTaskCreatedHasBeenCompleted(taskDoneByUserCreatedList)}
                            </ListGroup>
                        </div>
                    ) : null}
                </>
            );
        } else if (selectedButton === 'ReceiveToDo') {
            return (
                <>
                    <div className={`full-width-button`} >
                        <div className="under-buttons">
                            <button className={`option-buttons ${selectedStatusButton === 'ReceiveToDoMyTaskHasNotBeenCompleted' ? 'option-buttons-active' : ''}`} onClick={() => handleCheckStatusButton('ReceiveToDoMyTaskHasNotBeenCompleted')}>
                                Chưa xong {'('} {tasksNotDoneByUserDo.length} {')'}
                            </button>
                            <button className={`option-buttons ${selectedStatusButton === 'ReceiveToDoMyTaskIsCompleted' ? 'option-buttons-active' : ''}`} onClick={() => handleCheckStatusButton('ReceiveToDoMyTaskIsCompleted')}>
                                Đã xong {'('} {tasksDoneByUserDoList.length} {')'}
                            </button>
                        </div>
                    </div>
                    {selectedStatusButton === 'ReceiveToDoMyTaskHasNotBeenCompleted' ? (
                        <div className='Site-render-Items'>
                            <ListGroup className="user-list">
                                {renderMyTaskDoHasNotBeenCompleted(tasksNotDoneByUserDo)}

                            </ListGroup>
                        </div>
                    ) : null}
                    {selectedStatusButton === 'ReceiveToDoMyTaskIsCompleted' ? (
                        <div className='Site-render-Items'>
                            <ListGroup className="user-list">
                                {renderMyTaskDoHasBeenCompleted(tasksDoneByUserDoList)}
                            </ListGroup>
                        </div>
                    ) : null}
                </>
            );
        } else if (selectedButton === 'FollowToDo') {
            return (
                <>
                    <div className={`full-width-button`} >
                        <div className="under-buttons">
                            <button className="option-buttons-active" onClick={() => handleCheckStatusButton('CreateToDo')}>
                                Chưa xong
                            </button>

                        </div>
                    </div>
                    <div className='Site-render-to-following-process'>
                        <div className="user-list">

                            <div>
                                {tasksAndUserNotComplete && tasksAndUserNotComplete?.map((task) => (
                                    <div key={task.idTask} className="task-item">
                                        <h3 className="task-title">Danh sách các thành viên chưa hoàn thành của Công việc: {task.title}</h3>
                                        <div className="task-details">
                                            <span className="task-deadline">Hạn chót: {task.endDate}</span>
                                            <span className="task-uncompleted">
                                                Số lượng thành viên chưa hoàn thành: {task.listUserUncompleteThisTask.length}
                                            </span>
                                        </div>
                                        <ul className="member-list">
                                            {renderUncompleteUser(task)}
                                        </ul>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </>
            );
        }
    };


    return (
        <>
            {
                taskDetailShow && (
                    < TaskDetailDialog idTask={taskSelected}
                        showModal={taskDetailShow}
                        handleClose={HandleCloseDetailTask}
                        fetchData ={fetchData}
                    />
                )
            }
             {
                taskDetailForPartnerShow && (
                    < TaskDetailForPartnerDialog idTask={taskSelected}
                        showModal={taskDetailForPartnerShow}
                        handleClose={HandleCloseDetailForPartnerTask}
                        fetchData ={fetchData}
                        userId={props.userId}
                    />
                )
            }
            <Modal show={showModal} onHide={callModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Giao công việc mới</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group controlId="title">
                            <Form.Label>Tiêu đề</Form.Label>
                            <FormControl
                                type="text"
                                value={title}
                                onChange={(e) => setTitle(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group controlId="content">
                            <Form.Label>Nội dung</Form.Label>
                            <FormControl
                                as="textarea"
                                value={content}
                                onChange={(e) => setContent(e.target.value)}
                            />
                        </Form.Group>


                        <Form.Group controlId="assignee">
                            <Form.Label>Giao cho</Form.Label>
                            <div className="select-container">
                                {userList.map((user) => (
                                    <Form.Check
                                        key={user.id}
                                        type="checkbox"
                                        id={user.id}
                                        label={
                                            <div>
                                                <img src={user.avatar ? 'data:image;base64,' + user.avatar : UserAvatar}
                                                    alt="" className="rounded-circle user-avatar border size-mini-Avatar" />
                                                <span>{user.userName}</span>
                                            </div>
                                        }
                                        value={user.id}
                                        checked={selectedAssignees.includes(user.id)}
                                        onChange={handleAssigneeChange}
                                    />
                                ))}

                            </div>
                        </Form.Group>
                        <Form.Group controlId="deadline">
                            <Form.Label>Thời gian hoàn thành</Form.Label>
                            <InputGroup>
                                <FormControl
                                    type="date"
                                    value={deadline}
                                    onChange={(e) => setDeadline(e.target.value)}
                                />
                            </InputGroup>
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={callModal}>
                        Đóng
                    </Button>
                    <Button variant="primary" onClick={handleCreateToDoList}>
                        Tạo
                    </Button>
                </Modal.Footer>
            </Modal>
            <div className={`to-do-header-container`} >
                <button className='custom-new-task-button' onClick={callModal}>
                    Giao công việc mới
                </button>
            </div>
            <div className="button-container">
                <div className="top-buttons">
                    <button className={`vertical-button ${selectedButton === 'CreateToDo' ? 'selected' : ''}`} onClick={() => handleButtonClick('CreateToDo')}>
                        <img src={ToDoListIcon} className='size-mini-Avatar' alt="" /> Tôi giao {'('} {taskDoneByUserCreatedList.length + taskNotDoneByUserCreatedList.length} {')'}
                    </button>

                    <button className={`vertical-button ${selectedButton === 'ReceiveToDo' ? 'selected' : ''}`} onClick={() => handleButtonClick('ReceiveToDo')}>
                        <img src={ToReceivelist} className='size-mini-Avatar' alt="" /> Cần làm {'('} {tasksDoneByUserDoList.length + tasksNotDoneByUserDo.length} {')'}
                    </button>

                    <button className={`vertical-button ${selectedButton === 'FollowToDo' ? 'selected' : ''}`} onClick={() => handleButtonClick('FollowToDo')}>
                        <img src={ToFollowlist} className='size-mini-Avatar' alt="" /> Theo dõi
                    </button>
                </div>


                {renderContent()}

            </div>

        </>
    );
}


export default ToDoList;