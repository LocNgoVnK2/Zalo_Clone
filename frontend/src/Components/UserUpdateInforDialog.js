import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import avatar from 'https://static.wikia.nocookie.net/jujutsu-kaisen/images/e/ef/Satoru_Gojo_%28Anime_2%29.png/revision/latest?cb=20211213114254&path-prefix=vi';
import background from 'https://toigingiuvedep.vn/wp-content/uploads/2021/02/background-may-dep-cho-khai-giang.jpg';
class UserUpdateInforDialog extends Component {
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
                            <div
                                style={{
                                    position: 'absolute',
                                    bottom: '0',
                                    left: '50%',
                                    transform: 'translateX(-50%)',
                                    borderRadius: '50%',
                                    border: '2px solid white',
                                    overflow: 'hidden',
                                    width: '80px',
                                    height: '80px',
                                    marginTop: '-40px',
                                }}
                            >
                                <img
                                    alt="User's avatar"
                                    className="a-child"
                                    style={{ width: '100%', height: 'auto' }}
                                    src={avatar}
                                />
                            </div>
                        </div>
                        <div style={{ width: '100%' }}>
                            <div style={{ textAlign: 'center' }}>
                                <div>
                                    <strong>{props.user.userName}</strong>
                                </div>
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

export default UserUpdateInforDialog;