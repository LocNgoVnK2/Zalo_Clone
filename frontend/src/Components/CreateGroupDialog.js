import React, { useState, useEffect } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import AddUserIcon from "./assets/icon/addUserIcon.png";
import CameraIcon from "./assets/icon/photo-camera-interface-symbol-for-button.png";
import SearchIcon from "./assets/icon/searchIcon.png";
import { ListGroup, ListGroupItem } from "react-bootstrap";
import Test from "./assets/test.png";
import { GetListFriend } from '../Services/friendService';
import UserAvatar from "./assets/friends.png";
import { CreateGroupChat, AddManyGroupUser } from '../Services/groupService';
import Swal from 'sweetalert2';

function CreateGroupDialog(props) {
    const [isFinishLoading, setIsFinishLoading] = useState(false);
    const [listFriends, setListFriends] = useState([]);
    const [originalListFriends, setOriginalListFriends] = useState([]);
    const [listSelectedUser, setListSelectedUser] = useState([]);
    const [searchText, setSearchText] = useState('');
    const [avatarImage, setAvatarImage] = useState('');
    const [groupName, setGroupName] = useState('');

    useEffect(() => {
        const fetchData = async () => {
            let listFriendsRes = await GetListFriend(props.userId);
            if (listFriendsRes) {
                setOriginalListFriends(listFriendsRes.data);
                setListFriends(listFriendsRes.data);
                setIsFinishLoading(true);
            }
        };

        fetchData();
    }, [props.userId]);

    const handleCheckboxChange = (friend) => {
        setListSelectedUser(prevListSelectedUser => {
            return prevListSelectedUser.includes(friend)
                ? prevListSelectedUser.filter(f => f !== friend) // trả về 1 list khác friend
                : [...prevListSelectedUser, friend];
        });
    }

    const handleGroupNameChange = (event) => {
        setGroupName(event.target.value);
    }

    const handleCreateGroup = async () => {
        let base64StringAvatar = null;
        if (avatarImage) {
            const avatar = avatarImage;
            base64StringAvatar = avatar.split(',')[1];
        }

        try {
            let createGrRes = await CreateGroupChat(groupName, props.userId, base64StringAvatar);
            if (createGrRes) {
                const usersToAdd = listSelectedUser.map(user => ({
                    idUser: user.id,
                    idGroup: createGrRes.data,
                    idGroupRole: 0,
                    joinDate: new Date().toISOString()
                }));
                let addUserIntoGrRes = await AddManyGroupUser(usersToAdd);
                if (addUserIntoGrRes) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Tạo nhóm thành công',
                        showConfirmButton: true,
                        timer: 1500
                    }).then(() => {
                        window.location.reload();
                    });
                }
            }
        } catch (err) {
            console.log(err);
        }
    }

    const handleSearchChange = (event) => {
        const searchText = event.target.value;
        const filteredFriends = searchText
            ? listFriends.filter(friend =>
                friend.userName.toLowerCase().includes(searchText.toLowerCase())
            )
            : originalListFriends;

        setSearchText(searchText);
        setListFriends(filteredFriends);
    }

    const handleAvatarChange = (event) => {
        const avatarImage = event.target.files[0];
        if (avatarImage && avatarImage.type.startsWith('image/')) {
            getBase64(avatarImage, (result) => {
                setAvatarImage(result);
            });
        }
    };

    const getBase64 = (file, cb) => {
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            cb(reader.result);
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    }

    const rows = listFriends.map(friend => {
        const avatarImageToLoad = 'data:image/jpeg;base64,' + friend.avatar;
        return (
            <ListGroupItem
                key={friend.id}
                action
            >
                <div className="float-start">
                    <input
                        type="checkbox"
                        style={{ marginRight: '10px' }}
                        checked={listSelectedUser.includes(friend)}// if in list selected have already this user returned true checkbox will be checked
                        onChange={() => handleCheckboxChange(friend)}
                    />
                    <img
                        src={friend.avatar ? avatarImageToLoad : UserAvatar}
                        className="rounded-circle"
                        width="48 px"
                        height="48 px"
                        alt=""
                    />
                </div>
                <div className="float-start ms-3">
                    <span className="float-right">{friend.userName}</span>
                </div>
            </ListGroupItem>
        );
    });

    const usersSelected = listSelectedUser.map(selectedUser => {
        const avatarImageToLoad = 'data:image/jpeg;base64,' + selectedUser.avatar;
        return (
            <ListGroupItem
                key={selectedUser.id}
                action
            >
                <div className="float-start">
                    <img
                        src={selectedUser.avatar ? avatarImageToLoad : UserAvatar}
                        className="rounded-circle"
                        width="48 px"
                        height="48 px"
                        alt=""
                    />
                </div>
                <div className="float-start ms-5">
                    <span className="float-right">{selectedUser.userName}</span>
                </div>
            </ListGroupItem>
        );
    });

    return (
        <Modal show={props.show} onHide={props.handleClose} dialogClassName="custom-dialog">
            <Modal.Header closeButton>
                <Modal.Title>Tạo nhóm</Modal.Title>
            </Modal.Header>
            <Modal.Body className="custom-dialog__body">
                <form>
                    <div>
                        <div className="custom-dialog__user-name-container" style={{ marginTop: '20px' }}>
                            <div className="user-name-label">
                                <label htmlFor="avatarInput">
                                    {avatarImage ? (
                                        <img src={avatarImage} alt="Selected" width="35.6 px" height="38.2 px" style={{
                                            borderRadius: '50%',
                                            fontWeight: '400',
                                            cursor: 'pointer'
                                        }} />
                                    ) : (
                                        <span className="user-name-label__text" style={{
                                            border: '1px solid black',
                                            borderRadius: '50%',
                                            padding: '10px',
                                            paddingTop: '5px',
                                            fontWeight: '400',
                                            cursor: 'pointer'
                                        }}>
                                            <img src={CameraIcon} alt="" width="14 px" height="14 px" />
                                        </span>
                                    )}
                                </label>
                                <input
                                    id="avatarInput"
                                    type="file"
                                    accept="image/*"
                                    style={{ display: 'none', zIndex: 1 }}
                                    onChange={handleAvatarChange}
                                />
                            </div>
                            <span className="user-name-input">
                                <input
                                    tabIndex="1"
                                    placeholder="Nhập tên nhóm"
                                    className="user-name-input__field"
                                    value={groupName}
                                    onChange={handleGroupNameChange}
                                />
                            </span>
                        </div>
                    </div>
                </form>
                <span className="user-name-label__text">Thêm thành viên vào nhóm</span>
                <div className="user-name-input">
                    <input
                        style={{ borderRadius: '25px' }}
                        tabIndex="1"
                        placeholder="Nhập tên người cần thêm"
                        className="user-name-input__field"
                        value={searchText}
                        onChange={handleSearchChange}
                    />
                    <div className="search-icon-wrapper">
                        <img src={SearchIcon} alt="" className="search-icon" />
                    </div>
                </div>
                <div style={{ display: 'flex', width: '100%' }}>
                    <span className="user-name-label__text" style={{ flex: '65%', height: '30px', overflowY: 'auto' }}>
                        Danh sách bạn bè:
                    </span>
                    <span className="user-name-label__text" style={{ flex: '30%', height: '30px', overflowY: 'auto' }}>
                        Đã chọn:
                    </span>
                </div>
                <div style={{ display: 'flex', width: '100%' }}>
                    <div style={{ flex: '65%', height: '500px', overflowY: 'auto' }}>
                        <ListGroup variant="pills">{rows}</ListGroup>
                    </div>
                    <div style={{ flex: '30%', height: '500px', overflowY: 'auto', border: '1px solid black', marginTop: '20px', marginBottom: '20px' }}>
                        <ListGroup variant="pills">{usersSelected}</ListGroup>
                    </div>
                </div>
            </Modal.Body>
            <Modal.Footer className="custom-dialog__footer">
                <Button variant="secondary" onClick={props.handleClose}>
                    Đóng
                </Button>
                <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                    <Button variant="primary center" onClick={handleCreateGroup}>
                        Tạo nhóm
                    </Button>
                </div>
            </Modal.Footer>
        </Modal>
    );
}

export default CreateGroupDialog;
