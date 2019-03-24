import React, { Component } from 'react';
import { teamsGetAll, teamDelete, teamAdd } from '../../services/teams';
import { Segment, Grid, Table, List, Button, Input } from 'semantic-ui-react';

export class Teams extends Component {
  state = {
    teams: [],
    loading: true,
    newTeam: false,
    teamName: null,
  };

  teamAll = () => {
    teamsGetAll().then(data => {
      this.setState({ teams: data, loading: false });
    }).catch(c => console.log(c));
  }

  componentDidMount() {
    this.teamAll();
  }

  handleRemove = id => () => {
    teamDelete(id).then(() => {
      this.setState({ loading: true });
      this.teamAll();
    });
  }

  handleAdd = () => {
    const { newTeam, teams, teamName } = this.state;
    if (newTeam && teamName) {
      const length = teams.length;
      const newTeam = { id: length, name: teamName };
      teamAdd({ id: length, name: teamName })
        .then(() => {
          this.setState({
            newTeam: !newTeam,
            teams: [...teams, ...[newTeam]],
            teamName: null
          });
        }).catch(() => console.log('error'));
    } else {
      this.setState({ newTeam: !newTeam });
    }
  }

  handleChange = (_, { value }) => {
    this.setState({ teamName: value });
  }

  render() {
    const contents = this.state.loading
      ? null
      : this.state.teams;

    return (
      <Segment basic>
        <Segment.Inline>
          <List>
            <List.Item>
              <List.Header>Участники заездов</List.Header>
              <List.Description>Команды, на которые делают ставки</List.Description>
            </List.Item>
          </List>
          <Grid container>
            <Grid.Column>
              <Table singleLine basic='very'>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell width="15">
                      {this.state.newTeam ? <Input fluid
                        placeholder="Новая команда"
                        type="input"
                        onChange={this.handleChange} /> : 'Команда'}
                    </Table.HeaderCell>
                    <Table.HeaderCell width="1">
                      <Button color='blue' onClick={this.handleAdd}>
                        {!this.state.newTeam ? 'Добавить' : 'Сохранить'}
                      </Button>
                    </Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {(contents && contents.map(item => (
                    <Table.Row key={item.id}>
                      <Table.Cell>
                        {item.name}
                      </Table.Cell>
                      <Table.Cell>
                        <Button color='black'
                          icon="remove"
                          size="tiny"
                          onClick={this.handleRemove(item.id)} />
                      </Table.Cell>
                    </Table.Row>
                  )))}
                </Table.Body>
              </Table>
            </Grid.Column>
          </Grid>
        </Segment.Inline>
      </Segment>
    );
  }
}
