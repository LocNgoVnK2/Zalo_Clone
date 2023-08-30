import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, FormControl, InputGroup, Row, Col } from 'react-bootstrap';
import UserAvatar from "./assets/friends.png";
import moment from 'moment';
import Swal from 'sweetalert2';

import { UpdateToDoListAPI } from '../Services/toDoListService';

function UpdateToDoList(props) {
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [selectedAssignees, setSelectedAssignees] = useState([]);
    const [deadline, setDeadline] = useState('');

    const handleUpdateTask = () => {
        UpdateToDoListAPI(props.task.id, content, deadline, title, selectedAssignees).then((res) => {
            if (res) {
                props.handleClose();
                props.fetchDataDetails();
                props.fetchData();
                Swal.fire({
                    icon: 'success',
                    title: 'Cập nhật thành công',
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Cập nhật thất bại',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }

    // delay to load task 
    const handleAssigneeChange = (e) => {
        const { value, checked } = e.target;
        if (checked) {
            setSelectedAssignees([...selectedAssignees, value]);
        } else {

            setSelectedAssignees(selectedAssignees.filter((assignee) => assignee !== value));
        }

    };
    useEffect(() => {
        setTitle(props.task.title);
        setContent(props.task.content);
        
        const date = new Date(props.task.endDate);
        setDeadline(date.toISOString().slice(0, 10));
        
        console.log('Title:', title);
        console.log('Content:', content);
        console.log('Deadline:', deadline);
    }, []);
    return (

        <>


            <Modal show={props.showModal} onHide={props.handleClose}>
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
                            <Form.Label>Giao cho - Chọn để xóa thành viên</Form.Label>
                            <div className="select-container">

                                {props.task.partners.map((user) => (
                                    <Form.Check
                                        key={user.id}
                                        type="checkbox"
                                        id={user.id}
                                        label={
                                            <div>
                                                <img src={user.avatar ? 'data:image;base64,' + user.avatar : UserAvatar}
                                                    alt="" className="rounded-circle user-avatar border size-mini-Avatar" />
                                                <span>{user.contactName}</span>
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
                    <Button variant="secondary" onClick={props.handleClose}  >
                        Đóng
                    </Button>
                    <Button variant="primary" onClick={handleUpdateTask}>
                        Cập nhật
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    )




}
export default UpdateToDoList;