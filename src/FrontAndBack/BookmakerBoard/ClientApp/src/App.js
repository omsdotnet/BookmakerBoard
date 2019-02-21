import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Bidders } from './components/Bidders';
import { Teams } from './components/Teams';
import { Rides } from './components/Rides';
import { Counter } from './components/Counter';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/rides' component={Rides} />
        <Route path='/teams' component={Teams} />
        <Route path='/bidders' component={Bidders} />
        <Route path='/login' component={Counter} />
      </Layout>
    );
  }
}
