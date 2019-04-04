export const MENU_KEYS = {
    SUMMARY: 'Результаты',
    RIDES: 'Заезды',
    TEAMS: 'Команды',
    PARTICIPANTS: 'Участники',
    LOGIN: 'Войти'
}

export const MENU_CONTAINER_ITEMS = [
    {
        index: 0,
        key: 0,
        name: MENU_KEYS.SUMMARY,
        active: true,
    },
    {
        index: 1,
        key: 1,
        name: MENU_KEYS.RIDES,
        active: false,
    },
    {
        index: 2,
        key: 2,
        name: MENU_KEYS.TEAMS,
        active: false,
    },
    {
        index: 3,
        key: 3,
        name: MENU_KEYS.PARTICIPANTS,
        active: false,
    },
    {
        index: 4,
        key: 4,
        name: MENU_KEYS.LOGIN,
        active: false,
        position: 'right',
    }
];