import React, { Component } from 'react';
import { teamsGetAll } from '../../services/teams';
import { Grid, Table, Button, Segment, List, Divider } from 'semantic-ui-react';
import TeamRow from './TeamRow';

export class Teams extends Component {
  state = {
    teams: [],
    loading: true,
    newTeam: null,
  };

  teamAll = () => {
    this.setState({ loading: true });
    teamsGetAll().then(data => {
      this.setState({ teams: data.map(p => ({ ...p, isExist: true })), loading: false });
    }).catch(c => console.log(c));
  }

  componentDidMount() {
    this.teamAll();
  }

  handleRemove = id => () => {
    const { teams, newTeam } = this.state;
    this.setState({
      teams: teams.filter(p => p.id !== id),
      newTeam: newTeam === id ? null : newTeam,
    });
  }

  handleSave = id => () => {
    const { newTeam } = this.state;

    this.setState({
      newTeam: newTeam === id ? null : newTeam,
    });

    this.teamAll();
  }

  handleAdd = () => {
    const { teams } = this.state;

    this.setState({
      teams: [{ id: teams.length, name: '', isExist: false }].concat(teams),
      newTeam: teams.length,
    });
  }

  render() {
    const { loading, teams, newTeam } = this.state;
    const contents = loading ? null : teams;

    return (
      <Segment loading={loading} basic>
        <Grid container>
          <Grid.Column>
            <List>
              <List.Item>
                <List.Header>Спикеры конференции</List.Header>
                <List.Description>Те, на кого ставят</List.Description>
              </List.Item>
            </List>
            <Grid.Row>
              <Button content="Добавить"
                color='blue'
                onClick={this.handleAdd}
                disabled={!!newTeam} />
              <Divider />
            </Grid.Row>
            <Table celled basic='very'>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell width={14} content="Номинант" />
                  <Table.HeaderCell singleLine width={2} />
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {(contents && contents.map(item => (
                  <TeamRow id={item.id} name={item.name}
                    isExist={item.isExist}
                    handleRemove={this.handleRemove(item.id)}
                    handleSave={this.handleSave(item.id)} />
                )))}
              </Table.Body>
            </Table>
          </Grid.Column>
        </Grid>
      </Segment>
    );
  }
}
