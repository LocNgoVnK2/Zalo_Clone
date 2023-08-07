import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import avatar from "./assets/test.png";
import UserAvatar from "./assets/friends.png";
import background from './assets/background-may-dep-cho-khai-giang.jpg';
import Test from "./assets/test.png";
import { ListGroup, ListGroupItem } from "react-bootstrap";
import { getuserApi } from '../Services/userService';
import { SendFriendRequest, CheckIsFriend } from '../Services/friendService';
import UserInforSearchedDialog from './UserInforSearchedDialog';
import { RecommandFriend } from '../Services/friendService';
class AddFriendDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            userInforSearched: false,
            returnUserInfor: null,
            listRecomandUser: [],
            isFinishLoading: false
        }
       
        this.handleEmailChange = this.handleEmailChange.bind(this);

    }
    handleEmailChange = (event) => {
        this.setState({ email: event.target.value });
    };
    componentDidMount = () => {
        RecommandFriend(this.props.userId).then((response) => {
            this.setState({
                listRecomandUser: response.data,
                isFinishLoading: true,
            });
        });
    }

    handleAddFriend = async (userId, email) => {
        try {
           
            let receiverRes = await getuserApi(email);
            
            if (receiverRes.data.id !== userId) {
                
                let checkFriend = await CheckIsFriend(userId, email);
               
                if (receiverRes && checkFriend.data === false) {
                    
                    let sendFriendRequestRes = await SendFriendRequest(userId, receiverRes.data.id);
                    
                    if (sendFriendRequestRes) {
                        alert("Send Friend Request success");
                        this.componentDidMount();
                    }
                } else if (checkFriend === true) {
                    alert("Đã là bạn bè");
                    return ("AreFriends");
                }
            } else {
                return ("ThisIsYou");
            }
        } catch (error) {
            if (error.response && error.response.status === 404) {
                alert("Đang xử lí yêu cầu");
                return ("Requesting");
            } else if (error.response) {
                alert(error.response.data.error);
                return null;
            } else {
                alert("An error occurred");
                return null;
            }
        }
    }
    handleOpenUserInforSearchedDialog = async (email) => {
        try {
            let receiverRes = await getuserApi(email);
            if (receiverRes) {
                this.setState({ userInforSearched: true, returnUserInfor: receiverRes.data });
                
            }

        } catch (error) {
            if (error.response) {
                alert(error.response.data.error);
            } else {
                alert("An error occurred");
            }
        }

    }

    handleCloseUserInforSearchedDialog = () => {
        this.setState({ userInforSearched: false, returnUserInfor: null });
    }

    render() {
        let rows = [];
        let rowsRecentSearch = [];
        for (let i = 0; i < 3; i++) {
            let name = 'Test';
            rowsRecentSearch.push(
                <ListGroupItem key={i} >
                    <div className="row align-items-center">
                        <div className="col-2">
                            <img
                                src={Test}
                                className="rounded-circle"
                                width="48 px"
                                height="48 px"
                                alt=""
                            />
                        </div>
                        <div className="col-7">
                            <span className="float-right">{name}</span>
                            <br />

                        </div>
                    </div>
                </ListGroupItem>
            );
        }
        for (let i = 0; i < this.state.listRecomandUser.length; i++) {
            let avatarImageToLoad = 'data:image/jpeg;base64,' + this.state.listRecomandUser[i].avatar;
                rows.push(
                    <ListGroupItem key={this.state.listRecomandUser[i]} >
                        <div className="row align-items-center">
                            <div className="col-2" onClick={() => this.handleOpenUserInforSearchedDialog(this.state.listRecomandUser[i].email)}>
                                <img
                                    src={this.state.listRecomandUser[i].avatar? avatarImageToLoad : UserAvatar}
                                    className="rounded-circle"
                                    width="48 px"
                                    height="48 px"
                                    alt=""
                                />
                            </div>
                            <div className="col-7" onClick={() => this.handleOpenUserInforSearchedDialog(this.state.listRecomandUser[i].email)}>
                                <span className="float-right">{this.state.listRecomandUser[i].userName}</span>
                                <br />
                                <span className="float-right">Từ gợi ý kết bạn</span>
                            </div>
                            <div className="col-3 text-end">
                                <button
                                    className="z--btn--v2 btn-outline-tertiary-primary"
                                onClick={() => this.handleAddFriend(this.props.userId,this.state.listRecomandUser[i].email)}
                                >
                                    Kết bạn
                                </button>
                            </div>
                        </div>
                    </ListGroupItem>
                );
        }

        const { show, handleClose } = this.props;
        return (
            <>

                {
                    this.state.returnUserInfor && (
                        < UserInforSearchedDialog show={this.state.userInforSearched}
                            handleClose={this.handleCloseUserInforSearchedDialog}
                            user={this.state.returnUserInfor}
                            handleAddFriend={this.handleAddFriend}
                            senderId={this.props.userId}
                        />
                    )
                }
                <Modal show={show} onHide={handleClose} dialogClassName="custom-dialog">
                    <Modal.Header closeButton >
                        <Modal.Title > Thêm bạn </Modal.Title>

                    </Modal.Header>
                    <Modal.Body className="custom-dialog__body">
                        <form >
                            <div style={{ flex: '1 1 0%' }}>
                                <div className="custom-dialog__user-name-container" style={{ marginTop: '20px' }}>
                                    <div className="user-name-label">
                                        <span className="user-name-label__text">
                                            Email
                                        </span>
                                    </div>
                                    <span className="user-name-input">
                                        <input
                                            tabIndex="1"
                                            placeholder="Nhập tên email"
                                            className="user-name-input__field"
                                            onChange={this.handleEmailChange}
                                        />
                                    </span>

                                </div>
                            </div>
                            <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                                <Button variant="primary center" onClick={() => this.handleOpenUserInforSearchedDialog(this.state.email)}>
                                    Tìm kiếm
                                </Button>
                            </div>
                        </form>
                        <div style={{
                            width: 'calc(100% + 30px)',
                            height: '330px',
                            marginLeft: '-15px',

                            overflowY: 'auto',
                        }}>
                            <span className="user-name-label__text">
                                Tìm kiếm gần đây
                            </span>
                            <ListGroup variant="pills">{rowsRecentSearch}</ListGroup>
                            <span className="user-name-label__text">
                                Bạn có thể quen
                            </span>
                            {this.state.isFinishLoading && (
                               <ListGroup variant="pills">{rows}</ListGroup>
                            )}
                            

                        </div>
                    </Modal.Body>
                    <Modal.Footer className="custom-dialog__footer">
                        <Button variant="secondary" onClick={handleClose}>
                            Đóng
                        </Button>
                    </Modal.Footer>
                </Modal>
            </>
        );
    }
}

export default AddFriendDialog;