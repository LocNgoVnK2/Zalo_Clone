import React, { Component } from "react";

import SidebarMenu from "react-bootstrap-sidebar-menu";
import UserAvatar from "./assets/test.png";
import ChatIcon from "./assets/icon/chatIcon.png";
import PhonebookIcon from "./assets/icon/phonebookIcon.png";
import TodoIcon from "./assets/icon/todoIcon.png";
import ToolboxIcon from "./assets/icon/toolboxIcon.png";
import SettingIcon from "./assets/icon/settingIcon.png";
import {Message, None} from "./Home";

class Sidebar extends Component {



  render() {

    return (
      <>
       <SidebarMenu variant="pill" onSelect={this.props.changeState} activeKey="1"  className="sidebar-container">
        <SidebarMenu.Body  className="sidebar-body">
          <div className="sidebar-tabs-top">
            <SidebarMenu.Nav className="sidebar-tabs sidebar-avatar">
              <img src={UserAvatar} className="sidebar-avatar-image" alt="" />
            </SidebarMenu.Nav>
            <SidebarMenu.Nav.Link eventKey={Message} className="sidebar-tabs">
                <img src={ChatIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
            <SidebarMenu.Nav.Link eventKey={None} className="sidebar-tabs">
              <img src={PhonebookIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
            <SidebarMenu.Nav.Link eventKey="3" className="sidebar-tabs">
              <img src={TodoIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
          </div>
          <div className="sidebar-tabs-bottom">
            <SidebarMenu.Nav.Link eventKey="4" className="sidebar-tabs">
              <img src={ToolboxIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
            <SidebarMenu.Nav.Link eventKey="5" className="sidebar-tabs">
              <img src={SettingIcon} className="icon" alt="" />
            </SidebarMenu.Nav.Link>
          </div>
        </SidebarMenu.Body>
      </SidebarMenu>
      </>
     
    );
  }
}

export default Sidebar;
