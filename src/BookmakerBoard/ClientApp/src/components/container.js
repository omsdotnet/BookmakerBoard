import React from 'react';
import { Menu, Segment } from 'semantic-ui-react';
import { MENU_CONTAINER_ITEMS, MENU_KEYS } from './constants';
import { Home } from './Summary/index';
import { Teams } from './Teams';
import { Bidders } from './Bidders';
import { Rides } from './Rides/index';
import { Login } from './Authorization/Login';
import { Logout } from './Authorization/logout';
import { isSignIn } from '../services/authentication';

const fixedMenuStyle = {
  backgroundColor: '#fff',
  border: '1px solid #ddd',
  boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.2)',
}

class Container extends React.Component
{
  state = {
    itemsMenu: MENU_CONTAINER_ITEMS
  };

  componentDidMount()
  {
    this.isloggedIn();
  }

  handlerMenuClick = (_, data) =>
  {
    this.setState((prev) =>
    {
      return {
        itemsMenu: prev.itemsMenu.map(item => ({ ...item, active: data.index === item.index }))
      }
    });
  }

  disableMenuItem = (data) =>
  {
    this.setState((prev) =>
    {
      return {
        itemsMenu: prev.itemsMenu.map(item => (
          {
            ...item,
            disabled: !data && [1, 2, 3].some(index => index === item.index)
          }))
      };
    });
  }

  isloggedIn = async () =>
  {
    await isSignIn().then(data =>
    {
      this.disableMenuItem(data);
    }).catch(() =>
    {
      this.disableMenuItem(true);
    });
  }

  switch = () =>
  {
    this.state.itemsMenu.find(p => p.key === 1).active = true;
    var loginButton = this.state.itemsMenu.find(p => p.key === 4);
    loginButton.name = loginButton.name === MENU_KEYS.LOGIN ? MENU_KEYS.LOGOUT : MENU_KEYS.LOGIN;
  }

  render()
  {
    const activeMenu = this.state.itemsMenu.find(p => p.active);
    return (
      <>
        <Menu pointing
          secondary
          borderless
          fixed="top"
          style={fixedMenuStyle}
          items={this.state.itemsMenu}
          onItemClick={this.handlerMenuClick} />
        {activeMenu.name === MENU_KEYS.SUMMARY && (
          <Segment vertical padded='very'>
            <Home />
          </Segment>
        )}
        {activeMenu.name === MENU_KEYS.RIDES && (
          <Segment vertical padded='very'>
            <Rides />
          </Segment>
        )}
        {activeMenu.name === MENU_KEYS.TEAMS && (
          <Segment vertical padded='very'>
            <Teams />
          </Segment>
        )}
        {activeMenu.name === MENU_KEYS.PARTICIPANTS && (
          <Segment vertical padded='very'>
            <Bidders />
          </Segment>
        )}
        {activeMenu.name === MENU_KEYS.LOGIN && (
          <Segment vertical padded='very' className="ui middle aligned center aligned grid" >
            <Login CallBack={this.isloggedIn} Switch={this.switch} />
          </Segment>
        )}
        {activeMenu.name === MENU_KEYS.LOGOUT && (
          <Segment vertical padded='very' className="ui middle aligned center aligned grid" >
            <Logout CallBack={this.isloggedIn} Switch={this.switch} />
          </Segment>
        )}
      </>
    );
  }
}

export default Container;