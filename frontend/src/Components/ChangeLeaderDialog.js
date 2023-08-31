import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, FormControl, InputGroup } from 'react-bootstrap';
import UserAvatar from "./assets/friends.png";
import { GetAllUsersInGroupAPI, UpdateChatLeaderAPI } from '../Services/groupService';
import Swal from 'sweetalert2';
function ChangeLeaderDialog(props) {


    const [userListInGroup, setUserListInGroup] = useState([]);
    const [selectedAssignee, setSelectedAssignee] = useState('');
    const handleAssigneeChange = (e) => {
        const assigneeId = e.target.value;
        setSelectedAssignee(assigneeId);
    };


    useEffect(() => {
        const fetchData = () => {
            GetAllUsersInGroupAPI(props.group.idGroup)
                .then((usersRes) => {
                    setUserListInGroup(usersRes.data);
                })
                .catch((error) => {
                    Swal.fire({
                        icon: 'error',
                        title: error.response.data.error,
                        showConfirmButton: false,
                        timer: 1500
                    });
                });
        };

        fetchData();
    }, []);
    const handleChangeLeader = () => {
        UpdateChatLeaderAPI(props.group.idGroup, selectedAssignee).then((res) => {
            if (res) {
                Swal.fire({
                    icon: 'success',
                    title: "Chuyển nhóm trưởng thành công",
                    showConfirmButton: false,
                    timer: 1500
                });
                props.onClose();
                props.fetchDataInPhoneBook();
            }

        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: error.response.data.error,
                showConfirmButton: false,
                timer: 1500
            });
        })
    }

    return (
        <Modal show={props.showModal} onHide={props.onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Đổi nhóm trưởng của nhóm : {props.group.name}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>

                    <Form.Group controlId="assignee">

                        <Form.Label>Chuyển vị trí nhóm trưởng cho :</Form.Label>
                        <div className="select-container">
                            <div className='Site-render-UserGroups'>
                                {userListInGroup.map((user) => {
                                    if (user.id === props.userId) {
                                        return null;
                                    }
                                    return (
                                        <Form.Check
                                            key={user.id}
                                            type="radio"
                                            id={user.id}
                                            label={
                                                <div>
                                                    <img
                                                        src={
                                                            user.avatar
                                                                ? "data:image;base64," + user.avatar
                                                                : UserAvatar
                                                        }
                                                        alt=""
                                                        className="rounded-circle user-avatar border size-mini-Avatar"
                                                    />
                                                    <span>{user.contactName}</span>
                                                </div>
                                            }
                                            value={user.id}
                                            checked={selectedAssignee === user.id}
                                            onChange={handleAssigneeChange}
                                        />
                                    );
                                })}
                            </div>
                        </div>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={props.onClose}>
                    Đóng
                </Button>
                <Button variant="primary" onClick={handleChangeLeader}>
                    Tạo
                </Button>
            </Modal.Footer>
        </Modal>
    );

}
export default ChangeLeaderDialog;