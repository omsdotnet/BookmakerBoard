import React, { Component } from 'react';
import { login } from '../../services/authentication';


export class Login extends Component
{
  constructor(props)
  {
    super(props);
    this.state = { name: '', password: '', isLoginError: false, isRedirect: false };
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  handleInputChange(event)
  {
    this.setState({ [event.target.name]: event.target.value });
  }

  handleLogin = async () =>
  {
    const { name, password } = this.state;

    await login(name, password).then(() =>
    {
      this.setState({ isLoginError: false, isRedirect: true });
    }).catch(() =>
    {
      this.setState({ isLoginError: true, isRedirect: false });
    });
  }

  render()
  {
    const { isLoginError } = this.state;

    if (this.state.isRedirect) {
      this.props.CallBack();
      this.props.Switch();
    }
    return (
      <div className="column">
        <h2 className="ui teal image header">
          <div className="content">
            Выполните вход
            </div>
        </h2>
        <form className="ui large form">
          <div className="ui stacked segment">
            <div className="field">
              <div className="ui left icon input">
                <i className="user icon" />
                <input type="text" name="name" placeholder="Имя пользователя..." onChange={this.handleInputChange} />
              </div>
            </div>
            <div className="field">
              <div className="ui left icon input">
                <i className="lock icon" />
                <input type="password" name="password" placeholder="Пароль..." onChange={this.handleInputChange} />
              </div>
            </div>
            <div className="ui fluid large teal submit button" onClickCapture={this.handleLogin}>Войти</div>
          </div>
          <div className="ui error message" hidden={isLoginError} >
            Неверное имя пользователя или пароль!
          </div>
        </form>
      </div>
    );
  }
}