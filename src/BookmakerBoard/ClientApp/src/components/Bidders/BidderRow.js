import React from 'react';
import PropTypes from 'prop-types';
import { bidderDelete, bidderAdd, bidderPut } from '../../services/bidders';
import { Table, Button, Input } from 'semantic-ui-react';

class BidderRow extends React.Component {
    state = {
        isChanged: false,
        bidderName: null,
        bidderStartScore: null,
        isRemove: false,
        isSave: false,
    };

    handleSave = id => () => {
        const { name, startScore, isExist, handleSave: save } = this.props;
        const { bidderName, bidderStartScore } = this.state;

      const newBidder = {
        id,
        name: bidderName ? bidderName : name,
        startScore: bidderStartScore ? bidderStartScore : startScore
      };

        if (isExist) {
            bidderPut(newBidder)
                .then(() => {
                    this.setState({ isChanged: false, isSave: false });
                    save(newBidder);
                }).catch((error) => {
                    console.log(error);
                    this.setState({ isSave: false });
                });
        } else {
            bidderAdd(newBidder)
                .then(() => {
                    this.setState({ isChanged: false, isSave: false });
                    save(newBidder);
                }).catch((error) => {
                    console.log(error);
                    this.setState({ isSave: false });
                });
        }
    }

    handleRemove = id => () => {
        const { handleRemove, isExist } = this.props;

        if (isExist) {
            this.setState({ isRemove: true });
            bidderDelete(id).then(() => {
                this.setState({ isRemove: false });
                handleRemove(id);
            });
        } else {
            handleRemove(id);
        }
    }

    handleChangeName = id => (_, { value }) => {
        this.setState({ isChanged: true, bidderName: value });
    }

    handleChangeScore = id => (_, { value }) => {
        this.setState({ isChanged: true, bidderStartScore: value });
    }


    render() {
        const { isChanged, isRemove, isSave } = this.state;
        const { id, name, startScore, currentScore } = this.props;

        return (
            <Table.Row key={id}>
                <Table.Cell>
                    <Input fluid
                        defaultValue={name}
                        onChange={this.handleChangeName(id)} />
                </Table.Cell>
                <Table.Cell>
                    <Input fluid
                      defaultValue={startScore}
                      onChange={this.handleChangeScore(id)} />
                </Table.Cell>
                <Table.Cell>
                      {currentScore}
                </Table.Cell>
                <Table.Cell>
                    <Button color='blue'
                        icon="save"
                        size="medium"
                        disabled={!isChanged}
                        loading={isSave}
                        onClick={this.handleSave(id)} />
                    <Button color='red'
                        icon="remove"
                        size="medium"
                        loading={isRemove}
                        onClick={this.handleRemove(id)} />
                </Table.Cell>
            </Table.Row>
        );
    }
}

BidderRow.propTypes = {
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    startScore: PropTypes.string.isRequired,
    currentScore: PropTypes.string.isRequired,
    isExist: PropTypes.bool.isRequired,
    handleRemove: PropTypes.func.isRequired,
    handleSave: PropTypes.func.isRequired,
};

export default BidderRow;