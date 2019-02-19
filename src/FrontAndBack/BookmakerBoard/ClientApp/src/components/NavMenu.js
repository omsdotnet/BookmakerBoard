import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to={'/'}>BookmakerBoard</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            <LinkContainer to={'/'} exact>
              <NavItem>
                <Glyphicon glyph='list' /> Результаты
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/rides'}>
              <NavItem>
                <Glyphicon glyph='knight' /> Заезды
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/teams'}>
              <NavItem>
                <Glyphicon glyph='asterisk' /> Команды
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/bidders'}>
              <NavItem>
                <Glyphicon glyph='user' /> Участники
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/login'}>
              <NavItem>
                <Glyphicon glyph='lock' /> Войти
              </NavItem>
            </LinkContainer>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
