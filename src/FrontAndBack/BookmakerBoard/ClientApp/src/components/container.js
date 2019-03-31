import React from 'react';
import { Menu, Segment } from 'semantic-ui-react';
import { MENU_CONTAINER_ITEMS, MENU_KEYS } from './constants';
import { Home } from './Summary/index';
import { Teams } from './Teams';
import { Bidders } from './Bidders';
import { Rides } from './Rides/index';

const fixedMenuStyle = {
    backgroundColor: '#fff',
    border: '1px solid #ddd',
    boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.2)',
}

class Container extends React.Component {
    state = {
        itemsMenu: MENU_CONTAINER_ITEMS,
    };

    handlerMenuClick = (_, data) => {
        this.setState((prev) => {
            return {
                itemsMenu: prev.itemsMenu.map(item => ({ ...item, active: data.index === item.index }))
            }
        });
    }

    render() {
        const activeMenu = this.state.itemsMenu.find(p => p.active);

        return (
            <>
                <Menu pointing
                    secondary
                    borderless
                    fixed="top"
                    style={fixedMenuStyle}
                    items={this.state.itemsMenu}
                    onItemClick={this.handlerMenuClick} />
                {activeMenu.name === MENU_KEYS.SUMMARY && (
                    <Segment vertical padded='very'>
                        <Home />
                    </Segment>
                )}
                {activeMenu.name === MENU_KEYS.RIDES && (
                    <Segment vertical padded='very'>
                        <Rides />
                    </Segment>
                )}
                {activeMenu.name === MENU_KEYS.TEAMS && (
                    <Segment vertical padded='very'>
                        <Teams />
                    </Segment>
                )}
                {activeMenu.name === MENU_KEYS.PARTICIPANTS && (
                    <Segment vertical padded='very'>
                        <Bidders />
                    </Segment>
                )}
            </>
        );
    }
}

export default Container;