import React, { useState, useEffect } from 'react';
//image
import UserAvatar from "./assets/friends.png";
import GroupAvatar from "./assets/community-3245739_640.png";
import FriendIcon from "./assets/icon/icons8-group-50.png";
import GroupIcon from "./assets/icon/icons8-group-64.png";
import LetterIcon from "./assets/icon/icons8-letter-64.png";
import SearchIcon from "./assets/icon/searchIcon.png";

//api
import { GetListFriend, UnfriendAPI, GetFriendRequestsByIdOfReceiverAPI, GetFriendRequestsByIdOfSenderAPI, AcceptFriendRequestAPI, DeniedFriendRequestAPI } from '../Services/friendService';
import { getuserApi } from '../Services/userService';
import { GetAllGroupChatsOfUserByUserIdAPI } from '../Services/groupService';
//react bootstrap
import { ListGroup, ListGroupItem, OverlayTrigger } from 'react-bootstrap';
import { Popover } from 'react-bootstrap';
import { Spinner } from "react-bootstrap";
import UserInforSearchedDialog from './UserInforSearchedDialog';
import { useCallback } from 'react';


function PhoneBook(props) {

  const [selectedButton, setSelectedButton] = useState('friends');
  const [usersList, setUserList] = useState([]);
  const [sendFriendRequestList, setSendFriendRequestList] = useState([]);
  const [receiveFriendRequestList, setreceiveFriendRequestList] = useState([]);
  const [listGroup, setListGroup] = useState([]);
  const [usersListTmp, setUserListTmp] = useState([]);
  const [listGroupTmp, setListGroupTmp] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [searchText, setSearchText] = useState("");
  const [searchGroupText, setSearchGroupText] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [userInforSearched, setuserInforSearched] = useState(false);
  const [UserInfor, setUserInfor] = useState(null);


  const fetchData = useCallback(async () => {
    try {
      const [listFriendResponse, sendRequestResponse, receiveRequestResponse, listGroupResponse] = await Promise.all([
        GetListFriend(props.userId),
        GetFriendRequestsByIdOfSenderAPI(props.userId),
        GetFriendRequestsByIdOfReceiverAPI(props.userId),
        GetAllGroupChatsOfUserByUserIdAPI(props.userId)
      ]);

      setUserList(listFriendResponse.data);
      setUserListTmp(listFriendResponse.data);
      setSendFriendRequestList(sendRequestResponse.data);
      setreceiveFriendRequestList(receiveRequestResponse.data);
      setListGroup(listGroupResponse.data);
      setListGroupTmp(listGroupResponse.data);
    } catch (error) {
      if (error.response) {
        alert(error.response.data.error);
      } else {
        alert("An error occurred");
      }
    } finally {
      setIsLoading(false);
    }
  }, [props.userId]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);


  const handleUserContextMenu = (e, user) => {
    e.preventDefault();

    setSelectedUser(user);
  };

  const hidePopover = () => {
    setSelectedUser(null);
  };
  const HandleSearchChange = (e) => {
    const value = e.target.value;
    setSearchText(value);

    if (value.trim() === "") {
      setUserList(usersListTmp);
    } else {
      const filteredList = usersListTmp.filter((user) =>
        user.userName.toLowerCase().includes(value.toLowerCase())
      );
      setUserList(filteredList);
    }
  };

  const HandleSearchGroupChange = (e) => {
    const value = e.target.value;
    setSearchGroupText(value);

    if (value.trim() === "") {
      setListGroup(listGroupTmp);
    } else {
      const filteredList = listGroupTmp.filter((group) =>
        group.name.toLowerCase().includes(value.toLowerCase())
      );
      setListGroup(filteredList);
    }
  };


  const handleUnfriend = (userSenderId, userReceiverId) => {
    UnfriendAPI(userSenderId, userReceiverId)
      .then((response) => {
        if (response) {
          alert("Xóa thành công");
          GetListFriend(userSenderId).then((response) => {
            setUserList(response.data);
            setUserListTmp(response.data);
            setSearchText("");
          });
        }
      })
      .catch((error) => {
        alert("Lỗi kết nối. Vui lòng thử lại sau.");
      });
  };
  const handleOpenUserInforSearchedDialog = async (email) => {
    try {
      let receiverRes = await getuserApi(email);
      if (receiverRes) {
        setUserInfor(receiverRes.data);
        setuserInforSearched(true);
      }
    } catch (error) {
      if (error.response) {
        alert(error.response.data.error);
      } else {
        alert("An error occurred");
      }
    }

  }
  const handleCloseUserInforSearchedDialog = () => {
    setUserInfor(null);
    setuserInforSearched(false);
    hidePopover();
  }
  const handleAcceptFriendRequest = (UserSrcId, UserDesId) => {
    try {
      AcceptFriendRequestAPI(UserSrcId, UserDesId).then((response) => {
        if (response) {
          fetchData();
          alert("Xác nhận thành công");

        }
      });
    } catch (error) {
      if (error.response) {
        alert(error.response.data.error);
      } else {
        alert("An error occurred");
      }
    }
  }
  const handleDeniedFriendRequest = (UserSrcId, UserDesId) => {
    try {
      DeniedFriendRequestAPI(UserSrcId, UserDesId).then((response) => {
        if (response) {
          fetchData();
          alert("Hủy thành công");
        }
      });
    } catch (error) {
      if (error.response) {
        alert(error.response.data.error);
      } else {
        alert("An error occurred");
      }
    }
  };




  const showPopover = (user) => (
    <Popover id="popover-basic" className="custom-popover" placement="right">
      <Popover.Header as="h3">
        <span className="popover-body" onClick={() => handleOpenUserInforSearchedDialog(user.email)} >Xem thông tin</span>
      </Popover.Header>
      <Popover.Body>
        <span className="popover-body" >Chặn</span>
      </Popover.Body>
      <Popover.Body>
        <span className="popover-body" onClick={() => handleUnfriend(props.userId, user.id)} >Xóa bạn</span>
      </Popover.Body>

      <Popover.Header as="h5">

        <span className="popover-body" onClick={hidePopover} >Đóng</span>
      </Popover.Header>
    </Popover>
  );

  const renderUserList = (userList) => {


    return userList.map((user) => (
      <OverlayTrigger
        key={user.id}
        trigger="click"
        show={selectedUser === user}
        onHide={hidePopover}
        placement="left"
        overlay={showPopover(user)}
      >
        <ListGroup.Item
          key={user.id}
          style={{
            borderBottom: '1px solid black',
            paddingTop: '5px',
            paddingBottom: '5px',
            boxShadow: '0px 1px 0px 0px #ccc',
          }}
          onClick={(e) => handleUserContextMenu(e, user)}
        >
          <div className="row align-items-center">
            <div className="col-2 d-flex justify-content-center">
              <img
                src={user.avatar ? 'data:image;base64,' + user.avatar : UserAvatar}
                className="rounded-circle user-avatar border"
                width="64px"
                height="64px"
                alt=""
              />
            </div>
            <div className="col user-details">
              <span className="user-name">{user.userName}</span>
            </div>
          </div>
        </ListGroup.Item>
      </OverlayTrigger>
    ));
  };
  // nhận từ các người khác send cho mình
  const renderFriendRequestToMe = (FrienRequestList) => {
    if (FrienRequestList.length === 0) {
      return (
        <>
          <h3>Không có lời mời nào hiện tại</h3>
          <div style={{ height: '300px' }}></div>
        </>
      );
    }
    return FrienRequestList.map((request) => (

      <ListGroup.Item
        key={request.user1}
        style={{
          borderBottom: '1px solid black',
          paddingTop: '5px',
          paddingBottom: '5px',
          boxShadow: '0px 1px 0px 0px #ccc',
        }}

      >
        <div className="row align-items-center">
          <div className="col-2" onClick={() => handleOpenUserInforSearchedDialog(request.email)}>
            <img
              src={request.avatar ? 'data:image/jpeg;base64,' + request.avatar : UserAvatar}
              className="rounded-circle"
              width="48 px"
              height="48 px"
              alt=""
            />
          </div>
          <div className="col-7">
            <span className="user-name">{request.userName}</span>

          </div>
          <div className="col-3 text-end">
            <button className='z--btn--v2 btn-outline-tertiary-primary'
              onClick={() => handleAcceptFriendRequest(request.user1, props.userId)}
            >
              Chấp nhận
            </button>
            <button className='z--btn--v2 btn-outline-tertiary-primary'
              onClick={() => handleDeniedFriendRequest(request.user1, props.userId)}
            >
              Xóa lời mời
            </button>
          </div>
        </div>

      </ListGroup.Item>
    ));

  };
  const renderFriendRequestToOtherPeople = (FrienRequestList) => {
    if (FrienRequestList.length === 0) {
      return (
        <>
          <h3>Không có lời mời nào hiện tại</h3>
          <div style={{ height: '300px' }}></div>
        </>
      );
    }
    return FrienRequestList.map((request) => (

      <ListGroup.Item
        key={request.user2}
        style={{
          borderBottom: '1px solid black',
          paddingTop: '5px',
          paddingBottom: '5px',
          boxShadow: '0px 1px 0px 0px #ccc',
        }}
        onContextMenu={(e) => handleUserContextMenu(e, request)}
      >
        <div className="row align-items-center">
          <div className="col-2" onClick={() => handleOpenUserInforSearchedDialog(request.email)}>
            <img
              src={request.avatar ? 'data:image/jpeg;base64,' + request.avatar : UserAvatar}
              className="rounded-circle"
              width="48 px"
              height="48 px"
              alt=""
            />
          </div>
          <div className="col-7">
            <span className="user-name">{request.userName}</span>

          </div>
          <div className="col-3 text-end">
            <button className='z--btn--v2 btn-outline-tertiary-primary'
              onClick={() => handleDeniedFriendRequest(props.userId, request.user2)}
            >

              Hủy lời mời
            </button>
          </div>
        </div>
      </ListGroup.Item>
    ));

  };
  const renderListGroupOfUser = (listGroup) => {
    if (listGroup.length === 0) {
      return (
        <>
          <h3>Không có nhóm nào hiện tại</h3>
          <div style={{ height: '300px' }}></div>
        </>
      );
    }
    return listGroup.map((request) => (

      <ListGroup.Item
        key={request.idGroup}
        style={{
          borderBottom: '1px solid black',
          paddingTop: '5px',
          paddingBottom: '5px',
          boxShadow: '0px 1px 0px 0px #ccc',
        }}
      //onContextMenu={(e) => handleUserContextMenu(e, request)}
      >
        <div className="row align-items-center">
          <div className="col-2">
            <img
              src={request.imageByBase64
                ? 'data:image/jpeg;base64,' + request.imageByBase64
                : GroupAvatar}
              className="rounded-circle"
              width="48 px"
              height="48 px"
              alt=""
            />
          </div>
          <div className="col-7">
            <span className="user-name">{request.name}</span>
          </div>
        </div>
      </ListGroup.Item>
    ));

  };

  const renderContent = () => {

    if (selectedButton === 'friends') {
      return <div className="main" style={{ backgroundColor: 'yellow' }}>
        <div style={{ backgroundColor: 'blue' }}>
          <div className="chat-title">
            <div className="contact-name">
              <img
                src={FriendIcon}
                alt="" />Danh sách bạn bè</div>
          </div>
        </div>
        <div className="contact-search-box1">

          <div className="search-container">
            <img src={SearchIcon} alt="" className="search-icon" />
            <input
              type="text"
              placeholder="Tìm kiếm"
              className="search-input"
              value={searchText}
              onChange={HandleSearchChange}
            />
          </div>
        </div>
        {isLoading ? (
          <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
            <Spinner animation="border" variant="primary" />
          </div>
        ) : (
          <>
            <span className="user-name">
              Bạn bè {'('} {usersList.length} {')'}
            </span>
            <main style={{ backgroundColor: 'green', flex: 1 }}>
              <div style={{
                height: '330px',
                overflowY: 'auto',
              }}>
                <ListGroup className="user-list">
                  {renderUserList(usersList)}
                </ListGroup>
              </div>
            </main>
          </>
        )}
      </div>
    } else if (selectedButton === 'groups') {
      return <div className="main" style={{ backgroundColor: 'yellow' }}><div style={{ backgroundColor: 'blue' }}>
        <div className="chat-title">
          <div className="contact-name">
            <img
              src={GroupIcon}
              alt="" />Danh sách nhóm</div>
        </div>
      </div >
        <div className="contact-search-box1">

          <div className="search-container">
            <img src={SearchIcon} alt="" className="search-icon" />
            <input
              type="text"
              placeholder="Tìm kiếm"
              className="search-input"
              value={searchGroupText}
              onChange={HandleSearchGroupChange}
            />
          </div>
        </div>
        {isLoading ? (
          <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
            <Spinner animation="border" variant="primary" />
          </div>
        ) : (
          <>

            <main style={{ backgroundColor: 'green', flex: 1 }}>
              <div className="card-title">
                Danh sách nhóm :
              </div >
              <div style={{
                height: '330px',
                overflowY: 'auto',
              }}>
                <ListGroup className="user-list" style={{ height: '330px' }}>
                  {renderListGroupOfUser(listGroup)}
                </ListGroup>
              </div>
            </main>
          </>
        )}

      </div>;
    } else if (selectedButton === 'invitations') {
      return <div className="main" style={{ backgroundColor: 'yellow' }}><div style={{ backgroundColor: 'blue' }}>
        <div className="chat-title">
          <div className="contact-name">
            <img
              src={LetterIcon}
              alt="" />Lời mời kết bạn</div>
        </div>
      </div >
        <main style={{ backgroundColor: 'green' }}>
          {isLoading ? (
            <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
              <Spinner animation="border" variant="primary" />
            </div>
          ) : (
            <>

              <main style={{ backgroundColor: 'green', flex: 1 }}>
                <div className="card-title">
                  Lời mời đã nhận
                </div >
                <div style={{
                  height: '330px',
                  overflowY: 'auto',
                }}>
                  <ListGroup className="user-list" style={{ height: '330px' }}>
                    {renderFriendRequestToMe(receiveFriendRequestList)}
                  </ListGroup>
                </div>

                <div className="card-title">
                  Lời mời đã gửi
                </div>
                <div style={{
                  height: '330px',
                  overflowY: 'auto',
                }}>
                  <ListGroup className="user-list" style={{ height: '330px' }}>
                    {renderFriendRequestToOtherPeople(sendFriendRequestList)}
                  </ListGroup>
                </div>
              </main>
            </>
          )}
        </main>
      </div>;
    }
  };

  return (
    <>
      {
        userInforSearched && (
          < UserInforSearchedDialog show={userInforSearched}
            handleClose={handleCloseUserInforSearchedDialog}
            user={UserInfor}
            senderId={props.userId}
          />
        )
      }
      <div className="conversation-list-container" style={{ backgroundColor: 'red' }}>
        <button className={`vertical-button ${selectedButton === 'friends' ? 'selected' : ''}`} onClick={() => setSelectedButton('friends')}>
          <img src={FriendIcon} width="32px" height="32px" alt="" /> Danh sách bạn bè
        </button>
        <button className={`vertical-button ${selectedButton === 'groups' ? 'selected' : ''}`} onClick={() => setSelectedButton('groups')}>
          <img src={GroupIcon} width="32px" height="32px" alt="" /> Danh sách nhóm
        </button>
        <button className={`vertical-button ${selectedButton === 'invitations' ? 'selected' : ''}`} onClick={() => setSelectedButton('invitations')}>
          <img src={LetterIcon} width="32px" height="32px" alt="" /> Lời mời kết bạn
        </button>
      </div>

      {renderContent()}

    </>
  );
}

export default PhoneBook;