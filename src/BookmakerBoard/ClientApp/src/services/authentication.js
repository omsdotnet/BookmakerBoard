const address = process.env.REACT_APP_SERVER ? 'http://127.0.0.1:3001/' : '';

export const isSignIn = async () => {
    const result = await fetch(`${address}api/Authentication/IsSignIn`);
    return result.json();
};

export const logout = async () => {
  return await fetch(`${address}api/Authentication/Logout`);
};

export const login = async (name, pass) => {
  return await fetch(`${address}api/Authentication/Login?login=${name}&password=${pass}`, {
        body: '',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}