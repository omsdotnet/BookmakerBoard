const express = require('express');

const ridesResponse = require('./mock/rides.json');
const teamsResponse = require('./mock/teams.json');
const biddersResponse = require('./mock/bidders.json');
const topTreeResponse = require('./mock/topTree.json');

const router = express.Router();

const api = () => {
    router.get('/Rides/GetAll', (req, res) => {
        res.json(ridesResponse);
    });

    router.get('/Teams/GetAll', (req, res) => {
        res.json(teamsResponse);
    });

    router.get('/Bidders/GetAll', (req, res) => {
        res.json(biddersResponse);
    });

    router.delete('/Teams', (req, res) => {
        res.status(200);
    });

    router.post('/Teams', (req, res) => {
        res.status(200).json('OK');
    });

    router.get('/Bidders/GetTopThree', (req, res) => {
        res.json(topTreeResponse);
    });

    return router;
};

module.exports = api;