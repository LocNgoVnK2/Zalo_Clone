import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, FormControl, InputGroup, Row, Col } from 'react-bootstrap';
import UserAvatar from "./assets/friends.png";
import { GetAllUsersInGroupAPI, UpdateChatLeaderAPI } from '../Services/groupService';
import Swal from 'sweetalert2';
function TaskDetailDialog(props) {
    useEffect(() => {
        const fetchData = () => {
        };

        fetchData();
    }, []);


    return (
        <Modal show={props.showModal} onHide={props.handleClose} size="lg">
            <Modal.Header closeButton>
                <Modal.Title>Chi tiết công việc</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Row>
                    <Col md={8} className='split-content'>
                        <div className='message-site-container'>
                            <div >
                                <div className='review-main'>
                                    <div className='review-title'>
                                        cc
                                    </div>
                                    <div className='review-content'>
                                        cc
                                    </div>
                                </div>
                                <div>
                                    {/* LOAD MESSAGE IN HERE */}
                                </div>
                            </div>
                        </div>
                        <div>
                            {/* SEND MESSAGE INPUT HERE */}
                        </div>
                    </Col>
                    <Col md={4}>
                        <div className='setup-site-container' >
                            <div className='right-column-section' style={{ height: '70px', backgroundColor: 'red' }}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Trạng thái
                                    </span>
                                </div>
                                <div className='label-status'>

                                    <div>
                                        <span className='pr-status-content' >
                                            cc {/* render status here */}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div className='right-column-section' style={{ height: '85px', backgroundColor: 'white' }}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Ngày hết hạn
                                    </span>
                                </div>
                                <div className='label-status'>
                                    <div>
                                        <span className='pr-status-content-deadline' >
                                            ngày nào đó {/* render deadline here */}
                                        </span>
                                        <br />
                                        <span className='pr-status-content' >
                                            còn xxx ngày {/* render deadline here */}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div className='right-column-section' style={{ height: '80px', backgroundColor: 'green' }}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Giao cho
                                    </span>
                                </div>
                                <Button variant="primary">
                                    Xem danh sách thành viên
                                </Button>

                            </div>
                            <div className='right-column-section' style={{ height: '260px', backgroundColor: 'yellow' }}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Tác vụ
                                    </span>
                                </div>
                                <div>
                                    <Button variant='light' className='gray-button'>
                                       
                                        Button 1
                                    </Button>
                                    <Button variant='light' className='gray-button'>
                                       
                                        Button 2
                                    </Button>
                                    <Button variant='light' className='gray-button'>
                                        
                                        Button 3
                                    </Button>
                                    <Button variant='light' className='gray-button'>
                                       
                                        Button 4
                                    </Button>
                                </div>
                            </div>
                        </div>
                    </Col>
                </Row>
            </Modal.Body>
        </Modal>
    );

}
export default TaskDetailDialog;