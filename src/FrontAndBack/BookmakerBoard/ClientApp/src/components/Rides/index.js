import React, { Component } from 'react';
import { ridesGetAll, ridesCreate, ridesPut, ridesDelete } from '../../services/rides';
import { biddersGetAll } from '../../services/bidders';
import { teamsGetAll } from '../../services/teams';
import {
  Segment, Grid, List, Button,
  Divider, Table, Container,
  Dropdown,
  Header,
} from 'semantic-ui-react';
import RidesRow from './RidesRow';

export class Rides extends Component {
  state = {
    ridesList: [],
    rides: {},
    currentRide: 0,
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

        const rides = response[0];
        const ride = rides[0];

        this.setState({
          bidders,
          teams,
          loading: false,
          ridesList: rides,
          currentRide: ride.number
        });
      }).catch((err) => {
        console.log(err);
        this.setState({ loading: false });
      });
  }

  getRides = (isDefault = false) => {
    this.setState({ loading: true });
    ridesGetAll()
      .then((response) => {
        const ride = !isDefault ? response.find(p => p.id === this.state.currentRide) : {};
        this.setState({
          rides: {
            ...ride,
            rates: !isDefault ? ride.rates.map(p => ({ ...p, isExist: true })) : null,
          }, 
          loading: false,
          ridesList: !isDefault ? this.state.ridesList : response,
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

  handleRide = () => {
    const { ridesList } = this.state;

    const newID = ridesList.length;
    const ride = {
      id: newID,
      number: newID,
      winnerTeams: [],
      rates: []
    };

    this.setState({ loading: true });
    ridesCreate(ride)
      .then(() => {
        this.setState({
          rides: ride,
          loading: false,
          ridesList: [...ridesList, ...[ride]],
          currentRide: newID,
        });
      }).catch((err) => {
        console.log(err);
        this.setState({ loading: false });
      });
  }

  handleRideChange = (_, { value }) => {
    const { ridesList } = this.state;
    const ride = ridesList.find(p => p.id === value);
    this.setState({
      currentRide: ride ? ride.number : 0,
      rides: {
        ...ride,
        rates: ride.rates ? ride.rates.map(p => ({ ...p, isExist: true })) : []
      },
    });
  }

  handleWinnersChange = (_, { value }) => {
    const { rides } = this.state;
    this.setState({
      rides: {
        ...rides,
        winnerTeams: value,
      }
    });
  }

  handleSaveRide = () => {
    const { rides, ridesList } = this.state;

    this.setState({ loading: true });
    ridesPut(rides).then(() => {
      this.setState({
        loading: false,
        ridesList: ridesList.map((p, i) => p.id === rides ? rides : p),
      });
    }).catch(() => {
      this.setState({ loading: false });
    });
  }

  handleDeleteRide = () => {
    const { rides, ridesList } = this.state;

    this.setState({ loading: true });
    ridesDelete(rides.id)
      .then(() => {
        this.setState({ loading: false, ridesList: ridesList.filter(p => p.id !== rides.id) });
        this.getRides(true);
      }).catch(() => {
        this.setState({ loading: false });
      });
  }

  render() {
    const { loading, rides,
      newRateId, bidders,
      ridesList,
      currentRide,
      teams } = this.state;

    const contents = loading
      ? null
      : rides;

    const isRide = (contents && contents.rates) || false;
    const ridesOptions = ridesList.map((p, key) => ({ key, value: p.id, text: p.number }));
    const biddersOptions = bidders.map((p, key) => ({ key, value: p.id, text: p.name }));
    const teamsOptions = teams.map((p, key) => ({ key, value: p.id, text: p.name }));
    const wins = rides.winnerTeams;

    return (
      <Segment loading={loading} basic>
        <Container>
          <List>
            <List.Item>
              <List.Header>Заезды</List.Header>
              <List.Description>Здесь регистрируем заезды и ставки</List.Description>
            </List.Item>
          </List>
          <Divider />
          <Button content="Создать заезд"
            onClick={this.handleRide} />
          <span style={{ marginRight: '25px' }} />
          <Button content="Создать ставку"
            color='blue'
            onClick={this.handleCreateRate}
            disabled={!isRide ? true : newRateId !== null} />
          <span style={{ marginRight: '25px' }} />
          <Button content="Сохранить заезд"
            color='red'
            onClick={this.handleSaveRide}
            disabled={!isRide} />
          <Button content="Удалить"
            color='red'
            onClick={this.handleDeleteRide}
            disabled={!isRide} />
          <Divider />
          <List horizontal>
            <List.Item>
              <Header>
                {`Номер заезда:  `}
                <Header.Content>
                  <Dropdown fluid
                    options={ridesOptions}
                    value={currentRide}
                    onChange={this.handleRideChange} />
                </Header.Content>
              </Header>
            </List.Item>
            <List.Item>
              <Dropdown fluid header="Список победителей:"
                basic
                multiple
                selection
                disabled={!isRide}
                options={teamsOptions}
                value={wins}
                onChange={this.handleWinnersChange} />
            </List.Item>
          </List>
          <Divider />
          <Grid container>
            <Grid.Column>
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
                      ride={contents}
                      handleRemove={this.handleRemove(item.id)}
                      handleSave={this.handleSave(item.id)} />
                  ))}
                </Table.Body>
              </Table>
            </Grid.Column>
          </Grid>
        </Container>
      </Segment>
    );
  }
}
