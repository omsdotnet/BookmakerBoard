const address = process.env.REACT_APP_SERVER ? 'http://127.0.0.1:3001/' : '';

export const teamsGetAll = async () => {
    const result = await fetch(`${address}api/Teams/GetAll`);
    return result.json();
};

export const teamDelete = async (id) => {
    return await fetch(`${address}api/Teams/${id}`, { method: 'DELETE' });
}

export const teamAdd = async (team) => {
    return await fetch(`${address}api/Teams`, {
        body: JSON.stringify(team),
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}