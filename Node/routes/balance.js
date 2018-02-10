const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/:address/confirmations/:confirmationsIndex', (req, res) => {
    
    const balance = handlerNode.getBalance(req, res);

    res.status(200).json(balance)
 })

module.exports = router