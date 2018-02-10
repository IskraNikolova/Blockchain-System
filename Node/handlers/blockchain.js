let Block = require("../models/block")
let Transaction = require("../models/transaction")
let MiningJob = require("../models/miningJob")
let CryptoJS = require("crypto-js");
let main = require('../index');

module.exports.calculateHash = (index, prevBlockHash, dateCreated, transactions, nonce) => {
    return CryptoJS.SHA256(index + prevBlockHash + dateCreated + transactions + nonce).toString();
}

module.exports.calculateHashForBlock = (block) => {
    return this.calculateHash(block.index, block.prevBlockHash, block.dateCreated, block.transactions, block.nonce);
}

module.exports.addBlock = (newBlock) => {
    if (this.isValidNewBlock(newBlock, this.getLatestBlock())) {
        main.blockchain.push(newBlock);
    }
}

module.exports.getLatestBlock = () => main.blockchain[main.blockchain.length - 1];

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

module.exports.generateNextBlock = (transactions) => {
    let previousBlock = this.getLatestBlock();
    let nextIndex = previousBlock.index + 1;
    let nextTimestamp = new Date().getTime() / 1000;
    let nextHash = this.calculateHash(
         nextIndex, 
         previousBlock.blockHash,
         nextTimestamp, 
         transactions);

    return new Block(
        nextIndex,
         previousBlock.blockHash,
          nextTimestamp,
           transactions,
            nextHash);
}

module.exports.miningJob = (minerAddress) => {	
    let expectedReward = 25;
    let index = this.getLatestBlock().index + 1;

    let coinBaseTransaction = new Transaction(
        "0x0",          // fromAddress
        minerAddress,   // toAddress
        expectedReward, // transactionValue,
        "",             // senderPubKey
        "",             //  senderSignature,
        "",             // transactionHash,
        Date.now(),     // dateReceived,
        index,          // minedInBlockIndex,
        false           // paid
    );

    let pendingTransactions = main.pendingTransactions;
    pendingTransactions.push(coinBaseTransaction);

    let transactions = pendingTransactions;
    let transactionsHash = CryptoJS.SHA256(transactions).toString();
    let prevBlockHash = this.calculateHashForBlock(this.getLatestBlock());

    let jobForMining = new MiningJob(index, 
        expectedReward, 
        transactions, 
        transactionsHash, 
        prevBlockHash , 
        main.difficulty);

    main.miningJobs[minerAddress] = jobForMining;

    return jobForMining;
}

module.exports.isValidPoW = (pow) => {
    let miningJob = main.miningJobs[pow.minedBy];
    let validHash = this.calculateHash(
        miningJob.index, 
        miningJob.prevBlockHash, 
        pow.timestamp, 
        miningJob.transactionsHash, 
        pow.nonce)

    return pow.blockHash === validHash && this.powDetect(pow.blockHash) ;
}

module.exports.powDetect = (hash) => {
        for (var i = 0, b = hash.length; i < b; i ++) {
            if (hash[i] !== '0') {
                break;
            }
        }
        return i === main.difficulty;
}

module.exports.postPoW = (pow) => {
    let newBlock = new Block(
        main.miningJobs[pow.minedBy].index,
        main.miningJobs[pow.minedBy].transactions,
        main.miningJobs[pow.minedBy].difficulty,
        main.miningJobs[pow.minedBy].prevBlockHash,
        pow.minedBy,
        main.miningJobs[pow.minedBy].transactionsHash,
        pow.nonce,
        pow.dateCreated,
        pow.blockHash
    );

    let previousBlock = this.getLatestBlock();

    if (this.isValidNewBlock(newBlock, previousBlock)){
        main.miningJobs[pow.minedBy].transactions.forEach((transaction)=> {
            main.pendingTransactions = main.pendingTransactions.filter(function( tran ) {
                return tran.index !== transaction.index;
            });
        })

        main.blockchain.push(newBlock);
        main.miningJobs[pow.minedBy] = '';
    }

    return pow;
}
