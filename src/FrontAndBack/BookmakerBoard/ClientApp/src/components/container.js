import React from 'react';
import { Menu, Segment } from 'semantic-ui-react';
import { MENU_CONTAINER_ITEMS, MENU_KEYS } from './constants';
import { Home } from './Summary/index';
import { Teams } from './Teams';
import { Bidders } from './Bidders';

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
                        Заезды
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

// const Container = () => {
//     const []
//     return (
//         <>
//             <Menu pointing secondary
//                 items={MENU_CONTAINER_ITEMS} onItemClick />
//             <Segment basic stackable>
//                 <Grid stackable columns={2}>
//                     <Grid.Column width="5">
//                         <List divided relaxed size='large'>
//                             <List.Item>
//                                 <List.Icon name='github' size='large' verticalAlign='middle' />
//                                 <List.Content>
//                                     <List.Header as='a'>Semantic-Org/Semantic-UI</List.Header>
//                                     <List.Description as='a'>Updated 10 mins ago</List.Description>
//                                 </List.Content>
//                             </List.Item>
//                             <List.Item>
//                                 <List.Icon name='github' size='large' verticalAlign='middle' />
//                                 <List.Content>
//                                     <List.Header as='a'>Semantic-Org/Semantic-UI-Docs</List.Header>
//                                     <List.Description as='a'>Updated 22 mins ago</List.Description>
//                                 </List.Content>
//                             </List.Item>
//                             <List.Item>
//                                 <List.Icon name='github' size='large' verticalAlign='middle' />
//                                 <List.Content>
//                                     <List.Header as='a'>Semantic-Org/Semantic-UI-Meteor</List.Header>
//                                     <List.Description as='a'>Updated 34 mins ago</List.Description>
//                                 </List.Content>
//                             </List.Item>
//                         </List>
//                     </Grid.Column>
//                     <Grid.Column width="11">
//                         <Table celled>
//                             <Table.Header>
//                                 <Table.Row>
//                                     <Table.HeaderCell>Номер заезда</Table.HeaderCell>
//                                     <Table.HeaderCell>Статус</Table.HeaderCell>
//                                 </Table.Row>
//                             </Table.Header>

//                             <Table.Body>
//                                 <Table.Row>
//                                     <Table.Cell>
//                                         <Label ribbon color="orange">Раунд: 1</Label>
//                                     </Table.Cell>
//                                     <Table.Cell>Завершен</Table.Cell>
//                                 </Table.Row>
//                                 <Table.Row >
//                                     <Segment basic>
//                                         <List>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Krowlewskie Jadlo</List.Header>
//                                                     <List.Description>
//                                                         An excellent polish restaurant, quick delivery and hearty, filling meals.
//         </List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Xian Famous Foods</List.Header>
//                                                     <List.Description>
//                                                         A taste of Shaanxi's delicious culinary traditions, with delights like spicy cold noodles
//                                                         and lamb burgers.
//         </List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Sapporo Haru</List.Header>
//                                                     <List.Description>Greenpoint's best choice for quick and delicious sushi.</List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Enid's</List.Header>
//                                                     <List.Description>At night a bar, during the day a delicious brunch spot.</List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                         </List>
//                                     </Segment>
//                                 </Table.Row>
//                                 <Table.Row >
//                                     <Segment basic>
//                                         <List>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Krowlewskie Jadlo</List.Header>
//                                                     <List.Description>
//                                                         An excellent polish restaurant, quick delivery and hearty, filling meals.
//         </List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Xian Famous Foods</List.Header>
//                                                     <List.Description>
//                                                         A taste of Shaanxi's delicious culinary traditions, with delights like spicy cold noodles
//                                                         and lamb burgers.
//         </List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Sapporo Haru</List.Header>
//                                                     <List.Description>Greenpoint's best choice for quick and delicious sushi.</List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Enid's</List.Header>
//                                                     <List.Description>At night a bar, during the day a delicious brunch spot.</List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                         </List>
//                                     </Segment>
//                                 </Table.Row>
//                                 <Table.Row >
//                                     <Segment basic>
//                                         <List>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Krowlewskie Jadlo</List.Header>
//                                                     <List.Description>
//                                                         An excellent polish restaurant, quick delivery and hearty, filling meals.
//         </List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Xian Famous Foods</List.Header>
//                                                     <List.Description>
//                                                         A taste of Shaanxi's delicious culinary traditions, with delights like spicy cold noodles
//                                                         and lamb burgers.
//         </List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Sapporo Haru</List.Header>
//                                                     <List.Description>Greenpoint's best choice for quick and delicious sushi.</List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                             <List.Item>
//                                                 <List.Icon name='user' />
//                                                 <List.Content>
//                                                     <List.Header as='a'>Enid's</List.Header>
//                                                     <List.Description>At night a bar, during the day a delicious brunch spot.</List.Description>
//                                                 </List.Content>
//                                             </List.Item>
//                                         </List>
//                                     </Segment>
//                                 </Table.Row>
//                             </Table.Body>
//                         </Table>
//                     </Grid.Column>
//                 </Grid>
//             </Segment>
//         </>
//     );
// }

export default Container;