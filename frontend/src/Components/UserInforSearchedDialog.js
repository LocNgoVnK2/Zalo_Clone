import React, { useState, useEffect } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import UserAvatar from "./assets/friends.png";
import background from './assets/background-may-dep-cho-khai-giang.jpg';
import { CheckFriendRequesting, CheckIsFriend } from '../Services/friendService';

function UserInforSearchedDialog(props) {
    const [statusRequest, setStatusRequest] = useState(false);
    const [avatarImageToLoad, setAvatarImageToLoad] = useState('data:image/jpeg;base64,' + props.user.avatar);
    const [backgroundImageToload, setBackgroundImageToload] = useState('data:image/jpeg;base64,' + props.user.background);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const checkFriend = await CheckIsFriend(props.senderId, props.user.email);
                const checkRequesting = await CheckFriendRequesting(props.senderId, props.user.id);

                if (checkFriend.data === true || checkRequesting.data === true || props.senderId === props.user.id) {
                    setStatusRequest(false);
                } else {
                    setStatusRequest(true);
                }
            } catch (error) {
                console.error("Error fetching friend status:", error);
            }
        };

        fetchData();
    }, [props.senderId, props.user.email, props.user.id]);

    const handleAddFriend = () => {
        const res = props.handleAddFriend(props.senderId, props.user.email);
        if (res) {
            setStatusRequest(false);
        }
    };

    const { show, handleClose } = props;

    return (
        <Modal show={show} onHide={handleClose} dialogClassName="custom-dialog">
            <Modal.Header closeButton>
                <Modal.Title>Thông tin tài khoản</Modal.Title>
            </Modal.Header>
            <Modal.Body className="custom-dialog__body">
                <div className="custom-dialog__profile-photo">
                    <div style={{ position: 'relative', marginBottom: '10px' }}>
                        <img
                            alt="User's cover"
                            crossOrigin="Anonymous"
                            style={{ cursor: 'pointer', width: ' 466.4px', height: '315.95px' }}
                            src={props.user.background ? backgroundImageToload : background}
                        />
                        <div className="avatar-container">
                            <img
                                alt="User's avatar"
                                className="a-child"
                                src={props.user.avatar ? avatarImageToLoad : UserAvatar}
                            />
                        </div>
                    </div>
                    <div className="user-name-container">
                        <div>
                            <strong>{props.user.userName}</strong>
                        </div>
                    </div>
                </div>
                <div style={{ flex: '1 1 0%' }}>
                    <p className="custom-dialog__detail-header">
                        Thông tin cá nhân
                    </p>
                    <div>
                        <div style={{ marginBottom: '22px' }}>
                            <div className="custom-dialog__details-line">
                                <span>Điện thoại</span>
                                <span style={{ position: 'relative' }}> : +{props.user.phoneNumber}</span>
                            </div>
                            <div className="custom-dialog__details-line">
                                <span>Giới tính</span>
                                <span>: {props.user.gender === 'nam' ? 'Nam' : 'Nữ'}</span>
                            </div>
                            <div className="custom-dialog__details-line">
                                <span>Ngày sinh</span>
                                <span style={{ position: 'relative' }}>: {props.user.dateOfBirth}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                    {statusRequest && (
                        <Button variant="primary center" onClick={handleAddFriend}>
                            Kết bạn
                        </Button>
                    )}
                </div>
            </Modal.Body>
            <Modal.Footer className="custom-dialog__footer">
                <Button variant="secondary" onClick={handleClose}>
                    Đóng
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

export default UserInforSearchedDialog;