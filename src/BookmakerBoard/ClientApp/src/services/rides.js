
const address = process.env.REACT_APP_SERVER ? 'http://127.0.0.1:3001/' : '';

export const ridesGetAll = async () => {
    const result = await fetch(`${address}api/Rides/GetAll`);
    return result.json();
};

export const ridesGetById = async (id) => {
  const result = await fetch(`${address}api/Rides/GetById/${id}`);
  return result.json();
};

export const ridesDelete = async (id) => {
    return await fetch(`${address}api/Rides/${id}`, { method: 'DELETE' });
};

export const ridesPut = async (param) => {
    return await fetch(`${address}api/Rides/${param.id}`, {
        body: JSON.stringify(param),
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        }
    });
};

export const ridesCreate = async (param) => {
    return await fetch(`${address}api/Rides`, {
        body: JSON.stringify(param),
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}