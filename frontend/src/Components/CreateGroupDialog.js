import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import AddUserIcon from "./assets/icon/addUserIcon.png";
import CameraIcon from "./assets/icon/photo-camera-interface-symbol-for-button.png";
import SearchIcon from "./assets/icon/searchIcon.png";
import { ListGroup, ListGroupItem } from "react-bootstrap";
import Test from "./assets/test.png";
import { GetListFriend } from '../Services/friendService';
import UserAvatar from "./assets/friends.png";
import { CreateGroupChat,AddManyGroupUser } from '../Services/groupService';
import Swal from 'sweetalert2';
class CreateGroupDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isFinishLoading: false,
            listFriends: [],
            originalListFriends: [],
            listSelectedUser: [],
            searchText: '',
            avatarImage: '',
            groupName:''
        }
        this.HandleSearchChange = this.HandleSearchChange.bind(this);
        this.handleAvatarChange = this.handleAvatarChange.bind(this);
        this.handlegroupNameChange = this.handlegroupNameChange.bind(this);
    }
    componentDidMount = async () => {
        let listFriendsRes = await GetListFriend(this.props.userId);
        if (listFriendsRes) {
            this.setState({
                originalListFriends: listFriendsRes.data,
                listFriends: listFriendsRes.data,
                isFinishLoading: true,
            });
        }
    }
    handleCheckboxChange = (friend) => {
        this.setState(prevState => {
            const listSelectedUser = prevState.listSelectedUser.includes(friend)
                ? prevState.listSelectedUser.filter(f => f !== friend)
                : [...prevState.listSelectedUser, friend];
            
            return { listSelectedUser };

        });
    }
    handlegroupNameChange =(event)=>{
        this.setState({ groupName: event.target.value });
    }
    handleCreateGroup = async () => {
        var base64StringAvatar = null;
        
        if (this.state.avatarImage) {
            var avatar = this.state.avatarImage;
            base64StringAvatar = avatar.split(',')[1];
        } 
        try{
            let createGrRes = await CreateGroupChat(this.state.groupName,this.props.userId,base64StringAvatar);
            if(createGrRes){
                const usersToAdd = this.state.listSelectedUser.map(user => ({
                    idUser: user.id, 
                    idGroup: createGrRes.data, 
                    idGroupRole: 0 ,
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
        }
        catch(err){
            console.log(err);
        }
    }

    HandleSearchChange(event) {
        const searchText = event.target.value;
        const filteredFriends = searchText
            ? this.state.listFriends.filter(friend =>
                friend.userName.toLowerCase().includes(searchText.toLowerCase())
            )
            : this.state.originalListFriends;

        this.setState({ searchText, listFriends: filteredFriends });

    }

    handleClose = () => {
        this.setState({
            listSelectedUser: [],
            searchText: '',
            avatarImage: '',
            groupName:''
        });

        this.props.handleClose(); // Gọi hàm handleClose từ props để đóng dialog
    }
    handleAvatarChange = (event) => {
        const avatarImage = event.target.files[0];
        if (avatarImage && avatarImage.type.startsWith('image/')) {
            this.getBase64(avatarImage, (result) => {
                this.setState({ avatarImage: result });
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
    render() {

        const { show, handleClose } = this.props;
        let rows = [];

        for (let i = 0; i < this.state.listFriends.length; i++) {
            let avatarImageToLoad = 'data:image/jpeg;base64,' + this.state.listFriends[i].avatar;
            //data:image;base64,
            rows.push(
                <ListGroupItem
                    key={this.state.listFriends[i].id}

                    action
                >

                    <div className="float-start">
                        <input
                            type="checkbox"
                            style={{ marginRight: '10px' }}
                            checked={this.state.listSelectedUser && this.state.listSelectedUser.includes(this.state.listFriends[i])}
                            onChange={() => this.handleCheckboxChange(this.state.listFriends[i])}
                        />

                        <img
                            src={this.state.listFriends[i].avatar ? avatarImageToLoad : UserAvatar}
                            className="rounded-circle"
                            width="48 px"
                            height="48 px"
                            alt=""
                        />

                    </div>
                    <div className="float-start ms-3">
                        <span className="float-right">{this.state.listFriends[i].userName}</span>
                    </div>
                </ListGroupItem>
            );
        }
        let usersSelected = [];

        for (let i = 0; i < this.state.listSelectedUser.length; i++) {
            let avatarImageToLoad = 'data:image/jpeg;base64,' + this.state.listSelectedUser[i].avatar;
            usersSelected.push(
                <ListGroupItem
                    key={this.state.listSelectedUser[i].id}
                    action
                >
                    <div className="float-start">
                        <img
                            src={this.state.listSelectedUser[i].avatar ? avatarImageToLoad : UserAvatar}
                            className="rounded-circle"
                            width="48 px"
                            height="48 px"
                            alt=""
                        />

                    </div>
                    <div className="float-start ms-5">
                        <span className="float-right">{this.state.listSelectedUser[i].userName}</span>
                    </div>
                </ListGroupItem>
            );
        }


        return (
            <>
                <Modal show={show} onHide={this.handleClose} dialogClassName="custom-dialog" >
                    <Modal.Header closeButton >
                        <Modal.Title > Tạo nhóm </Modal.Title>

                    </Modal.Header>
                    <Modal.Body className="custom-dialog__body" >
                        <form >
                            <div >
                                <div className="custom-dialog__user-name-container" style={{ marginTop: '20px' }}>
                                    <div className="user-name-label" >
                                        <label htmlFor="avatarInput">

                                            {this.state.avatarImage ? (
                                                <img src={this.state.avatarImage} alt="Selected" width="35.6 px" height="38.2 px" style={{
                                                    borderRadius: '50%',
                                                    fontWeight: '400',
                                                    cursor: 'pointer' // Đổi con trỏ chuột thành dấu hỏi để chỉ ra có thể click vào
                                                }} />

                                            ) : (
                                                <span className="user-name-label__text" style={{
                                                    border: '1px solid black',
                                                    borderRadius: '50%',
                                                    padding: '10px',
                                                    paddingTop: '5px',
                                                    fontWeight: '400',
                                                    cursor: 'pointer' // Đổi con trỏ chuột thành dấu hỏi để chỉ ra có thể click vào
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
                                            onChange={this.handleAvatarChange}
                                        />
                                    </div>

                                    <span className="user-name-input">
                                        <input
                                            tabIndex="1"
                                            placeholder="Nhập tên nhóm"
                                            className="user-name-input__field"
                                            value={this.state.groupName}
                                            onChange={this.handlegroupNameChange}
                                        />
                                    </span>

                                </div>
                            </div>

                        </form>
                        <span className="user-name-label__text">
                            Thêm thành bạn vào nhóm
                        </span>
                        <div className="user-name-input">
                            <input
                                style={{ borderRadius: '25px' }}
                                tabIndex="1"
                                placeholder="Nhập tên người cần thêm"
                                className="user-name-input__field"
                                value={this.state.searchText}
                                onChange={this.HandleSearchChange}
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
                    <Modal.Footer className="custom-dialog__footer" >
                        <Button variant="secondary" onClick={this.handleClose}>
                            Đóng
                        </Button>
                        <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                            <Button variant="primary center" onClick={this.handleCreateGroup}>
                                Tạo nhóm
                            </Button>
                        </div>
                    </Modal.Footer>
                </Modal>
            </>
        );
    }
}

export default CreateGroupDialog;