import React, { Component } from 'react';
import { ridesGetAll } from '../../services/rides';
import { biddersGetAll } from '../../services/bidders';
import { teamsGetAll } from '../../services/teams';
import { Segment, Grid, List, Button, Divider, Table } from 'semantic-ui-react';
import RidesRow from './RidesRow';

export class Rides extends Component {
  state = {
    rides: {},
    bidders: [],
    teams: [],
    loading: true,
    newRateId: null,
  };

  componentDidMount() {
    this.setState({ loading: true });
    Promise.all([ridesGetAll(), biddersGetAll(), teamsGetAll()])
      .then(response => {
        const bidders = response[1];
        const teams = response[2];
        const rides = response[0][9];

        this.setState({
          rides: {
            ...rides,
            rates: rides && rides.rates ? rides.rates.map(p => ({ ...p, isExist: true })) : []
          }, bidders, teams, loading: false
        });
      }).catch((err) => {
        console.log(err);
        this.setState({ loading: false });
      });
  }

  getRides = () => {
    this.setState({ loading: true });
    ridesGetAll()
      .then((response) => {
        this.setState({
          rides: {
            ...response,
            rates: response.rates.map(p => ({ ...p, isExist: true }))
          }, loading: false
        });
      }).catch((err) => {
        console.log(err);
        this.setState({ loading: false });
      });
  }

  handleCreateRate = () => {
    const { rides } = this.state;

    this.setState({
      rides: {
        ...rides,
        rates: [{
          id: rides.rates.length,
          isExist: false,
          bidder: {
            id: -1,
            currentScore: 0,
          },
          rateValue: 100,
          team: -1,
        }].concat(rides.rates),
      },
      newRateId: rides.rates.length
    });
  }

  handleRemove = id => () => {
    const { rides, newRateId } = this.state;
    this.setState({
      rides: { ...rides, rates: rides.rates.filter(p => p.id !== id) },
      newRateId: newRateId === id ? null : newRateId,
    });
  }

  handleSave = id => () => {
    const { newRateId } = this.state;
    this.setState({
      newRateId: newRateId === id ? null : newRateId,
    });

    this.getRides();
  }

  render() {
    const { loading, rides, newRateId, bidders, teams } = this.state;
    const contents = loading
      ? null
      : rides;

    return (
      <Segment loading={loading} basic>
        <Grid container>
          <Grid.Column>
            <List>
              <List.Item>
                <List.Header>Заезды</List.Header>
                <List.Description>Здесь регистрируем заезды и ставки</List.Description>
              </List.Item>
            </List>
            <Grid.Row>
              <Button content="Создать ставку"
                color='blue'
                disabled={!!newRateId}
                onClick={this.handleCreateRate} />
              <Button content="Сохранить"
                color='red' />
              <Button content="Отмена"
                color='grey' />
              <Divider />
            </Grid.Row>
            <Table celled basic='very'>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell width={4} content="Участник" />
                  <Table.HeaderCell width={3} content="Очки" />
                  <Table.HeaderCell width={3} content="Ставка" />
                  <Table.HeaderCell width={4} content="Кто победит" />
                  <Table.HeaderCell singleLine width={2} />
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {contents && contents.rates && contents.rates.map(item => (
                  <RidesRow
                    key={item.id}
                    id={item.id}
                    isExist={item.isExist}
                    bidders={bidders}
                    teams={teams}
                    bidderId={item.bidder.id}
                    bidderScore={item.bidder.currentScore}
                    rateScore={item.rateValue}
                    teamWin={item.team}
                    ride={item}
                    handleRemove={this.handleRemove(item.id)}
                    handleSave={this.handleSave(item.id)} />
                ))}
              </Table.Body>
            </Table>
          </Grid.Column>
        </Grid>
      </Segment>
    );
  }
}
