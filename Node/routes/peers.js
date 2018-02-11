const express = require('express')
const router = new express.Router()
const WebSocket = require("ws")
const index = require('./../index')

//set HTTP_PORT=5555 && set P2P_PORT=6001 && npm start
//set HTTP_PORT=5556 && set P2P_PORT=6002 && set PEERS=http://localhost:6001 && npm start

//GET Peers
router.get('/', (req, res) => {
   res.send(index.sockets.map(s => s._socket.remoteAddress + ':' + s._socket.remotePort));
})

//POST Peer
 router.post('/', (req, res) => {
    index.connectToPeers([req.body.peer]);
    res.send();
})

module.exports = router