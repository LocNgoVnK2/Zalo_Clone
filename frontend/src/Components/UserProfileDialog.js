import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import avatar from "./assets/test.png";
import background from './assets/background-may-dep-cho-khai-giang.jpg';
class UserProfileDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {

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
                                style={{ cursor: 'pointer', width: '100%', height: 'auto' }}
                                src={background}
                            />
                            <div className="avatar-container">
                                <img
                                    alt="User's avatar"
                                    className="a-child"
                                    src={avatar}
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
                                    <span >: {this.props.user.gender}</span>
                                </div>
                                <div className="custom-dialog__details-line">
                                    <span >Ngày sinh</span>
                                    <span style={{ position: 'relative' }}>: {this.props.user.dateOfBirth}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                        <Button variant="primary center" onClick={handleClose}>
                            Cập nhật thông tin
                        </Button>
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

export default UserProfileDialog;