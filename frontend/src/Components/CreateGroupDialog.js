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
class CreateGroupDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isFinishLoading: false,
            listFriends: [],
            originalListFriends: [],
            listSelectedUser: [],
            searchText: ''
        }
        this.HandleSearchChange = this.HandleSearchChange.bind(this);
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
    handleCreateGroup = () => {

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
    render() {

        const { show, handleClose } = this.props;
        let rows = [];

        for (let i = 0; i < this.state.listFriends.length; i++) {
            let avatarImageToLoad = 'data:image/jpeg;base64,' + this.state.listFriends[i].avatar;
            rows.push(
                <ListGroupItem
                    key={this.state.listFriends[i]}
                    action
                >

                    <div className="float-start">
                        <input
                            type="checkbox"
                            style={{ marginRight: '10px' }}
                            checked={this.state.selectedFriends && this.state.selectedFriends.includes(this.state.listFriends[i])}
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
        let usersSelect = [];

        for (let i = 0; i < 20; i++) {
            let avatarImageToLoad = 'data:image/jpeg;base64,';
            usersSelect.push(
                <ListGroupItem
                    key={i}
                    action
                >
                    <div className="float-start">
                        <img
                            src={Test}
                            className="rounded-circle"
                            width="48 px"
                            height="48 px"
                            alt=""
                        />

                    </div>
                    <div className="float-start ms-3">
                        <span className="float-right">test name</span>
                    </div>
                </ListGroupItem>
            );
        }


        return (
            <>
                <Modal show={show} onHide={handleClose} dialogClassName="custom-dialog" >
                    <Modal.Header closeButton >
                        <Modal.Title > Tạo nhóm </Modal.Title>

                    </Modal.Header>
                    <Modal.Body className="custom-dialog__body" >
                        <form >
                            <div >
                                <div className="custom-dialog__user-name-container" style={{ marginTop: '20px' }}>
                                    <div className="user-name-label">
                                        <span className="user-name-label__text" style={{
                                            border: '1px solid black',
                                            borderRadius: '50%',
                                            padding: '10px',
                                            paddingTop: '5px',
                                            fontWeight: '400'
                                        }}>

                                            <img src={CameraIcon} alt="" width="14 px" height="14 px" />


                                        </span>
                                    </div>
                                    <span className="user-name-input">
                                        <input
                                            tabIndex="1"
                                            placeholder="Nhập tên nhóm"
                                            className="user-name-input__field"
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
                        <span className="user-name-label__text"style={{ flex: '65%', height: '30px', overflowY: 'auto' }}>
                            Danh sách bạn bè:
                        </span>

                        <span className="user-name-label__text"style={{ flex: '30%', height: '30px', overflowY: 'auto' }}>
                            Đã chọn:
                        </span>
                        </div>
                        <div style={{ display: 'flex', width: '100%' }}>
                            <div style={{ flex: '65%', height: '500px', overflowY: 'auto' }}>
                                <ListGroup variant="pills">{rows}</ListGroup>
                            </div>

                            <div style={{ flex: '30%', height: '500px', overflowY: 'auto', border: '1px solid black', marginTop: '20px', marginBottom: '20px' }}>

                                <ListGroup variant="pills">{usersSelect}</ListGroup>
                            </div>
                        </div>
                    </Modal.Body>
                    <Modal.Footer className="custom-dialog__footer" >
                        <Button variant="secondary" onClick={handleClose}>
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