const express = require('express')
const bodyParser = require('body-parser')
var WebSocket = require("ws");

var initialPeers = process.env.PEERS ? process.env.PEERS.split(',') : [];

const blocksRoutes = require('./routes/blocks')
const balanceRoutes = require('./routes/balance')
const transactionRoutes = require('./routes/transactions')
const peersRoutes = require('./routes/peers')
const infoRoutes = require('./routes/info')
const miningRoutes = require('./routes/mining')
const Block = require("./models/block")
const Node = require('./models/node')

const http_port = process.env.HTTP_PORT || 5555;
var p2p_port = process.env.P2P_PORT || 6002;

let getGenesisBlock = () => {
    return new Block(
         0, 
         [],
         5, 
         "d279fa6a31ae4fb07cfd9cf7f35cc013cf20a", 
         "f582d57711a618e69d588ce93895d749858fa95b", 
         "5d845cddcd4404ecfd5476fd6b1cf0ea80cd3", 
         2455432, 
         "2018-02-01T23:23:56.337Z", 
         '816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7');
};

module.exports.blockchain = [getGenesisBlock()];
module.exports.pendingTransactions = [];
module.exports.confirmedTransactions = [];
module.exports.miningJobs = [];
module.exports.difficulty = 5;
module.exports.peers = initialPeers.length;

module.exports.sockets = [];
module.exports.broadcast = (message) => this.sockets.forEach(socket => write(socket, message));

module.exports.responseLatestMsg = () => ({
    'type': MessageType.RESPONSE_BLOCKCHAIN,
    'data': JSON.stringify([getLatestBlock()])
});

var MessageType = {
    QUERY_LATEST: 0,
    QUERY_ALL: 1,
    RESPONSE_BLOCKCHAIN: 2
};

var initHttpServer = () => {
    const app = express()
    app.use(bodyParser.urlencoded({ extended: false }))
    app.use(bodyParser.json())
    // routes
    app.use('/info', infoRoutes)
    app.use('/blocks', blocksRoutes)
    app.use('/balance', balanceRoutes)
    app.use('/transactions', transactionRoutes)
    app.use('/peers', peersRoutes)
    app.use('/mining', miningRoutes)
    app.listen(http_port, () => {
    console.log(`Server running on port ${http_port}...`)
    })
};

var initP2PServer = () => {
  var server = new WebSocket.Server({port: p2p_port});
  server.on('connection', ws => initConnection(ws));
  console.log('listening websocket p2p port on: ' + p2p_port);
};

var initConnection = (ws) => {
  this.sockets.push(ws);
  initMessageHandler(ws);
  initErrorHandler(ws);
  write(ws, queryChainLengthMsg());
};

var initMessageHandler = (ws) => {
  ws.on('message', (data) => {
      var message = JSON.parse(data);
      console.log('Received message' + JSON.stringify(message));
      switch (message.type) {
          case MessageType.QUERY_LATEST:
              write(ws, this.responseLatestMsg());
              break;
          case MessageType.QUERY_ALL:
              write(ws, responseChainMsg());
              break;
          case MessageType.RESPONSE_BLOCKCHAIN:
              handleBlockchainResponse(message);
              break;
      }
  });
};

var initErrorHandler = (ws) => {
  var closeConnection = (ws) => {
      console.log('connection failed to peer: ' + ws.url);
      this.sockets.splice(this.sockets.indexOf(ws), 1);
  };
  ws.on('close', () => closeConnection(ws));
  ws.on('error', () => closeConnection(ws));
};

module.exports.connectToPeers = (newPeers) => {
    newPeers.forEach((peer) => {
        var ws = new WebSocket(peer);
        ws.on('open', () => {
            initConnection(ws)
        });

        ws.on('error', () => {
            console.log('connection failed')
        });
    });
};

var handleBlockchainResponse = (message) => {
  var receivedBlocks = JSON.parse(message.data).sort((b1, b2) => (b1.index - b2.index));
  var latestBlockReceived = receivedBlocks[receivedBlocks.length - 1];
  var latestBlockHeld = getLatestBlock();
  if (latestBlockReceived.index > latestBlockHeld.index) {
      console.log('blockchain possibly behind. We got: ' + latestBlockHeld.index + ' Peer got: ' + latestBlockReceived.index);
      if (latestBlockHeld.hash === latestBlockReceived.previousHash) {
          console.log("We can append the received block to our chain");
          this.blockchain.push(latestBlockReceived);
          this.broadcast(this.responseLatestMsg());
      } else if (receivedBlocks.length === 1) {
          console.log("We have to query the chain from our peer");
          this.broadcast(queryAllMsg());
      } else {
          console.log("Received blockchain is longer than current blockchain");
          replaceChain(receivedBlocks);
      }
  } else {
      console.log('received blockchain is not longer than current blockchain. Do nothing');
  }
};

var replaceChain = (newBlocks) => {
  if (isValidChain(newBlocks) && newBlocks.length > this.blockchain.length) {
      console.log('Received blockchain is valid. Replacing current blockchain with received blockchain');
      this.blockchain = newBlocks;
      this.broadcast(this.responseLatestMsg());
  } else {
      console.log('Received blockchain invalid');
  }
};

var isValidChain = (blockchainToValidate) => {
  if (JSON.stringify(blockchainToValidate[0]) !== JSON.stringify(getGenesisBlock())) {
      return false;
  }
  var tempBlocks = [blockchainToValidate[0]];
  for (var i = 1; i < blockchainToValidate.length; i++) {
      if (isValidNewBlock(blockchainToValidate[i], tempBlocks[i - 1])) {
          tempBlocks.push(blockchainToValidate[i]);
      } else {
          return false;
      }
  }
  return true;
};

var getLatestBlock = () => this.blockchain[this.blockchain.length - 1];
var queryChainLengthMsg = () => ({'type': MessageType.QUERY_LATEST});
var queryAllMsg = () => ({'type': MessageType.QUERY_ALL});
var responseChainMsg = () =>({
  'type': MessageType.RESPONSE_BLOCKCHAIN, 'data': JSON.stringify(this.blockchain)
});

var write = (ws, message) => ws.send(JSON.stringify(message));

this.connectToPeers(initialPeers);
initHttpServer();
initP2PServer();