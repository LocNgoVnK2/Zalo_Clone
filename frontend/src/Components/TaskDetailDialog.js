import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, FormControl, InputGroup, Row, Col } from 'react-bootstrap';
import UserAvatar from "./assets/friends.png";

import Swal from 'sweetalert2';
import AlarmIcon from "./assets/icon/icons8-alarm-48.png";
import TaskRemoveIcon from "./assets/icon/icons8-taskRemove-48.png";
import TaskDoneIcon from "./assets/icon/icons8-taskDone-48.png";
import TaskEditIcon from "./assets/icon/icons8-taskEdit-48.png";
import moment from 'moment';
import { ListGroup, ListGroupItem, OverlayTrigger } from 'react-bootstrap';
import { Popover } from 'react-bootstrap';
import { GetTaskByTaskIdAPI, UpdateRemindCountAPI, UpdateCompleteTaskAPI } from '../Services/toDoListService';
import UpdateToDoList from './UpdateToDoList';
function TaskDetailDialog(props) {
    const [task, setTask] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [remainingDays, setRemainingDays] = useState(null);
    const [updateDialogShow, setUpdateDialogShow] = useState(false);

    const [showPartners, setShowPartners] = useState(false);
    const fetchData = () => {
        //idTask
        GetTaskByTaskIdAPI(props.idTask).then((task) => {
            setTask(task.data);
            calculateRemainingDays(task.data.endDate);
            setIsLoading(false);

        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Lấy dữ liệu thất bại vui lòng thử lại sau',
                showConfirmButton: false,
                timer: 1500
            });
        });
    };
    useEffect(() => {
        fetchData();
    }, []);
    // chưa xóa công việc
    const showPopover = (data) => (
        <Popover id="popover-basic" className="custom-popover" placement="right">
            <Popover.Header as="h3">
                <span className="popover-body">Danh sách thành viên</span>
            </Popover.Header>
            <Popover.Body >
                <div className='Site-render-Members'>
                    {data.map((partners) => {
                        return (

                            <div className='user-Item ' key={partners.id}>
                                <img src={
                                    partners.avatar
                                        ? "data:image;base64," + partners.avatar
                                        : UserAvatar}
                                    alt=""
                                    className="rounded-circle user-avatar border size-mini-Avatar"
                                />
                                <span>{partners.contactName}</span>


                            </div>
                        );
                    })}
                </div>
            </Popover.Body>
        </Popover >
    );

    const hidePopover = () => {
        setShowPartners(false);
    };


    const calculateRemainingDays = (endDate) => {
        const now = moment();
        const end = moment(endDate);
        const diff = end.diff(now, 'days');
        setRemainingDays(diff);
    };
    const renderDeadlineMessage = () => {
        if (remainingDays !== null) {
            if (remainingDays > 0) {
                return `còn ${remainingDays} ngày`;
            } else if (remainingDays === 0) {
                return 'Hôm nay là hạn cuối';
            } else {
                return `Quá hạn ${Math.abs(remainingDays)} ngày`;
            }
        }
        return null;
    };
    const handleOpenUpdateTaskDialog = () => {
        setUpdateDialogShow(true);
    }
    const handleCloseUpdateTaskDialog = () => {

        setUpdateDialogShow(false);
    }
    const handleUpdateRemindCount = () => {
        UpdateRemindCountAPI(task.id).then(() => {
            fetchData();
            Swal.fire({
                icon: 'success',
                title: 'Nhắc nhở thành công',
                showConfirmButton: false,
                timer: 1500
            });
        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Nhắc nhở thất bại',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }

    const handleCompleteTask = () => {
        UpdateCompleteTaskAPI(task.id, true).then((res) => {
            props.fetchData();
            props.handleClose();
            Swal.fire({
                icon: 'success',
                title: 'Hoàn thành công việc',
                showConfirmButton: false,
                timer: 1500
            });
        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Cập nhật thất bại',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }
    const handleReOpenTask = () => {
        UpdateCompleteTaskAPI(task.id, false).then((res) => {
            props.fetchData();
            props.handleClose();
            Swal.fire({
                icon: 'success',
                title: 'Mở lại công việc thành công',
                showConfirmButton: false,
                timer: 1500
            });
        }).catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Cập nhật thất bại',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }
    // delay to load task 
    if (isLoading) {
        return <Modal show={props.showModal} onHide={props.handleClose} size="lg">
        </Modal>
    }

    return (<>
        {
            updateDialogShow && (
                < UpdateToDoList task={task}
                    showModal={updateDialogShow}
                    handleClose={handleCloseUpdateTaskDialog}
                    fetchData={props.fetchData}
                    fetchDataDetails={fetchData}
                />
            )
        }
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
                                        {task.title}
                                    </div>
                                    <div className='review-content'>
                                        {task.content}
                                    </div>
                                </div>
                                <div>

                                    {/*
                                    // với khi mà nhớ check task.isDone bằng false mới mở nhắn tin còn nếu xong r 
                                        thì khóa cái gửi tin nhắn alij nhưng vẫn load tin nhắn cũ
                                     // chưa xóa công việc nữa làm nhớ làm cái xóa công việc lun cái nút ở dưới đấy
                                    LOAD MESSAGE IN HERE */}
                                </div>
                            </div>
                        </div>
                        <div>
                            {/* SEND MESSAGE INPUT HERE */}
                        </div>
                    </Col>
                    <Col md={4}>
                        <div className='setup-site-container' >
                            <div className='right-column-section' style={{ height: '70px'}}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Trạng thái
                                    </span>
                                </div>
                                <div className='label-status'>

                                    <div>
                                        <span className='pr-status-content' >
                                            {task.isDone === true ? 'Đã hoàn thành' : 'Chưa hoàn thành'}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div className='right-column-section' style={{ height: '85px' }}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Ngày hết hạn
                                    </span>
                                </div>
                                <div className='label-status'>
                                    {task.isDone === false ? (<div>

                                        <span className='pr-status-content-deadline' >
                                            {moment(task.endDate).format('DD/MM/YYYY')}
                                        </span>
                                        <br />
                                        <span className='pr-status-content' >
                                            {renderDeadlineMessage()}
                                        </span>
                                    </div>
                                    ) : (
                                        <div>
                                            <span className='pr-status-content' >
                                                Không thời hạn
                                            </span>
                                        </div>)}

                                </div>
                            </div>
                            <div className='right-column-section' style={{ height: '80px'}}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Giao cho
                                    </span>
                                </div>
                                <OverlayTrigger
                                    placement="right"
                                    show={showPartners}
                                    onHide={hidePopover}
                                    overlay={showPopover(task.partners)}

                                >
                                    <Button variant="primary" onClick={() => setShowPartners(!showPartners)}>
                                        Xem danh sách thành viên
                                    </Button>
                                </OverlayTrigger>

                            </div>
                            <div className='right-column-section' style={{ height: '260px'}}>
                                <div className='label-status'>
                                    <span className='label-status-text'>
                                        Tác vụ
                                    </span>
                                </div>
                                {task.isDone === false ? (
                                    <div className='button-container'>
                                        <Button variant='light' className='setting-button' onClick={handleCompleteTask}>
                                            <img src={TaskDoneIcon} className='size-mini-Avatar' alt="" />
                                            Hoàn thành
                                        </Button>
                                        <Button variant='light' className='setting-button' onClick={handleUpdateRemindCount}>
                                            <img src={AlarmIcon} className='size-mini-Avatar' alt="" />
                                            Nhắc nhỡ {task.remindCount === 0 ? '' : <span style={{ color: 'red' }}> lần thứ ({task.remindCount})</span>}
                                            {/* denine task if who is person do this task */}
                                        </Button>
                                        <Button variant='light' className='setting-button' onClick={handleOpenUpdateTaskDialog}>
                                            <img src={TaskEditIcon} className='size-mini-Avatar' alt="" />
                                            Chỉnh sửa
                                        </Button>
                                        <Button variant='light' className='setting-button'>
                                            <img src={TaskRemoveIcon} className='size-mini-Avatar' alt="" />
                                            Xóa công việc
                                        </Button>
                                    </div>
                                ) : (
                                    <div className='button-container'>
                                        <Button variant='light' className='setting-button' onClick={handleReOpenTask}>
                                            <img src={TaskDoneIcon} className='size-mini-Avatar' alt="" />
                                            Mở lại
                                        </Button>
                                        <Button variant='light' className='setting-button' onClick={handleOpenUpdateTaskDialog}>
                                            <img src={TaskEditIcon} className='size-mini-Avatar' alt="" />
                                            Chỉnh sửa
                                        </Button>
                                        <Button variant='light' className='setting-button'>
                                            <img src={TaskRemoveIcon} className='size-mini-Avatar' alt="" />
                                            Xóa công việc
                                        </Button>
                                    </div>
                                )}

                            </div>
                        </div>
                    </Col>
                </Row>
            </Modal.Body>
        </Modal>
    </>);

}
export default TaskDetailDialog;