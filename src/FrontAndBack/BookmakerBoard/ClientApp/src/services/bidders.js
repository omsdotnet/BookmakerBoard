const address = process.env.REACT_APP_SERVER ? 'http://127.0.0.1:3001/' : '';

export const biddersGetAll = async () => {
    const result = await fetch(`${address}api/Bidders/GetAll`);
    return result.json();
};

export const getTopTree = async () => {
    const result = await fetch(`${address}api/Bidders/GetTopThree`);
    return result.json();
}