import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import avatar from "./assets/test.png";
import background from './assets/background-may-dep-cho-khai-giang.jpg';
import { UpdateUserInformationApi } from '../Services/userService';
class UserProfileDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {

            updateDialog: false,
            userName: this.props.user.userName,
            gender: this.props.user.gender,
            dateOfBirth: this.props.user.dateOfBirth,
            avatarImageToLoad:'data:image/jpeg;base64,'+ this.props.user.avatar,
            backgroundImageToload:'data:image/jpeg;base64,'+ this.props.user.background,
            avatarImage: '',
            backgroundImage: ''
        }
        this.handleUserNameChange = this.handleUserNameChange.bind(this);
        this.handleGenderChange = this.handleGenderChange.bind(this);
        this.handleDateOfBirthChange = this.handleDateOfBirthChange.bind(this);
        this.handleAvatarChange = this.handleAvatarChange.bind(this);
        this.handleBackgroundChange = this.handleBackgroundChange.bind(this);
    }
    handleAvatarChange = (event) => {
        const avatarImage = event.target.files[0];
        if (avatarImage && avatarImage.type.startsWith('image/')) {
            this.getBase64(avatarImage, (result) => {

                this.setState({ avatarImage: result });
            });
        }
    };

    handleBackgroundChange = (event) => {
        const backgroundImage = event.target.files[0];
        if (backgroundImage && backgroundImage.type.startsWith('image/')) {
            this.getBase64(backgroundImage, (result) => {

                this.setState({ backgroundImage: result });
            });
        }
    };

    getBase64(file, cb) {
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            cb(reader.result);
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    }
    handleOpenDialog = () => {
        this.setState({ updateDialog: true });
    };
    handleClose = () => {
        const { handleClose } = this.props;
        if (handleClose) {
            handleClose();
        }
        this.setState({ updateDialog: false, avatarImage: '',backgroundImage: ''});
    };

    handleUserNameChange = (event) => {
        this.setState({ userName: event.target.value });
    };

    handleGenderChange = (event) => {
        this.setState({ gender: event.target.value });
    };

    handleDateOfBirthChange = (event) => {
        this.setState({ dateOfBirth: event.target.value });
    };
    handleSubmit = async () => {
        var base64StringAvatar = null;
        var base64StringBackGround = null;
        if (this.state.avatarImage) {
            var avatar = this.state.avatarImage;
            base64StringAvatar = avatar.split(',')[1];
        }
        if (this.state.backgroundImage) {
            var background = this.state.backgroundImage;
            base64StringBackGround = background.split(',')[1];
        }
        try {
            let res = await UpdateUserInformationApi(this.props.user.id
                , this.props.user.email
                , this.state.userName
                , this.state.gender
                , this.state.dateOfBirth
                , base64StringAvatar
                , base64StringBackGround);
            if (res) {
                alert("Excute Success");
            }
        } catch (error) {
            if (error.response && error.response.status === 400) {
                alert(error.response.data.error);
            } else {
                alert("An error occurred");
            }
        }
    }
    
    render() {
        const { show, handleClose } = this.props;

        
        if (this.state.updateDialog) {
            return (<Modal show={this.state.updateDialog} onHide={this.handleClose} dialogClassName="custom-dialog">
                <Modal.Header closeButton >
                    <Modal.Title >Cập nhật thông tin tài khoản</Modal.Title>
                </Modal.Header>
                <Modal.Body className="custom-dialog__body">
                    <form onSubmit={this.handleSubmit}>
                        <div className="custom-dialog__profile-photo" style={{ height: '450px' }}>
                            <div style={{ position: 'relative', marginBottom: '10px' }}>
                                {/* Avatar Image */}
                                <label htmlFor="brInput">
                                    <img
                                        alt="User's cover"
                                        crossOrigin="Anonymous"
                                        style={{ cursor: 'pointer', width: '466.4px', height: '310.95px', zIndex: 2 }}
                                        src={this.state.backgroundImage ? this.state.backgroundImage : background}
                                    />
                                </label>
                                <div className="avatar-container">
                                    <label htmlFor="avatarInput">

                                        <img
                                            alt="User's avatar"
                                            className="a-child"
                                            src={this.state.avatarImage ? this.state.avatarImageToLoad : avatar }
                                        />

                                    </label>
                                </div>
                                <input
                                    id="avatarInput"
                                    type="file"
                                    accept="image/*"
                                    style={{ display: 'none', zIndex: 1 }}
                                    onChange={this.handleAvatarChange}
                                />
                                {/* Background Image */}
                                <input
                                    id="brInput"
                                    type="file"
                                    accept="image/*"
                                    style={{ display: 'none' }}
                                    onChange={this.handleBackgroundChange}
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
                                        value={this.state.userName}
                                        onChange={this.handleUserNameChange}
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
                        <div >

                            <div>
                                <p className="custom-dialog__detail-header" >
                                    Thông tin cá nhân
                                </p>
                                <div style={{ marginBottom: '22px' }}>
                                    <div className="custom-dialog__details-line">
                                        <label>
                                            Giới tính :
                                        </label>
                                        <div>
                                            <label>
                                                <input type="radio" value="nam" checked={this.state.gender === "nam"} onChange={this.handleGenderChange} />
                                                Nam
                                            </label>
                                        </div>
                                        <div>
                                            <label>
                                                <input type="radio" value="nu" checked={this.state.gender === "nu"} onChange={this.handleGenderChange} />
                                                Nữ
                                            </label>
                                        </div>
                                    </div>
                                    <div className="custom-dialog__details-line">
                                        <label>
                                            Ngày sinh :
                                            <input type="datetime-local" value={this.state.dateOfBirth} onChange={this.handleDateOfBirthChange} />
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
                    <Button variant="secondary" onClick={this.handleClose}>
                        Đóng
                    </Button>
                </Modal.Footer>
            </Modal>);
        }

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
                                style={{ cursor: 'pointer', width: ' 466.4px', height: '310.95px' }}
                                src={this.state.backgroundImageToload}
                            />
                            <div className="avatar-container">
                                <img
                                    alt="User's avatar"
                                    className="a-child"
                                    src={this.state.avatarImageToLoad}
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
                        <Button variant="primary center" onClick={this.handleOpenDialog}>
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