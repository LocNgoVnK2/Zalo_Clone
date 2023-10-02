import React, { useState, useEffect } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import UserAvatar from "./assets/friends.png";
import background from "./assets/background-may-dep-cho-khai-giang.jpg";
import Test from "./assets/test.png";
import { ListGroup, ListGroupItem } from "react-bootstrap";
import { getuserApi } from '../Services/userService';
import { SendFriendRequest, CheckIsFriend, GetRecentSearch, AddSearchLog, RemoveSearchLog } from '../Services/friendService';
import UserInforSearchedDialog from './UserInforSearchedDialog';
import { RecommandFriend } from '../Services/friendService';
import Swal from 'sweetalert2';

function AddFriendDialog(props) {
  const [email, setEmail] = useState('');
  const [userInforSearched, setUserInforSearched] = useState(false);
  const [returnUserInfor, setReturnUserInfor] = useState(null);
  const [listRecomandUser, setListRecomandUser] = useState([]);
  const [listRecentSearch, setListRecentSearch] = useState([]);
  const [isFinishLoading, setIsFinishLoading] = useState(false);
  const [userSearchButton, setUserSearchButton] = useState(false);

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };
  const fetchData = async () => {
    let recommandFriendRes = await RecommandFriend(props.userId);
    let recentSearchRes = await GetRecentSearch(props.userId);
    if (recommandFriendRes && recentSearchRes) {
      setListRecomandUser(recommandFriendRes.data);
      setListRecentSearch(recentSearchRes.data);
      setIsFinishLoading(true);
    }
  };
  useEffect(() => {

    fetchData();
  }, [props.userId]);

  const handleAddFriend = async (userId, email) => {
    try {
      let receiverRes = await getuserApi(email);
      if (receiverRes.data.id !== userId) {
        let checkFriend = await CheckIsFriend(userId, email);
        if (receiverRes && checkFriend.data === false) {
          let sendFriendRequestRes = await SendFriendRequest(userId, receiverRes.data.id);
          if (sendFriendRequestRes) {
            Swal.fire({
              icon: 'success',
              title: 'Gửi lời mời kết bạn thành công',
              showConfirmButton: false,
              timer: 1500,
            });
            fetchData(); // Gọi lại fetchData để cập nhật danh sách sau khi gửi lời mời
          }
        } else if (checkFriend === true) {
          Swal.fire({
            icon: 'error',
            title: 'Đã là bạn bè',
            showConfirmButton: false,
            timer: 1500,
          });
        }
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Đây là bạn',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    } catch (error) {
      if (error.response && error.response.status === 404) {
        Swal.fire({
          icon: 'error',
          title: 'Đang xử lý yêu cầu',
          showConfirmButton: false,
          timer: 1500,
        });
      } else if (error.response) {
        Swal.fire({
          icon: 'error',
          title: error.response.data.error,
          showConfirmButton: false,
          timer: 1500,
        });
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Có lỗi gì đó xảy ra.',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    }
  };

  const handleOpenUserInforSearchedDialog = async (email, userButton) => {
    try {
      let receiverRes = await getuserApi(email);
      if (receiverRes) {
        setUserInforSearched(true);
        setReturnUserInfor(receiverRes.data);
        if (userButton) {
          await AddSearchLog(props.userId, receiverRes.data.id);
          fetchData(); // Gọi lại fetchData để cập nhật danh sách sau khi thêm log
        }
      }
    } catch (error) {
      if (error.response) {
        Swal.fire({
          icon: 'error',
          title: error.response.data.error,
          showConfirmButton: false,
          timer: 1500,
        });
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Có lỗi gì đó xảy ra.',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    }
  };

  const handleCloseUserInforSearchedDialog = () => {
    setUserInforSearched(false);
    setReturnUserInfor(null);
    setUserSearchButton(false);
  };

  const handleRemoveRecentSearch = async (userSrc, userDes) => {
    try {
      let res = await RemoveSearchLog(userSrc, userDes);
      if (res) {
        fetchData(); // Gọi lại fetchData để cập nhật danh sách sau khi xóa log
      }
    } catch (error) {
      if (error.response) {
        Swal.fire({
          icon: 'error',
          title: error.response.data.error,
          showConfirmButton: false,
          timer: 1500,
        });
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Có lỗi gì đó xảy ra.',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    }
  };

  let rows = [];
  let rowsRecentSearch = [];

  for (let i = 0; i < listRecentSearch.length; i++) {
    let avatarImageToLoad = 'data:image/jpeg;base64,' + listRecentSearch[i].avatar;
    rowsRecentSearch.push(
      <ListGroupItem key={listRecentSearch[i]} >
        <div className="row align-items-center">
          <div className="col-2" onClick={() => handleOpenUserInforSearchedDialog(listRecentSearch[i].email)}>
            <img
              src={listRecentSearch[i].avatar ? avatarImageToLoad : UserAvatar}
              className="rounded-circle"
              width="48 px"
              height="48 px"
              alt=""
            />
          </div>
          <div className="col-7">
            <span className="float-right">{listRecentSearch[i].userName}</span>
            <br />
            <span className="float-right">{listRecentSearch[i].email}</span>
          </div>
          <div className="col-3 text-end">
            <button
              className='z--btn--v2 btn-outline-tertiary-primary'
              onClick={() => handleRemoveRecentSearch(props.userId, listRecentSearch[i].id)}
            >
              X
            </button>
          </div>
        </div>
      </ListGroupItem>
    );
  }

  for (let i = 0; i < listRecomandUser.length; i++) {
    let avatarImageToLoad = 'data:image/jpeg;base64,' + listRecomandUser[i].avatar;
    rows.push(
      <ListGroupItem key={listRecomandUser[i]} >
        <div className="row align-items-center">
          <div className="col-2" onClick={() => handleOpenUserInforSearchedDialog(listRecomandUser[i].email)}>
            <img
              src={listRecomandUser[i].avatar ? avatarImageToLoad : UserAvatar}
              className="rounded-circle"
              width="48 px"
              height="48 px"
              alt=""
            />
          </div>
          <div className="col-7" onClick={() => handleOpenUserInforSearchedDialog(listRecomandUser[i].email)}>
            <span className="float-right">{listRecomandUser[i].userName}</span>
            <br />
            <span className="float-right">Từ gợi ý kết bạn</span>
          </div>
          <div className="col-3 text-end">
            <button
              className="z--btn--v2 btn-outline-tertiary-primary"
              onClick={() => handleAddFriend(props.userId, listRecomandUser[i].email)}
            >
              Kết bạn
            </button>
          </div>
        </div>
      </ListGroupItem>
    );
  }

  return (
    <>
      {returnUserInfor && (
        <UserInforSearchedDialog
          show={userInforSearched}
          handleClose={handleCloseUserInforSearchedDialog}
          user={returnUserInfor}
          handleAddFriend={handleAddFriend}
          senderId={props.userId}
        />
      )}
      <Modal show={props.show} onHide={props.handleClose} dialogClassName="custom-dialog">
        <Modal.Header closeButton>
          <Modal.Title>Thêm bạn</Modal.Title>
        </Modal.Header>
        <Modal.Body className="custom-dialog__body">
          <form>
            <div style={{ flex: '1 1 0%' }}>
              <div className="custom-dialog__user-name-container" style={{ marginTop: '20px' }}>
                <div className="user-name-label">
                  <span className="user-name-label__text">Email</span>
                </div>
                <span className="user-name-input">
                  <input
                    tabIndex="1"
                    placeholder="Nhập tên email"
                    className="user-name-input__field"
                    onChange={handleEmailChange}
                  />
                </span>
              </div>
            </div>
            <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
              <Button variant="primary center" onClick={() => handleOpenUserInforSearchedDialog(email, true)}>
                Tìm kiếm
              </Button>
            </div>
          </form>
          <div
            style={{
              width: 'calc(100% + 30px)',
              height: '330px',
              marginLeft: '-15px',
              overflowY: 'auto',
            }}
          >
            {isFinishLoading && listRecentSearch !== null && listRecentSearch.length > 0 && (
              <>
                <span className="user-name-label__text">Tìm kiếm gần đây</span>
                <ListGroup variant="pills">{rowsRecentSearch}</ListGroup>
              </>
            )}
            <span className="user-name-label__text">Bạn có thể quen</span>
            {isFinishLoading && <ListGroup variant="pills">{rows}</ListGroup>}
          </div>
        </Modal.Body>
        <Modal.Footer className="custom-dialog__footer">
          <Button variant="secondary" onClick={props.handleClose}>
            Đóng
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}

export default AddFriendDialog;
