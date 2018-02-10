const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/', (req, res) => {
    const peers = handlerNode.getPeers(req, res); 
    res.status(200).json(peers)
 })

 router.post('/', (req, res) => {
    const message = handlerNode.getPeers(req, res);
    res.status(200).json({
      success: true,
      message
    })
  })

module.exports = router