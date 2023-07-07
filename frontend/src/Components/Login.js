import React, { Component } from 'react';


class Login extends Component {
    constructor(props){
        super(props);
        this.state={
            responseData: null,
            error: null
        }
    }

    componentDidMount() {
        this.postData("https://localhost:7009/api/User/signin", {
          email: 'GojoSensi@example.com',
          password: 'pass@Aword123'
        }).then((data) => {
          this.setState({responseData:data})
        }).catch((error) => {
          this.setState({error:error.message})
        });
      }
    
      async postData(url, data) {
        try {
          const response = await fetch(url, {
            method: "POST",
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
          });
    
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
    
          return response.json();
        } catch (error) {
          console.error('Error:', error);
        }
      }
  render() {
  

    return (
      <div>
      {this.state.error && <div>Error: {this.state.error}</div>}
      
      
      <pre>{JSON.stringify(this.state.responseData, null, 2)}</pre>
      
    </div>
    );
  }
}

export default Login;