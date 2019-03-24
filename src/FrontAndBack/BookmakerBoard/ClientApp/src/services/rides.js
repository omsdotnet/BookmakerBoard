
const address = process.env.REACT_APP_SERVER ? 'http://127.0.0.1:3001/' : '';

export const ridesGetAll = async () => {
    const result = await fetch(`${address}api/Rides/GetAll`);
    return result.json();
};