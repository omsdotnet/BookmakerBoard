const express = require('express');
const api = require('./api');

const port = 3001;

(() => {
    const app = express();

    app.use((req, res, next) => {
        res.header('Access-Control-Allow-Origin', req.headers.origin);
        res.header('Access-Control-Allow-Methods', 'PUT, POST, GET, DELETE, OPTIONS');
        next();
    });

    app.use('/api', api());

    app.use((error) => {
        console.error(error);
    });

    app.listen(port, () => {
        console.log(`Listening at http://localhost:${port}`);
    })
})();