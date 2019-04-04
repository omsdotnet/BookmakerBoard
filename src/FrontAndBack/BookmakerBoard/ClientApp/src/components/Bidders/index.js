import React, { Component } from 'react';
import { biddersGetAll } from '../../services/bidders';
import { Segment, List, Grid, Table, Button, Divider } from 'semantic-ui-react';
import BidderRow from './BidderRow';

export class Bidders extends Component {
  state = {
    bidders: [],
    loading: true,
    newBidder: null
  };

  bidderAll = () => {
    this.setState({ loading: true });
    biddersGetAll().then(data => {
      this.setState({ bidders: data.map(p => ({ ...p, isExist: true })), loading: false });
    }).catch(c => console.log(c));
  }

  componentDidMount() {
    this.bidderAll();
  }

  handleRemove = id => () => {
    const { bidders, newBidder } = this.state;
    this.setState({
      bidders: bidders.filter(p => p.id !== id),
      newBidder: newBidder === id ? null : newBidder,
    });
  }

  handleSave = id => () => {
    const { newBidder } = this.state;

    this.setState({
      newBidder: newBidder === id ? null : newBidder,
    });

    this.bidderAll();
  }

  handleAdd = () => {
    const { bidders } = this.state;

    this.setState({
      bidders: [{ id: bidders.length, name: '', startScore: 1000, isExist: false }].concat(bidders),
      newBidder: bidders.length,
    });
  }

  render() {
    const { loading, bidders, newBidder } = this.state;
    const contents = loading ? null : bidders;

    return (
      <>
        <Segment loading={loading} basic>
          <Grid container>
            <Grid.Column>
              <List>
                <List.Item>
                  <List.Header>Участники ставок</List.Header>
                  <List.Description>Люди, которые делают ставки</List.Description>
                </List.Item>
              </List>
              <Grid.Row>
                <Button content="Добавить"
                  color='blue'
                  onClick={this.handleAdd}
                  disabled={!!newBidder} />
                <Divider />
              </Grid.Row>
              <Table celled basic='very'>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell singleLine content="Участник" />
                    <Table.HeaderCell singleLine content="Начальный счет" />
                    <Table.HeaderCell singleLine content="Текущий счет" />
                    <Table.HeaderCell singleLine content="Действие" />
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {(contents && contents.map(item => (
                    <BidderRow id={item.id}
                      name={item.name}
                      startScore={item.startScore}
                      currentScore={item.currentScore}
                      isExist={item.isExist}
                      handleRemove={this.handleRemove(item.id)}
                      handleSave={this.handleSave(item.id)} />
                  )))}                  
                </Table.Body>
              </Table>
            </Grid.Column>
          </Grid>
        </Segment>
      </>      
    );
  }
}
