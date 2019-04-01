import React from 'react';
import PropTypes from 'prop-types';
import { Table, Dropdown, Input, Button } from 'semantic-ui-react';
import { ridesPut } from '../../services/rides';

class RidesRow extends React.Component {
    state = {
        biddersOptions: [],
        teamsOptions: [],
        isSave: false,
        isRemove: false,
        newRate: {},
        isChange: false,
    };

    static getDerivedStateFromProps = (props, state) => {
        if (!state.biddersOptions.length && !state.teamsOptions.length) {

            const bidder = props.bidders.find(p => p.id === props.bidderId);
            return {
                biddersOptions: props.bidders
                    .map((item, key) => ({ key, value: item.id, text: item.name })),
                teamsOptions: props.teams.map((item, key) => ({ key, value: item.id, text: item.name })),
                newRate: {
                    ...props.ride.rates[0],
                    bidder: {
                        id: bidder ? bidder.id : -1,
                        name: bidder ? bidder.name : '',
                        currentScore: bidder ? bidder.currentScore : 0,
                        startScore: bidder ? bidder.startScore : 0,
                    },
                    rateValue: props.rateScore,
                    team: props.teamWin,
                },
            };
        }

        return null;
    }

    handleRemove = () => {
        const { id, handleRemove } = this.props;
        handleRemove(id);
    }

    handleBidder = (_, { value }) => {
        const { bidders } = this.props;
        const { newRate } = this.state;

        const bidder = bidders.find(p => p.id === value);
        this.setState({
            newRate: {
                ...newRate,
                bidder: {
                    id: bidder ? bidder.id : -1,
                    name: bidder ? bidder.name : '',
                    currentScore: bidder ? bidder.currentScore : 0,
                    startScore: bidder ? bidder.startScore : 0,
                }
            },
            isChange: true,
        });
    }

    handleRate = (_, { value }) => {
        const { newRate } = this.state;
        this.setState({
            newRate: {
                ...newRate,
                rateValue: parseInt(value),
            },
            isChange: true,
        });
    }

    handleTeamWin = (_, { value }) => {
        const { newRate } = this.state;

        this.setState({
            newRate: {
                ...newRate,
                team: value,
            },
            isChange: true,
        });
    }

    handleSave = () => {
        const { handleSave, ride } = this.props;
        const { newRate } = this.state;

        const newRide = {
            ...ride,
            rates: ride.rates.map((p, i) => i === 0 ? newRate : p),
        };

        this.setState({ isSave: true });
        ridesPut(newRide).then(() => {
            this.setState({ isChange: false, isSave: false });
            handleSave({ newRate });
        }).catch(() => {
            this.setState({ isChange: false, isSave: false });
        });
    }

    render() {
        const {
            biddersOptions,
            teamsOptions,
            isSave,
            newRate,
            isChange,
        } = this.state;
        const { bidderId, rateScore, teamWin, isExist } = this.props;
        const isEdit = !isExist ? newRate && newRate.bidder.id && newRate.bidder.id !== -1 &&
            newRate.bidder.name && newRate.bidder.name !== '' &&
            newRate.bidder.currentScore && newRate.bidder.currentScore !== 0 &&
            newRate.bidder.startScore && newRate.bidder.startScore !== 0 &&
            newRate.rateValue && newRate.rateValue !== 0 &&
            newRate.team && newRate.team !== -1 : isExist ? isChange : false;

        return (
            <Table.Row>
                <Table.Cell>
                    <Dropdown fluid
                        direction='right'
                        options={biddersOptions}
                        defaultValue={bidderId}
                        onChange={this.handleBidder} />
                </Table.Cell>
                <Table.Cell>
                    {newRate.bidder.currentScore}
                </Table.Cell>
                <Table.Cell>
                    <Input fluid
                        defaultValue={rateScore}
                        onChange={this.handleRate} />
                </Table.Cell>
                <Table.Cell>
                    <Dropdown fluid
                        direction='right'
                        options={teamsOptions}
                        defaultValue={teamWin}
                        onChange={this.handleTeamWin} />
                </Table.Cell>
                <Table.Cell>
                    <Button color='blue'
                        icon="save"
                        size="medium"
                        disabled={!isEdit}
                        loading={isSave}
                        onClick={this.handleSave} />
                    <Button color='red'
                        icon="remove"
                        size="medium"
                        onClick={this.handleRemove} />
                </Table.Cell>
            </Table.Row>
        );
    }
}

RidesRow.propTypes = {
    id: PropTypes.number.isRequired,
    bidders: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
        currentScore: PropTypes.number.isRequired,
    })).isRequired,
    teams: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
    })).isRequired,
    isExist: PropTypes.bool.isRequired,
    bidderId: PropTypes.number.isRequired,
    bidderScore: PropTypes.number.isRequired,
    rateScore: PropTypes.number.isRequired,
    teamWin: PropTypes.number.isRequired,
    handleRemove: PropTypes.func.isRequired,
    handleSave: PropTypes.func.isRequired,
    ride: PropTypes.shape().isRequired, // ладно уже ночь
};

export default RidesRow;