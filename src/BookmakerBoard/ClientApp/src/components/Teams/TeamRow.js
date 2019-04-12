import React from 'react';
import PropTypes from 'prop-types';
import { teamDelete, teamAdd, teamPut } from '../../services/teams';
import { Table, Button, Input } from 'semantic-ui-react';

class TeamRow extends React.Component {
    state = {
        isChanged: false,
        teamName: null,
        isRemove: false,
        isSave: false,
    };

    handleSave = id => () => {
        const { isExist, handleSave: save } = this.props;
        const { teamName } = this.state;

        const newTeam = { id, name: teamName };
        if (isExist) {
            teamPut(newTeam)
                .then(() => {
                    this.setState({ teamName: '', isChanged: false, isSave: false });
                    save(newTeam);
                }).catch((error) => {
                    console.log(error);
                    this.setState({ isSave: false });
                });
        } else {
            teamAdd(newTeam)
                .then(() => {
                    this.setState({ teamName: '', isChanged: false, isSave: false });
                    save(newTeam);
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
            teamDelete(id).then(() => {
                this.setState({ isRemove: false });
                handleRemove(id);
            });
        } else {
            handleRemove(id);
        }
    }

    handleChange = id => (_, { value }) => {
        this.setState({ isChanged: true, teamName: value });
    }

    render() {
        const { isChanged, isRemove, isSave } = this.state;
        const { id, name } = this.props;

        return (
            <Table.Row key={id}>
                <Table.Cell>
                    <Input fluid
                        defaultValue={name}
                        onChange={this.handleChange(id)} />
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

TeamRow.propTypes = {
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    isExist: PropTypes.bool.isRequired,
    handleRemove: PropTypes.func.isRequired,
    handleSave: PropTypes.func.isRequired,
};

export default TeamRow;