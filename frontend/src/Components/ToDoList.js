import React, { useState, useEffect } from 'react';
//image
import UserAvatar from "./assets/friends.png";
import GroupAvatar from "./assets/community-3245739_640.png";
import FriendIcon from "./assets/icon/icons8-group-50.png";
import GroupIcon from "./assets/icon/icons8-group-64.png";
import LetterIcon from "./assets/icon/icons8-letter-64.png";
import SearchIcon from "./assets/icon/searchIcon.png";
import { Modal, Button, Form, FormControl, InputGroup } from 'react-bootstrap';
import Swal from 'sweetalert2';
//api
import { GetListFriend } from '../Services/friendService';
import { createToDoList } from '../Services/toDoListService';
//react bootstrap
import { ListGroup, ListGroupItem, OverlayTrigger } from 'react-bootstrap';
import { Popover } from 'react-bootstrap';
import { Spinner } from "react-bootstrap";
import UserInforSearchedDialog from './UserInforSearchedDialog';
import { useCallback } from 'react';


function ToDoList(props) {
    const [selectedButton, setSelectedButton] = useState('friends');
    const [showModal, setShowModal] = useState(false);
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [selectedAssignees, setSelectedAssignees] = useState([]);
    const [deadline, setDeadline] = useState('');
    const [userList, setUserList] = useState([]);
    const [selectedStatusButton, setSelectedStatusButton] = useState('');
    const fetchData = useCallback(async () => {
        try {
            const [listFriendResponse] = await Promise.all([
                GetListFriend(props.userId)
            ]);
            setUserList(listFriendResponse.data)
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
        setSelectedButton(buttonName);
    }


    const handleCheckStatusButton = (buttonId) => {
        selectedStatusButton(buttonId);
    };

    const renderContent = () => {
        if (selectedButton === 'CreateToDo') {
            return (
                <>
                    <button className="option-buttons-active" onClick={() => handleCheckStatusButton('CreateToDo')}>
                        Tôi giao
                    </button>
                    <button className="option-buttons" onClick={() => handleCheckStatusButton('ReceiveToDo')}>
                        Tôi giao
                    </button>
                    <button className="option-buttons" onClick={() => handleCheckStatusButton('FollowToDo')}>
                        Tôi giao
                    </button>
                    {/* render content cho từng option status */}
                </>
            );
        } else if (selectedButton === 'ReceiveToDo') {
            return (
                <>
                    <button className="option-buttons" onClick={() => handleCheckStatusButton('CreateToDo')}>
                        Tôi giao
                    </button>
                    <button className="option-buttons-active" onClick={() => handleCheckStatusButton('ReceiveToDo')}>
                        Tôi giao
                    </button>
                    <button className="option-buttons" onClick={() => handleCheckStatusButton('FollowToDo')}>
                        Tôi giao
                    </button>
                </>
            );
        } else if (selectedButton === 'FollowToDo') {
            return (
                <>
                    <button className="option-buttons" onClick={() => handleCheckStatusButton('CreateToDo')}>
                        Tôi giao
                    </button>
                    <button className="option-buttons" onClick={() => handleCheckStatusButton('ReceiveToDo')}>
                        Tôi giao
                    </button>
                    <button className="option-buttons-active" onClick={() => handleCheckStatusButton('FollowToDo')}>
                        Tôi giao
                    </button>
                </>
            );
        }
    };


    return (
        <>
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
                                                    width="32px" height="32px" alt="" className="rounded-circle user-avatar border" />
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
                <button style={{ fontSize: "24px", padding: "5px 20px" }} onClick={callModal}>
                    Giao công việc mới
                </button>
            </div>
            <div className="button-container">
                <div className="top-buttons">
                    <button className={`vertical-button ${selectedButton === 'CreateToDo' ? 'selected' : ''}`} onClick={() => handleButtonClick('CreateToDo')}>
                        <img src={FriendIcon} width="32px" height="32px" alt="" /> Tôi giao
                    </button>

                    <button className={`vertical-button ${selectedButton === 'ReceiveToDo' ? 'selected' : ''}`} onClick={() => handleButtonClick('ReceiveToDo')}>
                        <img src={GroupIcon} width="32px" height="32px" alt="" /> Cần làm
                    </button>

                    <button className={`vertical-button ${selectedButton === 'FollowToDo' ? 'selected' : ''}`} onClick={() => handleButtonClick('FollowToDo')}>
                        <img src={LetterIcon} width="32px" height="32px" alt="" /> Theo dõi
                    </button>
                </div>

                <div className={`full-width-button`} >
                    <div className="under-buttons">
                        {renderContent()}
                        {/*render button theo từng lựa chọn*/}

                        {/*
                    <button className="option-buttons-active" >
                        Tôi giao
                    </button>
                    <button className="option-buttons" >
                        Tôi giao
                    </button>
                    <button className="option-buttons">
                        Tôi giao
                    </button>
                                    */}
                    </div>
                </div>
            </div>

            {/*renderContent()*/}


        </>
    );
}

export default ToDoList;