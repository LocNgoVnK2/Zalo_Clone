import React, { useState } from "react";
import SidebarMenu from "react-bootstrap-sidebar-menu";
import UserAvatar from "./assets/friends.png";
import ChatIcon from "./assets/icon/chatIcon.png";
import PhonebookIcon from "./assets/icon/phonebookIcon.png";
import TodoIcon from "./assets/icon/todoIcon.png";
import ToolboxIcon from "./assets/icon/toolboxIcon.png";
import SettingIcon from "./assets/icon/settingIcon.png";
import { HomeState } from "./Home";
import OverlayTrigger from "react-bootstrap/OverlayTrigger";
import Popover from "react-bootstrap/Popover";
import UserProfileDialog from "./UserProfileDialog";

function Sidebar(props) {
  const [showDialog, setShowDialog] = useState(false);
  const [showPopover, setShowPopover] = useState(true);
  const [avatarImage, setAvatarImage] = useState(
    "data:image/jpeg;base64," + props.user.avatar
  );

  const handleSignOut = () => {
    localStorage.removeItem("token");
    props.selectionChange("default");
    props.navigate("/");
  };

  const handleOpenDialog = () => {
    setShowDialog(true);
    setShowPopover(false);
  };

  const handleCloseDialog = () => {
    setShowDialog(false);
    setShowPopover(true);
  };

  const popover = (
    <Popover id="popover-basic" className="custom-popover">
      <Popover.Header as="h3">
        <strong>{props.user.userName}</strong>
      </Popover.Header>
      <Popover.Body>
        <span className="popover-body" onClick={handleOpenDialog}>
          Hồ sơ của bạn
        </span>
        <span className="popover-body">Cài đặt</span>
      </Popover.Body>
      <Popover.Header as="h3" onClick={handleSignOut}>
        Đăng xuất
      </Popover.Header>
    </Popover>
  );

  const popoverForSetting = (
    <Popover id="popover-basic" className="custom-popover">
      <Popover.Header as="h3">
        <span className="popover-body" onClick={handleOpenDialog}>
          Hồ sơ của bạn
        </span>
      </Popover.Header>
      <Popover.Body>
        <span className="popover-body" onClick={handleSignOut}>
          Đăng xuất
        </span>
      </Popover.Body>
    </Popover>
  );

  return (
    <>
      <UserProfileDialog
        show={showDialog}
        handleClose={handleCloseDialog}
        user={props.user}
      />
      <SidebarMenu
        variant="pills"
        onSelect={props.changeState}
        activeKey="1"
        className="sidebar-container"
      >
        <SidebarMenu.Body className="sidebar-body">
          <div className="sidebar-tabs-top">
            <SidebarMenu.Nav className="sidebar-tabs sidebar-avatar">
              {showPopover && (
                <OverlayTrigger
                  trigger="click"
                  placement="right"
                  overlay={popover}
                >
                  <img
                    src={props.user.avatar ? avatarImage : UserAvatar}
                    className="sidebar-avatar-image"
                    alt=""
                  />
                </OverlayTrigger>
              )}
              {!showPopover && (
                <img
                  src={props.user.avatar ? avatarImage : UserAvatar}
                  className="sidebar-avatar-image"
                  alt=""
                />
              )}
            </SidebarMenu.Nav>
            <SidebarMenu.Nav.Link
              eventKey={HomeState.Message}
              className="sidebar-tabs"
              onClick={() => {
                props.selectionChange("default");
                props.changeState(HomeState.Message);
              }}
            >
              <img src={ChatIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
            <SidebarMenu.Nav.Link
              eventKey={HomeState.None}
              className="sidebar-tabs"
              onClick={() => props.selectionChange("phonebook")}
            >
              <img src={PhonebookIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
            <SidebarMenu.Nav.Link
              eventKey="3"
              className="sidebar-tabs"
              onClick={() => props.selectionChange("todolist")}
            >
              <img src={TodoIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
          </div>
          <div className="sidebar-tabs-bottom">
            <SidebarMenu.Nav.Link eventKey="4" className="sidebar-tabs">
              <img src={ToolboxIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
            <SidebarMenu.Nav.Link eventKey="5" className="sidebar-tabs">
              {showPopover && (
                <OverlayTrigger
                  trigger="click"
                  placement="right"
                  overlay={popoverForSetting}
                >
                  <img src={SettingIcon} className="icon" alt="" />
                </OverlayTrigger>
              )}
              {!showPopover && (
                <img src={SettingIcon} className="icon" alt="" />
              )}
            </SidebarMenu.Nav.Link>
          </div>
        </SidebarMenu.Body>
      </SidebarMenu>
    </>
  );
}

export default Sidebar;
