import React, { Component } from 'react';

export class Bidders extends Component {
  displayName = Bidders.name

  constructor(props) {
    super(props);
    this.state = { bidders: [], loading: true };

    fetch('api/Bidders/GetAll')
      .then(response => response.json())
      .then(data => {
        this.setState({ bidders: data, loading: false });
      });
  }

  static renderTable(bidders) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Имя</th>
            <th>Начальный счет</th>
            <th>Текущий счет</th>
          </tr>
        </thead>
        <tbody>
          {bidders.map(bidders =>
            <tr key={bidders.id}>
              <td>{bidders.name}</td>
              <td>{bidders.startScore}</td>
              <td>{bidders.currentScore}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Загрузка...</em></p>
      : Bidders.renderTable(this.state.bidders);

    return (
      <div>
        <h1>Участники ставок</h1>
        <p>Люди, которые делают ставки</p>
        {contents}
      </div>
    );
  }
}
