import React, { Component } from 'react';

export class Rides extends Component {
  displayName = Rides.name

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
      : Rides.renderTable(this.state.rides);

    return (
      <div>
        <h1>Заезды</h1>
        <p>Здесь регистрируем заезды и ставки</p>
        {contents}
      </div>
    );
  }
}
