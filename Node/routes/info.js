const express = require('express')
const router = new express.Router()
const main = require('./../index')

router.get('/', (req, res) => {
    let pendingTransactions = main.pendingTransactions.length;
    let confirmedTransactions = main.confirmedTransactions.length;
    let blocks = main.blockchain.length;
    let peers = main.peers;
    let difficulty = main.difficulty;
    res.setHeader('Content-Type', 'application/json');
    let info =
        {
            "about": "Block-Chain/0.9-csharp",
            "name": "Varna-01",
             peers,
             difficulty,
             blocks,
             confirmedTransactions,
             pendingTransactions
        };

    res.status(200).json(info)
 })

module.exports = router