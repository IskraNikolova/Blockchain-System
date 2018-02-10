const express = require('express')
const router = new express.Router()

router.get('/:address/confirmations/:confirmationsIndex', (req, res) => {
    
    const balance = 
    {
        "address": req.params['address'],
        "confirmations": req.params['confirmationsIndex'],
        "confirmedBalance": {"confirmations": 8, "balance": 120.00},
        "lastMinedBalance": {"confirmations": 1, "balance": 115.00},
        "pendingBalance": {"confirmations": 0, "balance": 170.20}
      }
      

    res.status(200).json(balance)
 })

module.exports = router