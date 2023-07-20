import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import React, { Component } from "react";
import Alert from 'react-bootstrap/Alert';

class EmailAuthentication extends Component {
  constructor(props) {
    super(props);
    this.state = {
      input: '',
      showError: false
    };
  }
  handleSubmit = (e) => {
    e.preventDefault();
    if (this.state.input.trim() === '') {
      this.setState({ showError: true });
    } else {
      // Handle form submission
    }
  }

  handleChange = (e) => {
    this.setState({ input: e.target.value, showError: false });
  }

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
            <p className="row justify-content-center">
              Vui lòng kiểm tra email để tiến hành đăng nhập.

            </p>

            <form onSubmit={this.handleSubmit}>

              <InputGroup hasValidation>
                <InputGroup.Text>Mã xác nhận</InputGroup.Text>
                <Form.Control type="text" isInvalid={this.state.showError} onChange={this.handleChange} />


              </InputGroup>
              

              <button type="submit" className="justify-content-center btn btn-primary mt-3">
                Submit
              </button>
             
            </form>
            <div style={{ height: '2rem' }}>
            
            {this.state.showError && (
                <div className="row justify-content-center mt-3">
                  <div className="alert alert-warning" role="alert">
                    Vui lòng nhập mã xác nhận tài khoản để tiếp tục
                  </div>
                </div>
              )}
              </div>
          </div>
        </div>
      </div>

    );
  }
}


export default EmailAuthentication;
