let blockchain = require('./blockchain');
let main = require('../index');
let pendingTransactions = require('./pendingTransaction');
let CryptoJS = require("crypto-js");
let crypto = require('./utils/crypto')
let Transaction = require('./../models/transaction')

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
    const newBlockIndex = req.body.index;

    res.send(
        {
            "message": "Thank you!" 
        }
    )
}

//GET Transaction
module.exports.getTransactionInfo = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    console.log(req.params)
    
    let transaction = main.pendingTransactions.filter(tr => tr.transactionHash == req.params['tranHash'])[0];
    console.log(transaction)
    res.send(
        transaction
    )
}

//POST Transaction
module.exports.postTransaction = (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    let from = req.body.from;
    let to = req.body.to;
    let value = req.body.value;
    let senderPubKey = req.body.senderPubKey;
    let senderSignature = req.body.senderSignature;

    let transactionHash = crypto.calculateSHA256([from, to, value, senderPubKey, senderSignature])
    let dateReceived = new Date();
    let minedInBlock = 0;
    let paid = false;

    let transaction = new Transaction(
        from,
        to,
        value,
        senderPubKey,
        senderSignature,
        transactionHash,
        dateReceived,
        minedInBlock,
        paid
    );   
    
    if (transaction){
       pendingTransactions.insertTransaction(transaction);
    }

    res.send(
        {
            "dateReceived": new Date(transaction.dateReceived).toISOString(),
            "transactionHash": transaction.transactionHash
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