const express = require('express')
const router = new express.Router()
const main = require('./../index')

router.get('/', (req, res) => {
    let pendTransaction = main.pendingTransactions.length;
    let confTran = main.confirmedTransactions.length;
    let blocks = main.blockchain.length;
    let peers = main.peers;
    res.setHeader('Content-Type', 'application/json');
    let info =
        {
            "about": "Block-Chain/0.9-csharp",
            "name": "Varna-01",
            "peers": peers,
            "blocks": blocks,
            "confirmedTransactions": confTran,
            "pendingTransactions": pendTransaction,
            "addresses": 12,
            "coins": 18000000
        };

    res.status(200).json(info)
 })

module.exports = router