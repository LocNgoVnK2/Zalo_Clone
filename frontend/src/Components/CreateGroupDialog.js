import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import AddUserIcon from "./assets/icon/addUserIcon.png";
import CameraIcon from "./assets/icon/photo-camera-interface-symbol-for-button.png";
import SearchIcon from "./assets/icon/searchIcon.png";
import { ListGroup, ListGroupItem } from "react-bootstrap";
import Test from "./assets/test.png";
class CreateGroupDialog extends Component {
    constructor(props) {
        super(props);
        this.state = {

        }

    }


    render() {
        const { show, handleClose } = this.props;
        let rows = [];
        for (let i = 0; i < 30; i++) {
            rows.push(
                <ListGroupItem
                    key={i}
                    action
                >

                    <div className="float-start">
                        <input type="radio" style={{ marginRight: '10px' }} />
                        <img
                            src={Test}
                            className="rounded-circle"
                            width="48 px"
                            height="48 px"
                            alt=""
                        />

                    </div>
                    <div className="float-start ms-3">
                        <span className="float-right">test</span>
                    </div>
                </ListGroupItem>
            );
        }


        return (
            <>
                <Modal show={show} onHide={handleClose} dialogClassName="custom-dialog">
                    <Modal.Header closeButton >
                        <Modal.Title > Tạo nhóm </Modal.Title>

                    </Modal.Header>
                    <Modal.Body className="custom-dialog__body">
                        <form >
                            <div style={{ flex: '1 1 0%' }}>
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
                                placeholder="Nhập tên email hoặc số điện thoại người cần thêm"
                                className="user-name-input__field"
                            />
                            <div className="search-icon-wrapper">
                                <img src={SearchIcon} alt="" className="search-icon" />
                            </div>
                        </div>
                        <span className="user-name-label__text">
                            Danh sách bạn bè:
                        </span>
                        <div style={{
                            width: 'calc(100% + 30px)',
                            height: '500px',
                            marginLeft: '-15px',

                            overflowY: 'auto',
                        }}>

                            <ListGroup variant="pills">{rows}</ListGroup>


                        </div>
                    </Modal.Body>
                    <Modal.Footer className="custom-dialog__footer" >
                        <Button variant="secondary" onClick={handleClose}>
                            Đóng
                        </Button>
                        <div className="custom-dialog__actions" style={{ textAlign: 'center' }}>
                            <Button variant="primary center" >
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