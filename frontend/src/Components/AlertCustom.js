import React, { Component } from 'react';
import Alert from 'react-bootstrap/Alert';

class AlertCustom extends Component {
  constructor(props) {
    super(props);
    this.state = {
      showAlert: true
    };
  }

  componentDidMount() {
    const { dismissTime } = this.props;
    setTimeout(() => {
      this.setState({ showAlert: false });
    }, dismissTime);
  }

  render() {
    const { message, variant } = this.props;
    const { showAlert } = this.state;

    return (
      showAlert && (
        <Alert variant={variant} onClose={() => this.setState({ showAlert: false })} dismissible>
          {message}
        </Alert>
      )
    );
  }
}

export default AlertCustom;