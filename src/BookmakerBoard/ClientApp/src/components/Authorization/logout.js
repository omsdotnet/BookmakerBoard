import React, { Component } from 'react';
import { logout } from '../../services/authentication';


export class Logout extends Component
{
  handleLogout = async () =>
  {
    await logout().then(() =>
    {
      this.props.CallBack();
      this.props.Switch();
    });
  }

  render()
  {
    return (
      <div className="column">
        <h2 className="ui teal image header">
          <div className="content">
            Подтвердите действие. Доступ к разделам: Заезды, Команды, Участники для не аутентифицированных пользователей закрыт.
            </div>
        </h2>
        <form className="ui large form">
          <div className="ui stacked segment">            
            <div className="ui fluid large teal submit button" onClickCapture={this.handleLogout}>Выйти</div>
          </div>
        </form>
      </div>
    );
  }
}