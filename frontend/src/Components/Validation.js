import React, { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { ValidateSignUp } from "../Services/userService";
import { useRef } from "react";
import { useState } from "react";
import { RenewToken } from "../Services/userService";
import { Spinner } from "react-bootstrap";
const Validation = () => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const token = queryParams.get("token");
  const navigate = useNavigate();

  const isApiCalledRef = useRef(false);
  const [showGetNewTokenPage, setGetNewTokenPage] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (token && !isApiCalledRef.current) {
      isApiCalledRef.current = true;


      validateToken();
    }
  }, [token, navigate]);

  const validateToken = async () => {
    try {
      let res = await ValidateSignUp(token);
      if (res) {
        navigate("/");
      }
    } catch (error) {
      if (error.response && error.response.status === 400) {
        setGetNewTokenPage(true);
        await RenewToken(token);
      } else if (error.response && error.response.status === 404) {
        return;
      }
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
      <Spinner animation="border" variant="primary" />
    </div>;
  }

  if (showGetNewTokenPage) {
    // Trả về trang chứa form và nút submit khi API gọi lỗi
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

        <div className="d-flex flex-column justify-content-center align-items-center" style={{ height: '100vh' }}>

          <h1 style={{ fontSize: '1.1em', marginTop: '0px', marginBottom: '100px', textAlign: "center" }}>
            <img width="48" height="48" src="https://img.icons8.com/color/48/zalo.png" alt="zalo" />
            <br />
            Mã xác nhận của bạn đã hết hạn và bạn đã được cấp lại mã mới vui lòng kiểm tra lại Gmail bạn vừa đăng kí để nhận mã xác nhận mới.

          </h1>

        </div>
      </div>
    );
  }
  return <div
    className="d-flex justify-content-center align-items-center"
    style={{
      height: '100vh',
      backgroundImage: 'url("data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' viewBox=\'0 0 1440 810\' preserveAspectRatio=\'xMinYMin slice\'%3E%3Cpath fill=\'%23aad6ff\' d=\'M592.66 0c-15 64.092-30.7 125.285-46.598 183.777C634.056 325.56 748.348 550.932 819.642 809.5h419.672C1184.518 593.727 1083.124 290.064 902.637 0H592.66z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M545.962 183.777c-53.796 196.576-111.592 361.156-163.49 490.74 11.7 44.494 22.8 89.49 33.1 134.883h404.07c-71.294-258.468-185.586-483.84-273.68-625.623z\'/%3E%3Cpath fill=\'%23cee7ff\' d=\'M153.89 0c74.094 180.678 161.088 417.448 228.483 674.517C449.67 506.337 527.063 279.465 592.56 0H153.89z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M153.89 0H0v809.5h415.57C345.477 500.938 240.884 211.874 153.89 0z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M1144.22 501.538c52.596-134.583 101.492-290.964 134.09-463.343 1.2-6.1 2.3-12.298 3.4-18.497 0-.2.1-.4.1-.6 1.1-6.3 2.3-12.7 3.4-19.098H902.536c105.293 169.28 183.688 343.158 241.684 501.638v-.1z\'/%3E%3Cpath fill=\'%23eef4f8\' d=\'M1285.31 0c-2.2 12.798-4.5 25.597-6.9 38.195C1321.507 86.39 1379.603 158.98 1440 257.168V0h-154.69z\'/%3E%3Cpath fill=\'%23e8f3ff\' d=\'M1278.31,38.196C1245.81,209.874 1197.22,365.556 1144.82,499.838L1144.82,503.638C1185.82,615.924 1216.41,720.211 1239.11,809.6L1439.7,810L1439.7,256.768C1379.4,158.78 1321.41,86.288 1278.31,38.195L1278.31,38.196z\'/%3E%3C/svg%3E")',
      backgroundSize: 'cover',
      backgroundRepeat: 'no-repeat',
      backgroundPosition: 'center',
    }}
  >

    <div className="d-flex flex-column justify-content-center align-items-center" style={{ height: '100vh' }}>

      <h1 style={{ fontSize: '1.1em', marginTop: '0px', marginBottom: '100px', textAlign: "center" }}>
        <img width="48" height="48" src="https://img.icons8.com/color/48/zalo.png" alt="zalo" />
        <br />
        404 - ERROR. Bạn đã xác nhận thành công hãy thủ đăng nhập.
      </h1>

    </div>
  </div>
};

export default Validation;
