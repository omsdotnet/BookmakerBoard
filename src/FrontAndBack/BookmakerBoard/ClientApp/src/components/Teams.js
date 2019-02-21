import React, { Component } from 'react';

export class Teams extends Component {
  displayName = Teams.name

  constructor(props) {
    super(props);
    this.state = { teams: [], loading: true };

    fetch('api/Teams/GetAll')
      .then(response => response.json())
      .then(data => {
        this.setState({ teams: data, loading: false });
      });
  }

  static renderTable(teams) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Название</th>
          </tr>
        </thead>
        <tbody>
          {teams.map(teams =>
            <tr key={teams.id}>
              <td>{teams.name}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Загрузка...</em></p>
      : Teams.renderTable(this.state.teams);

    return (
      <div>
        <h1>Участники заездов</h1>
        <p>Команды, на которые делают ставки</p>
        {contents}
      </div>
    );
  }
}
