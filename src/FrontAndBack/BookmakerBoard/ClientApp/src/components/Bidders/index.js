import React, { Component } from 'react';
import { biddersGetAll } from '../../services/bidders';
import { Segment, List, Grid, Table, Button, Divider } from 'semantic-ui-react';

export class Bidders extends Component {
  state = {
    bidders: [],
    loading: true
  };

  componentDidMount() {
    biddersGetAll()
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
      ? null
      : this.state.bidders;

    return (
      <Segment basic>
        <Segment.Inline>
          <List>
            <List.Item>
              <List.Header>Участники ставок</List.Header>
              <List.Description>Люди, которые делают ставки</List.Description>
            </List.Item>
          </List>
          <Grid container>
            <Grid.Column>
              <Table singleLine basic='very'>
                <Table.Header>
                  <Button content="Редактировать" />
                  <Button content="Сохранить" />
                  <Button content="Отмена" />
                  <Divider />
                  <Table.Row>
                    <Table.HeaderCell width="8">Участник</Table.HeaderCell>
                    <Table.HeaderCell width="7">Текущий счет</Table.HeaderCell>
                    <Table.HeaderCell width="1" />
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {(contents && contents.map(item => (
                    <Table.Row key={item.id}>
                      <Table.Cell>
                        {item.name}
                      </Table.Cell>
                      <Table.Cell>
                        {item.currentScore}
                      </Table.Cell>
                      <Table.Cell>
                        <Button color='black'
                          icon="remove"
                          size="tiny" />
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
