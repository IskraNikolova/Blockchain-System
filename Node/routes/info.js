const express = require('express')
const router = new express.Router()

router.get('/', (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    let info =
        {
            "about": "Block-Chain/0.9-csharp",
            "name": "Varna-01",
            "peers": 2,
            "blocks": 25,
            "confirmedTransactions": 208,
            "pendingTransactions": 7,
            "addresses": 12,
            "coins": 18000000
        };

    res.status(200).json(info)
 })

module.exports = router