const Block = require("../models/block")
const Transaction = require("../models/transaction")
const MiningJob = require("../models/miningJob")
const main = require('../index');
const crypto = require('./../utils/crypto')
const validator = require('./../utils/validate')

module.exports.miningJob = (minedBy) => {	
    let value = 5000350;
    let index = this.getLatestBlock().index + 1;

    //Get coinbase transaction
    let coinBaseTransaction = this.getCoinBaseTransaction(index, minedBy, value);


    let pendingTransactions = main.pendingTransactions;
    let transactions = pendingTransactions;
    main.pendingTransactions = main.pendingTransactions.filter(tr => tr.transferSuccessful == false);

    //Calculate previous block hash
    let prevBlockHash = this.calculateHashForBlock(this.getLatestBlock()); 
    let difficulty = main.difficulty;

    let blockDataHash = crypto.calculateSHA256({index, transactions: transactions.unshift(coinBaseTransaction), difficulty, 
                               prevBlockHash, minedBy});

    let jobForMining = new MiningJob(index, transactions, difficulty,
                               prevBlockHash, blockDataHash);

    main.miningJobs[minedBy] = jobForMining;

    let sentMiningJob = {
        index,
        transactionsIncluded: transactions.length,
        difficulty,
        expectedReward: value,
        blockDataHash
    }

    return sentMiningJob;
}

module.exports.submitBlock = (req, minedBy) => {
    const nonce = req.body.nonce;
    const dateCreated = req.body.dateCreated;
    const blockHash = req.body.blockHash;

    let miningJob = main.miningJobs[minedBy];
    let difficulty = main.difficulty;
    let blockDataHash = miningJob.blockDataHash;
    let blockHashForCheck = crypto.calculateSHA256({blockDataHash, nonce, dateCreated});
    //let isValid = validator.validateBlockHash(blockHashForCheck, blockHash, difficulty);

   //if(isValid){
        let newBlock = new Block(
            miningJob.index,
            miningJob.transactions,
            difficulty,
            miningJob.prevBlockHash,
            minedBy,
            blockDataHash,
            nonce,
            dateCreated,
            blockHash
        )

        this.addBlock(newBlock);
        main.broadcast(main.responseLatestMsg());
        return true;
    //}

   // return false;
}

module.exports.addBlock = (newBlock) => {
    //if (this.isValidNewBlock(newBlock, this.getLatestBlock())) {
        let transactions = newBlock.transactions;
        let index = newBlock.index;
        
        for(let i = 0; i < transactions.length; i++){
           transactions[i].minedInBlockIndex = index;
           transactions[i].transferSuccessful = true;
        }
        
        main.confirmedTransactions = transactions;
        main.pendingTransactions = main.pendingTransactions.filter(tr => tr.transferSuccessful == false);

        newBlock.transactions = transactions;
        main.blockchain.push(newBlock);
    //}
}

module.exports.getLatestBlock = () => main.blockchain[main.blockchain.length - 1];

module.exports.calculateHashForBlock = (block) => {
    let blockHash = this.calculateHash(
        block.index, 
        block.transactions,
        block.difficulty,
        block.prevBlockHash, 
        block.minedBy,
        block.blockDataHash,
        block.nonce,
        block.dateCreated, 
       );
       
    return blockHash;
}

module.exports.calculateHash = (index, prevBlockHash, dateCreated, transactions, nonce) => {
    return crypto.calculateSHA256({index, prevBlockHash, dateCreated, transactions, nonce});
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

module.exports.getCoinBaseTransaction = (index, minedBy, value) => 
{
    let date = new Date();
    date.toISOString();
    let from = "0000000000000000000000000000000000000000";
    let dateCreated = date;
    let to = minedBy;
    let fee = 0;

    let coinBaseTransactionHash = crypto.calculateSHA256({from, to, value, fee, dateCreated});

    let coinBaseTransaction = new Transaction(from, to, value, 
                              0, dateCreated, "", "", coinBaseTransactionHash, index, true);

    return coinBaseTransaction;
}