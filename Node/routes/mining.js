const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/get-block/:address', (req, res) => {
    const result = handlerNode.getMiningBlock(req, res);
    res.status(200).json(result)
 })

 router.post('/submit-block/:address', (req, res) => {
    let result = handlerNode.postMiningBlock(req, res);
    res.status(200).json("")
 })

module.exports = router