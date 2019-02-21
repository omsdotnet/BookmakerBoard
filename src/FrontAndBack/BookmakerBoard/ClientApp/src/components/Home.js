﻿import React, { Component } from 'react';

export class Home extends Component {
  displayName = Home.name

  constructor(props) {
    super(props);
    this.state = { rides: [], loading: true };

    fetch('api/Rides/GetAll')
      .then(response => response.json())
      .then(data => {
        this.setState({ rides: data, loading: false });
      });
  }

  static renderTable(rides) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Номер заезда</th>
            <th>Статус</th>
            <th>Ставки</th>
          </tr>
        </thead>
        <tbody>
          {rides.map(rides =>
            <tr key={rides.id}>
              <td>{rides.number}</td>
              <td></td>
              <td></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Загрузка...</em></p>
      : Home.renderTable(this.state.rides);

    return (
      <div>
        <h1>Результаты заездов</h1>
        <p>Лидеры:</p>
        {contents}
      </div>
    );
  }
}
