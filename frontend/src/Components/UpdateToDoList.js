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
import { GetTaskByTaskIdAPI } from '../Services/toDoListService';

function UpdateToDoList(props) {

    useEffect(() => {
        const fetchData = () => {

        };

        fetchData();
    }, []);




    // delay to load task 


    return (
        <Modal show={props.showModal} onHide={props.handleClose} size="lg">


        </Modal>
    );

}
export default UpdateToDoList;