﻿import React, { Component } from 'react';

export class Counter extends Component {
  displayName = Counter.name

  constructor(props) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }

  render() {
    return (
      <div>
        <h1>Пример счетчика</h1>

        <p>Жмешь кнопарь, счетчик считает</p>

        <p>Current count: <strong>{this.state.currentCount}</strong></p>

        <button onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}
