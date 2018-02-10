let blockchain = require('./blockchain');
let main = require('../index');
let CryptoJS = require("crypto-js");

//Node Info
module.exports.getNodeInfo = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "about": "Block-Chain/0.9-csharp",
            "name": "Varna-01",
            "peers": 2,
            "blocks": 25,
            "confirmedTransactions": 208,
            "pendingTransactions": 7,
            "addresses": 12,
            "coins": 18000000
        }
    )
}

//GET Blocks
module.exports.getAllBlocks = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(
        main.blockchain
    )
}

//GET Block
module.exports.getBlockByIndex = (req, res, index) => {
    res.setHeader('Content-Type', 'application/json');

    const block = main.blockchain.find(b => b.index == index);
    res.send(
        block
    )
}

//POST Block
module.exports.postNotifyForBlock = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    const newBlockIndex = 
    {
        "index": 9987
    }
    //TODO
    res.send(
        {
            "message": "Thank you!" 
        }
    )
}

//GET Transaction
module.exports.getTransactionInfo = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(
        {
            "from": req.params['tranHash'],
            "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
            "value": 25.00,
            "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
            "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"],
            "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b…176a",
            "paid": true,
            "dateReceived": "2018-02-01T07:47:51.982Z",
            "minedInBlockIndex": 7
          }
    )
}

//POST Transaction
module.exports.postTransaction = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    const transaction = 
    {
        "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
        "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
        "value": 25.00,
        "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
        "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"]
    }
    //TODO
    res.send(
        { 
            "dateReceived": "2018-02-01T23:17:02.744Z",
            "transactionHash": "cd8d9a345bb208c6f9b8acd6b8eefe6…20c8a" 
        } 
    )
}

//GET Balance
module.exports.getBalance = (req, res) => {
    res.setHeader('Content-Type', 'application/json');

    const balance = 
    {
        "address": req.params['address'],
        "confirmations": req.params['confirmationsIndex'],
        "confirmedBalance": {"confirmations": 8, "balance": 120.00},
        "lastMinedBalance": {"confirmations": 1, "balance": 115.00},
        "pendingBalance": {"confirmations": 0, "balance": 170.20}
      }

    res.send(
        balance
    )
}

//GET Peers
module.exports.getPeers = (req, res) => {
    res.setHeader('Content-Type', 'application/json');

    const peers = 
    [
        "http://212.50.11.109:5555",
        "http://af6c7a.ngrok.org:5555"
    ]

    res.send(
        peers
    )
}

//POST Peer
module.exports.postPeer = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    //TODO
    const peer = 
    {
        "url": "http://212.50.11.109:5555"
    }

    res.send(
        'Added peer...'
    )
}

//GET Mining Block
module.exports.getMiningBlock = (req, res) => {
    let minerAddress = req.params['address'];
    let miningJob = blockchain.miningJob(minerAddress);

    res.setHeader('Content-Type', 'application/json');
    res.send(miningJob)
}

//Post Done Miner Job
module.exports.postPoW = (req,res) => {
    let result = blockchain.postPoW(req.body);
    res.setHeader('Content-Type', 'application/json');
    res.send(result)
}
