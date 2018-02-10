let Block = require("../models/block")
let Transaction = require("../models/transaction")
let MiningJob = require("../models/miningJob")
let CryptoJS = require("crypto-js");
let main = require('../index');
let crypto = require('./utils/crypto')

module.exports.calculateHash = (index, prevBlockHash, dateCreated, transactions, nonce) => {
    return crypto.calculateSHA256([index, prevBlockHash, dateCreated, transactions, nonce]);
    //return CryptoJS.SHA256(index + prevBlockHash + dateCreated + transactions + nonce).toString();
}

module.exports.calculateHashForBlock = (block) => {
    return this.calculateHash(block.index, block.prevBlockHash, block.dateCreated, block.transactions, block.nonce);
}

module.exports.addBlock = (newBlock) => {
    if (this.isValidNewBlock(newBlock, this.getLatestBlock())) {
        main.blockchain.push(newBlock);
    }
}

module.exports.isValidNewBlock = (newBlock, previousBlock) => {
    if (previousBlock.index + 1 !== newBlock.index) {
        console.log('Invalid index!');
        return false;
    }

    if (previousBlock.hash !== newBlock.previousHash) {
        console.log('Invalid previous block hash!');
        return false;
    }

    if (this.calculateHashForBlock(newBlock) !== newBlock.hash) {
        console.log(typeof (newBlock.hash) + ' ' + typeof this.calculateHashForBlock(newBlock));
        console.log('Invalid hash: ' + this.calculateHashForBlock(newBlock) + ' ' + newBlock.hash);
        return false;
    }

    return true;
}

module.exports.miningJob = (minerAddress) => {	
    let expectedReward = 25;
    let index = this.getLatestBlock().index + 1;

    let coinBaseTransaction = new Transaction(
                "0x0",          
                minerAddress,   
                expectedReward, 
                "",            
                "",             
                Date.now(),    
                index,         
                false );

    let pendingTransactions = main.pendingTransactions;
    pendingTransactions.push(coinBaseTransaction);

    let transactions = pendingTransactions.length;
    let transactionsHash = CryptoJS.SHA256(transactions).toString();
    let prevBlockHash = this.calculateHashForBlock(this.getLatestBlock());

    let jobForMining = new MiningJob(
        index, 
        expectedReward, 
        transactions, 
        prevBlockHash);

    main.miningJobs[minerAddress] = jobForMining;

    return jobForMining;
}


module.exports.getLatestBlock = () => main.blockchain[main.blockchain.length - 1];