import React, { Component, createRef } from 'react';
import { ridesGetAll } from '../../services/rides';
import {
  Grid, Sticky, Ref, List,
  Segment, Header, Divider, Table, Label,
} from 'semantic-ui-react';

export class Home extends Component {
  contextRef = createRef();

  state = {
    rides: [],
    loading: true,
  };

  componentDidMount() {
    ridesGetAll().then(data => {
      this.setState({ rides: data, loading: false });
    }).catch(c => console.log(c));
  }

  render() {
    const contents = this.state.loading
      ? null
      : this.state.rides;

    return (
      <Segment basic>
        <Segment.Inline>
          <Ref innerRef={this.contextRef}>
            <Grid columns={2}>
              <Grid.Column width="4">
                <Sticky offset={50} context={this.contextRef}>
                  <Segment basic>
                    <Header>Лидеры</Header>
                    <Divider />
                    <List divided relaxed size='large'>
                      <List.Item>
                        <List.Icon name='user' size='large' verticalAlign='middle' />
                        <List.Content>
                          <List.Header>Константин Густов</List.Header>
                          <List.Description>+1000 очков</List.Description>
                        </List.Content>
                      </List.Item>
                      <List.Item>
                        <List.Icon name='user' size='large' verticalAlign='middle'>
                        </List.Icon>
                        <List.Content>
                          <List.Header>Павел Кульбида</List.Header>
                          <List.Description>+1000 очков</List.Description>
                        </List.Content>
                      </List.Item>
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
                    {contents && contents.map(item => (
                      <>
                        <Table.Row error>
                          <Table.Cell>
                            <Label ribbon color="black">{`Заезд: ${item.number}`}</Label>
                          </Table.Cell>
                          <Table.Cell>
                            <List as="ol">
                              <List.Item as="li">
                                Густов Константин
                              </List.Item>
                              <List.Item as="li">
                                Павел Кульбида
                              </List.Item>
                            </List>
                          </Table.Cell>
                          <Table.Cell>Завершен</Table.Cell>
                        </Table.Row>
                        <Table.Row>
                          <Segment basic>
                            <Header>Ставки:</Header>
                            <Divider />
                            <List>
                              <List.Item>
                                <List horizontal size="medium">
                                  <List.Item>
                                    <List.Header>Павел Кульбида</List.Header>
                                  </List.Item>
                                  <List.Item>
                                    <List.Icon name="long arrow alternate right" size="large" />
                                  </List.Item>
                                  <List.Item>
                                    1000$
                                  </List.Item>
                                  <List.Item>
                                    <List.Icon name="long arrow alternate right" size="large" />
                                  </List.Item>
                                  <List.Item>
                                    <List.Header>Константин Густов</List.Header>
                                  </List.Item>
                                </List>
                              </List.Item>
                            </List>
                          </Segment>
                        </Table.Row>
                      </>
                    ))}
                  </Table.Body>
                </Table>
                {/* <Segment>
                  {(new Array(150).fill(0).map(p => (<Header>Hello</Header>)))}
                </Segment> */}
              </Grid.Column>
            </Grid>
          </Ref>
        </Segment.Inline>
      </Segment>
    );
  }
}
