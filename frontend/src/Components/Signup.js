import React, { Component } from "react";
import Col from 'react-bootstrap/Col';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import { signupApi } from "../Services/userService";
import AlertCustom from "./AlertCustom";
import Swal from "sweetalert2";
import SweetAlert2 from 'react-bootstrap-sweetalert';
class Signup extends Component {
    constructor(props) {
        super(props);
        this.state = {
            responseData: null,
            error: null,
            email: '',
            password: '',
            phonenumber: '',
            username: '',
            gender: '',
            dob: '',
            showAlert: false,

        }
        this.handleEmailChange = this.handleEmailChange.bind(this);
        this.handlePasswordChange = this.handlePasswordChange.bind(this);
        this.handleUserNameChange = this.handleUserNameChange.bind(this);
        this.handlePhoneNumberChange = this.handlePhoneNumberChange.bind(this);
        this.handleGenderChange = this.handleGenderChange.bind(this);
        this.handleDOBChange = this.handleDOBChange.bind(this);
    }


    jumpToSignIn = () => {
        this.props.navigate("/");
    }

    handleEmailChange(event) {
        this.setState({ email: event.target.value });
    }
    handlePasswordChange(event) {
        this.setState({ password: event.target.value });
    }
    handleUserNameChange(event) {
        this.setState({ username: event.target.value });
    }
    handlePhoneNumberChange(event) {
        this.setState({ phonenumber: event.target.value });
    }
    handleGenderChange(event) {
        this.setState({ gender: event.target.value });
    }
    handleDOBChange(event) {
        this.setState({ dob: event.target.value });
    }
    handleSignup = async () => {
        if (!this.state.email || !this.state.password || !this.state.username || !this.state.phonenumber || !this.state.gender || !this.state.dob) {
            alert("Please fill in all fields");
            return;
        }
        try {
            let res = await signupApi(this.state.email, this.state.password, this.state.username, this.state.phonenumber, this.state.gender, this.state.dob);
            if (res) {

                this.setState({ showAlert: true }, () => {
                    setTimeout(() => {
                        this.setState({ showAlert: false });
                    }, 60000);
                });

            }
        } catch (error) {
            alert("An error occurred");
            return false;
        }
    };

    render() {
        return (

            <div
                className="d-flex justify-content-center align-items-center"
                style={{
                    height: '100vh',
                    backgroundImage: 'url("data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' viewBox=\'0 0 1440 810\' preserveAspectRatio=\'xMinYMin slice\'%3E%3Cpath fill=\'%23aad6ff\' d=\'M592.66 0c-15 64.092-30.7 125.285-46.598 183.777C634.056 325.56 748.348 550.932 819.642 809.5h419.672C1184.518 593.727 1083.124 290.064 902.637 0H592.66z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M545.962 183.777c-53.796 196.576-111.592 361.156-163.49 490.74 11.7 44.494 22.8 89.49 33.1 134.883h404.07c-71.294-258.468-185.586-483.84-273.68-625.623z\'/%3E%3Cpath fill=\'%23cee7ff\' d=\'M153.89 0c74.094 180.678 161.088 417.448 228.483 674.517C449.67 506.337 527.063 279.465 592.56 0H153.89z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M153.89 0H0v809.5h415.57C345.477 500.938 240.884 211.874 153.89 0z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M1144.22 501.538c52.596-134.583 101.492-290.964 134.09-463.343 1.2-6.1 2.3-12.298 3.4-18.497 0-.2.1-.4.1-.6 1.1-6.3 2.3-12.7 3.4-19.098H902.536c105.293 169.28 183.688 343.158 241.684 501.638v-.1z\'/%3E%3Cpath fill=\'%23eef4f8\' d=\'M1285.31 0c-2.2 12.798-4.5 25.597-6.9 38.195C1321.507 86.39 1379.603 158.98 1440 257.168V0h-154.69z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M1278.31,38.196C1245.81,209.874 1197.22,365.556 1144.82,499.838L1144.82,503.638C1185.82,615.924 1216.41,720.211 1239.11,809.6L1439.7,810L1439.7,256.768C1379.4,158.78 1321.41,86.288 1278.31,38.195L1278.31,38.196z\'/%3E%3C/svg%3E")',
                    backgroundSize: 'cover',
                    backgroundRepeat: 'no-repeat',
                    backgroundPosition: 'center',
                }}
            >

                <div className="container">
                    <div className="row justify-content-center">
                        {this.state.showAlert && (
                            <SweetAlert2
                                success
                                title="Đăng ký thành công vui lòng kiểm tra Gmail để sang trang xác thực"
                                onConfirm={() => {
                                    this.setState({ showAlert: false });
                                }}
                                timeout={1500}
                                show={true}
                            />
                        )}
                        <div className="col-lg-6 col-md-8 col-sm-12">
                            <h1 style={{ fontSize: '1.1em', marginTop: '0px', marginBottom: '100px', textAlign: "center" }}>
                                <img width="48" height="48" src="https://img.icons8.com/color/48/zalo.png" alt="zalo" />
                                <br />
                                <strong> Đăng Ký tài khoản Zalo </strong> <br />
                                để kết nối với ứng dụng Zalo Web
                            </h1>
                            <div className="card p-4">
                                <Form>
                                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextUserName">
                                        <Form.Label column xs="4" sm="4">
                                            Tên người dùng
                                        </Form.Label>
                                        <Col xs="8" sm="8">
                                            <Form.Control type="text"
                                                placeholder="User Name"
                                                value={this.state.username}
                                                onChange={this.handleUserNameChange} />
                                        </Col>
                                    </Form.Group>

                                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextEmail">
                                        <Form.Label column xs="3" sm="3">
                                            Email
                                        </Form.Label>
                                        <Col xs="9" sm="9">
                                            <Form.Control type="email"
                                                placeholder="email@example.com"
                                                value={this.state.email}
                                                onChange={this.handleEmailChange} />
                                        </Col>
                                    </Form.Group>

                                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextPassword">
                                        <Form.Label column xs="3" sm="3">
                                            Mật khẩu
                                        </Form.Label>
                                        <Col xs="9" sm="9">
                                            <Form.Control type="password"
                                                placeholder="Password"
                                                value={this.state.password}
                                                onChange={this.handlePasswordChange} />
                                        </Col>
                                    </Form.Group>

                                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextPhoneNumber">
                                        <Form.Label column xs="5" sm="5">
                                            Số điện thoại
                                        </Form.Label>
                                        <Col xs="7" sm="7">
                                            <Form.Control type="tel"
                                                placeholder="Phone Number"
                                                value={this.state.phonenumber}
                                                onChange={this.handlePhoneNumberChange} />
                                        </Col>
                                    </Form.Group>

                                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextGender">
                                        <Form.Label column xs="3" sm="3">
                                            Giới tính
                                        </Form.Label>
                                        <Col xs="9" sm="9">
                                            <Form.Check
                                                type="radio"
                                                name="gender"
                                                id="genderMale"
                                                label="Nam"
                                                value="nam"
                                                className="me-3"
                                                onChange={this.handleGenderChange}
                                            />
                                            <Form.Check
                                                type="radio"
                                                name="gender"
                                                id="genderFemale"
                                                label="Nữ"
                                                value="nu"
                                                className="me-3"
                                                onChange={this.handleGenderChange}
                                            />
                                        </Col>
                                    </Form.Group>
                                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextDOB">
                                        <Form.Label column xs="4" sm="4">
                                            Ngày sinh
                                        </Form.Label>
                                        <Col xs="8" sm="8">
                                            <Form.Control type="date"
                                                placeholder="Date of Birth"
                                                value={this.state.dob}
                                                onChange={this.handleDOBChange} />
                                        </Col>
                                    </Form.Group>
                                    <div className="d-grid">
                                        <Button variant="primary" type="button" onClick={this.handleSignup}  >
                                            Đăng ký
                                        </Button>
                                    </div>

                                </Form>

                            </div>
                            <br />
                            <div className="d-flex justify-content-center">
                                <Button variant="primary" type="button" onClick={this.jumpToSignIn}>
                                    Trở về
                                </Button>
                            </div>

                        </div>
                    </div>
                </div>


            </div>
        );

    }
}

export default Signup;