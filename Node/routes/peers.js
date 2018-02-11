const express = require('express')
const router = new express.Router()
const WebSocket = require("ws")
const index = require('./../index')

//GET Peers
router.get('/', (req, res) => {
   res.send(sockets.map(s => s._socket.remoteAddress + ':' + s._socket.remotePort));
})

//POST Peer
 router.post('/', (req, res) => {
    index.connectToPeers([req.body.peer]);
    res.send();
})

module.exports = router