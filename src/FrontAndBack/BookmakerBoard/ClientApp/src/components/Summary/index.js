import React, { Component, createRef, Fragment } from 'react';
import { ridesGetAll } from '../../services/rides';
import { getTop } from '../../services/bidders';
import { teamsGetAll } from '../../services/teams';
import {
  Grid, Sticky, Ref, List,
  Segment, Header, Divider, Table, Label,
} from 'semantic-ui-react';

export class Home extends Component {
  contextRef = createRef();
  timer = null;
  interval = 1 * 60 * 1000;

  state = {
    rides: [],
    topBidders: [],
    loading: true,
  };

  init = () => {
    getTop().then((data) => {
      this.setState({ topBidders: data });
    }).then(() => {
      Promise.all([ridesGetAll(), teamsGetAll()])
        .then(data => {
          const rides = data[0];
          const teams = data[1];

          const mapRides = rides.map(p => ({
            ...p,
            winnerTeams: p.winnerTeams
              .reduce((result, teamId) => [...result, ...[teams.find(i => i.id === teamId)]], [])
              .filter(i => i),
            rates: p.rates.map(rate => ({
              ...rate,
              team: teams.find(i => i.id === rate.team) || '',
            })),
          }));

          this.setState({ rides: mapRides, loading: false });
        }).catch(c => console.log(c));
    });
  }

  componentDidMount() {
    this.init();
    this.timer = setInterval(() => this.init(), this.interval);
  }

  componentWillUnmount() {
    clearInterval(this.timer);
  }

  render() {
    const { loading, rides, topBidders } = this.state;
    const contents = loading
      ? null
      : rides;

    return (
      <Segment loading={loading} basic>
        <Segment.Inline>
          <Ref innerRef={this.contextRef}>
            <Grid columns={2}>
              <Grid.Column width="4">
                <Sticky offset={50} context={this.contextRef}>
                  <Segment basic>
                    <Header>Лидеры</Header>
                    <Divider />
                    <List relaxed size='large'>
                      {topBidders && topBidders.map((item, key) => (
                        <List.Item key={key}>
                          <List.Icon name='user' size='large' verticalAlign='middle' />
                          <List.Content>
                            <List.Header as='h3'>{item.name}</List.Header>
                            <List.Item>
                              <div style={{ paddingBottom: '4px' }}>
                                <span style={{ fontSize: '16px', fontWeight: 'bold' }}>Текущие очки: </span>
                                <span>{item.currentScore}</span>
                              </div>
                            </List.Item>
                            <List.Item>
                              <div>
                                <span style={{ fontSize: '16px', fontWeight: 'bold' }}>Начальные очки: </span>
                                <span>{item.startScore}</span>
                              </div>
                            </List.Item>
                          </List.Content>
                          <List.Item>
                            <Divider />
                          </List.Item>
                        </List.Item>
                      ))}
                    </List>
                  </Segment>
                </Sticky>
              </Grid.Column>
              <Grid.Column width="12">
                <Table singleLine basic='very'>
                  <Table.Header>
                    <Table.Row>
                      <Table.HeaderCell>Номер заезда</Table.HeaderCell>
                      <Table.HeaderCell>Победители заезда</Table.HeaderCell>
                      <Table.HeaderCell>Статус</Table.HeaderCell>
                    </Table.Row>
                  </Table.Header>
                  <Table.Body>
                    {contents && contents.map((item, key) => (
                      <Fragment key={key}>
                        <Table.Row error>
                          <Table.Cell>
                            <Label ribbon color="black">{`Заезд: ${item.number}`}</Label>
                          </Table.Cell>
                          <Table.Cell>
                            <List key={key} as="ul">
                              {item.winnerTeams && item.winnerTeams.map((winner, key) => (
                                <List.Item key={key} as="li">
                                  {winner.name}
                                </List.Item>
                              ))}
                            </List>
                          </Table.Cell>
                          <Table.Cell>
                            {!item.winnerTeams.length ? 'Не завершен' : 'Завершен'}
                          </Table.Cell>
                        </Table.Row>
                        <Table.Row>
                          <Segment basic>
                            <Header>Ставки:</Header>
                            <Divider />
                            <List>
                              {item.rates && item.rates.map((rate, key) => (
                                <List.Item key={key}>
                                  <List horizontal size="medium">
                                    <List.Item>
                                      <List.Header>{rate.bidder.name}</List.Header>
                                    </List.Item>
                                    <List.Item>
                                      <List.Icon name="long arrow alternate right" size="large" />
                                    </List.Item>
                                    <List.Item>
                                      {rate.rateValue}
                                    </List.Item>
                                    <List.Item>
                                      <List.Icon name="long arrow alternate right" size="large" />
                                    </List.Item>
                                    <List.Item>
                                      <List.Header>{rate.team.name}</List.Header>
                                    </List.Item>
                                  </List>
                                </List.Item>
                              ))}
                            </List>
                          </Segment>
                        </Table.Row>
                      </Fragment>
                    ))}
                  </Table.Body>
                </Table>
              </Grid.Column>
            </Grid>
          </Ref>
        </Segment.Inline>
      </Segment>
    );
  }
}
