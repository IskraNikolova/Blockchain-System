const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/get-block/:address', (req, res) => {
    const result = handlerNode.getMiningBlock(req, res);
    res.status(200).json(result)
 })

 //router.post('/pow', (req, res) => {
    //const result = handlerNode.postPoW(req, res);
    //res.status(200).json(result)
 //})

module.exports = router