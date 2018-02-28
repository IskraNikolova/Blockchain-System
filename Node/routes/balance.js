const express = require('express');
const router = new express.Router();
const balanceHandler = require('./../handlers/balance');

//GET Balance TODO
router.get('/:address/confirmations/:confirmationsIndex', (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    let address = req.params['address'];
    let confirmations = req.params['confirmationsIndex'];
    let confirmedBalance = balanceHandler.getBalanceWithConfirm(address, confirmations);
    let lastMinedBalance = balanceHandler.getBalanceWithConfirm(address, 1);
    let pendingBalance = balanceHandler.getBalanceWithPendingTransaction(address);

    const balance = 
    {
         address,
         confirmations,
         confirmedBalance,
         lastMinedBalance,
         pendingBalance: {"confirmations": 0, "balance": pendingBalance}
    }

    res.status(200).json(balance)
 })

module.exports = router