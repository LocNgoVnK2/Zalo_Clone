import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import avatar from "./assets/test.png";
import UserAvatar from "./assets/friends.png";

import background from './assets/background-may-dep-cho-khai-giang.jpg';
import { CheckFriendRequesting, CheckIsFriend } from '../Services/friendService';
class UserInforSearchedDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {
            updateDialog: false,
            userName: this.props.user.userName,
            gender: this.props.user.gender,
            dateOfBirth: this.props.user.dateOfBirth,
            avatarImageToLoad: 'data:image/jpeg;base64,' + this.props.user.avatar,
            backgroundImageToload: 'data:image/jpeg;base64,' + this.props.user.background,
            statusRequest: false

        }
    }
    componentDidMount = async () => {
        let checkFriend = await CheckIsFriend(this.props.senderId, this.props.user.email);
        let checkRequesting = await CheckFriendRequesting(this.props.senderId,this.props.user.id);
        
        if (checkFriend.data === true|| checkRequesting.data ===true || this.props.senderId === this.props.user.id) {
            this.setState({ statusRequest: false });
        }
        else {
            this.setState({ statusRequest: true });
        }
    }

    handleAddFriend = () => {
        let res = this.props.handleAddFriend(this.props.senderId, this.props.user.email);
        if (res) {
            this.setState({ statusRequest: false });
        }
    }

    render() {
        const { show, handleClose } = this.props;

        return (
            <Modal show={show} onHide={handleClose} dialogClassName="custom-dialog">
                <Modal.Header closeButton >
                    <Modal.Title >Thông tin tài khoản</Modal.Title>

                </Modal.Header>
                <Modal.Body className="custom-dialog__body">
                    <div className="custom-dialog__profile-photo">
                        <div style={{ position: 'relative', marginBottom: '10px' }}>
                            <img
                                alt="User's cover"
                                crossOrigin="Anonymous"
                                style={{ cursor: 'pointer', width: ' 466.4px', height: '315.95px' }}
                                src={this.props.user.background ? this.state.backgroundImageToload : background}
                            />
                            <div className="avatar-container">
                                <img
                                    alt="User's avatar"
                                    className="a-child"
                                    src={this.props.user.avatar ? this.state.avatarImageToLoad : UserAvatar}
                                />
                            </div>

                        </div>
                        <div className="user-name-container">
                            <div>
                                <strong>{this.props.user.userName}</strong>
                            </div>
                        </div>
                    </div>
                    <div style={{ flex: '1 1 0%' }}>
                        <p className="custom-dialog__detail-header" >
                            Thông tin cá nhân
                        </p>
                        <div>
                            <div style={{ marginBottom: '22px' }}>
                                <div className="custom-dialog__details-line">
                                    <span >Điện thoại</span>
                                    <span style={{ position: 'relative' }}> : +{this.props.user.phoneNumber}</span>

                                </div>
                                <div className="custom-dialog__details-line">
                                    <span >Giới tính</span>
                                    <span >: {this.props.user.gender === 'nam' ? 'Nam' : 'Nữ'}</span>
                                </div>
                                <div className="custom-dialog__details-line">
                                    <span >Ngày sinh</span>
                                    <span style={{ position: 'relative' }}>: {this.props.user.dateOfBirth}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                        {
                            this.state.statusRequest && (
                                <Button variant="primary center" onClick={this.handleAddFriend}>
                                    Kết bạn
                                </Button>
                            )
                        }

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
}

export default UserInforSearchedDialog;