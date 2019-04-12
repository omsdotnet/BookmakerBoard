const address = process.env.REACT_APP_SERVER ? 'http://127.0.0.1:3001/' : '';

export const biddersGetAll = async () => {
    const result = await fetch(`${address}api/Bidders/GetAll`);
    return result.json();
};

export const getTop = async () => {
    const result = await fetch(`${address}api/Bidders/GetTop`);
    return result.json();
}

export const bidderDelete = async (id) => {
  return await fetch(`${address}api/Bidders/${id}`, { method: 'DELETE' });
}

export const bidderAdd = async (bidder) => {
  return await fetch(`${address}api/Bidders`, {
    body: JSON.stringify(bidder),
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    }
  });
}

export const bidderPut = async (bidder) => {
  return await fetch(`${address}api/Bidders/${bidder.id}`, {
    body: JSON.stringify(bidder),
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    }
  });
}