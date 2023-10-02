import React, { useState } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import avatar from "./assets/test.png";
import UserAvatar from "./assets/friends.png";
import background from './assets/background-may-dep-cho-khai-giang.jpg';
import { UpdateUserInformationApi } from '../Services/userService';
import Swal from 'sweetalert2';

function UserProfileDialog(props) {
    const [updateDialog, setUpdateDialog] = useState(false);
    const [userName, setUserName] = useState(props.user.userName);
    const [gender, setGender] = useState(props.user.gender);
    const [dateOfBirth, setDateOfBirth] = useState(props.user.dateOfBirth);
    const [avatarImageToLoad, setAvatarImageToLoad] = useState('data:image/jpeg;base64,' + props.user.avatar);
    const [backgroundImageToload, setBackgroundImageToload] = useState('data:image/jpeg;base64,' + props.user.background);
    const [avatarImage, setAvatarImage] = useState('');
    const [backgroundImage, setBackgroundImage] = useState('');

    const handleAvatarChange = (event) => {
        const avatarImage = event.target.files[0];
        if (avatarImage && avatarImage.type.startsWith('image/')) {
            getBase64(avatarImage, (result) => {
                setAvatarImage(result);
            });
        }
    };

    const handleBackgroundChange = (event) => {
        const backgroundImage = event.target.files[0];
        if (backgroundImage && backgroundImage.type.startsWith('image/')) {
            getBase64(backgroundImage, (result) => {
                setBackgroundImage(result);
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
    };

    const handleOpenDialog = () => {
        setUpdateDialog(true);
    };

    const handleClose = () => {
        const { handleClose } = props;
        if (handleClose) {
            handleClose();
        }
        setUpdateDialog(false);
        setAvatarImage('');
        setBackgroundImage('');
        //window.location.reload();
    };

    const handleUserNameChange = (event) => {
        setUserName(event.target.value);
    };

    const handleGenderChange = (event) => {
        setGender(event.target.value);
    };

    const handleDateOfBirthChange = (event) => {
        setDateOfBirth(event.target.value);
    };

    const handleSubmit = async () => {
        var base64StringAvatar = null;
        var base64StringBackGround = null;

        if (avatarImage) {
            var avatar = avatarImage;
            base64StringAvatar = avatar.split(',')[1];
        }
        if (backgroundImage) {
            var background = backgroundImage;
            base64StringBackGround = background.split(',')[1];
        }

        try {
            console.log(base64StringAvatar);
            await UpdateUserInformationApi(
                props.user.id,
                props.user.email,
                userName,
                gender,
                dateOfBirth,
                base64StringAvatar,
                base64StringBackGround
            ).then(() => {
                // You don't need to call render() in functional components
            });
        } catch (error) {
            if (error.response) {
                Swal.fire({
                    icon: 'warning',
                    title: error.response.data.error,
                    showConfirmButton: false,
                    timer: 1500
                });
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Có lỗi xảy ra',
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        }
    };

    if (updateDialog) {
        return (
            <Modal show={updateDialog} onHide={handleClose} dialogClassName="custom-dialog">
                <Modal.Header closeButton>
                    <Modal.Title>Cập nhật thông tin tài khoản</Modal.Title>
                </Modal.Header>
                <Modal.Body className="custom-dialog__body">
                    <form onSubmit={handleSubmit}>
                        <div className="custom-dialog__profile-photo" style={{ height: '450px' }}>
                            <div style={{ position: 'relative', marginBottom: '10px' }}>
                                {/* Avatar Image */}
                                <label htmlFor="brInput">
                                    <img
                                        alt="User's cover"
                                        crossOrigin="Anonymous"
                                        style={{ cursor: 'pointer', width: '466.4px', height: '310.95px', zIndex: 2 }}
                                        src={backgroundImage ? backgroundImage : props.user.background ? backgroundImageToload : background}
                                    />
                                </label>
                                <div className="avatar-container">
                                    <label htmlFor="avatarInput">
                                        <img
                                            alt="User's avatar"
                                            className="a-child"
                                            src={avatarImage ? avatarImage : props.user.avatar ? avatarImageToLoad : avatar}
                                        />
                                    </label>
                                </div>
                                <input
                                    id="avatarInput"
                                    type="file"
                                    accept="image/*"
                                    style={{ display: 'none', zIndex: 1 }}
                                    onChange={handleAvatarChange}
                                />
                                {/* Background Image */}
                                <input
                                    id="brInput"
                                    type="file"
                                    accept="image/*"
                                    style={{ display: 'none' }}
                                    onChange={handleBackgroundChange}
                                />
                            </div>

                            <div className="custom-dialog__user-name-container" style={{ marginTop: '65px' }}>
                                <div className="user-name-label">
                                    <span className="user-name-label__text">
                                        Tên hiển thị
                                    </span>
                                </div>
                                <span className="user-name-input">
                                    <input
                                        tabIndex="1"
                                        placeholder="Nhập tên hiển thị"
                                        className="user-name-input__field"
                                        value={userName}
                                        onChange={handleUserNameChange}
                                    />
                                </span>
                                <div className="display-name">
                                    <div className="display-name__hint">
                                        <div>
                                            <span>
                                                Sử dụng tên thật để bạn bè dễ dàng nhận diện hơn
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div>
                                <p className="custom-dialog__detail-header">
                                    Thông tin cá nhân
                                </p>
                                <div style={{ marginBottom: '22px' }}>
                                    <div className="custom-dialog__details-line">
                                        <label>
                                            Giới tính :
                                        </label>
                                        <div>
                                            <label>
                                                <input type="radio" value="nam" checked={gender === "nam"} onChange={handleGenderChange} />
                                                Nam
                                            </label>
                                        </div>
                                        <div>
                                            <label>
                                                <input type="radio" value="nu" checked={gender === "nu"} onChange={handleGenderChange} />
                                                Nữ
                                            </label>
                                        </div>
                                    </div>
                                    <div className="custom-dialog__details-line">
                                        <label>
                                            Ngày sinh :
                                            <input type="datetime-local" value={dateOfBirth} onChange={handleDateOfBirthChange} />
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div className="custom-dialog__footer">
                                <Button type='submit' variant="primary center" value="Update">
                                    Cập nhật thông tin
                                </Button>
                            </div>
                        </div>
                    </form>
                </Modal.Body>
                <Modal.Footer className="custom-dialog__footer">
                    <Button variant="secondary" onClick={handleClose}>
                        Đóng
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    } else {
        return (
            <Modal show={props.show} onHide={props.handleClose} dialogClassName="custom-dialog">
                <Modal.Header closeButton>
                    <Modal.Title>Thông tin tài khoản</Modal.Title>
                </Modal.Header>
                <Modal.Body className="custom-dialog__body">
                    <div className="custom-dialog__profile-photo">
                        <div style={{ position: 'relative', marginBottom: '10px' }}>
                            <img
                                alt="User's cover"
                                crossOrigin="Anonymous"
                                style={{ cursor: 'pointer', width: ' 466.4px', height: '310.95px' }}
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
                        <Button variant="primary center" onClick={handleOpenDialog}>
                            Cập nhật thông tin
                        </Button>
                    </div>
                </Modal.Body>
                <Modal.Footer className="custom-dialog__footer">
                    <Button variant="secondary" onClick={props.handleClose}>
                        Đóng
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

export default UserProfileDialog;
