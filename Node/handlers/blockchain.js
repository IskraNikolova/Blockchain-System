const Block = require("../models/block")
const Transaction = require("../models/transaction")
const MiningJob = require("../models/miningJob")
const main = require('../index');
const crypto = require('./../utils/crypto')
const validator = require('./../utils/validate')

module.exports.calculateHash = (index, prevBlockHash, dateCreated, transactions, nonce) => {
    return crypto.calculateSHA256([index, prevBlockHash, dateCreated, transactions, nonce]);
}

module.exports.calculateHashForBlock = (block) => {
    let blockHash = this.calculateHash(
        block.index, 
        block.prevBlockHash, 
        block.dateCreated, 
        block.transactions, 
        block.nonce);

    return blockHash;
}

module.exports.addBlock = (newBlock) => {
    if (this.isValidNewBlock(newBlock, this.getLatestBlock())) {
        let transactions = newBlock.transactions;
        let index = newBlock.index;

        for(let i = 0; i < 0; i++){
           transactions[i].paid = true;
           transactions[i].index = index;
        }

        console.log(transactions);//CHECK this
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
                "",            
                Date.now(),    
                index,         
                false);

    let pendingTransactions = main.pendingTransactions;
    pendingTransactions.push(coinBaseTransaction);

    let transactions = pendingTransactions;
    let prevBlockHash = this.calculateHashForBlock(this.getLatestBlock());

    let blockDataHash = crypto.calculateSHA256([
        index, 
        transactions, 
        main.difficulty, 
        prevBlockHash, 
        minerAddress]);

    let jobForMining = new MiningJob(
        index, 
        transactions, 
        main.difficulty,
        prevBlockHash,
        blockDataHash);

    main.miningJobs[minerAddress] = jobForMining;

    let sentMiningJob = {
        index,
        transactionIncuded: transactions.length,
        expectedReward,
        blockDataHash
    }

    return sentMiningJob;
}

module.exports.submitBlock = (req, minerAddress) => {
    const nonce = req.body.nonce;
    const dateCreated = req.body.dateCreated;
    const blockHash = req.body.blockHash;

    let miningJob = main.miningJobs[minerAddress];
    let difficulty = main.difficulty;
    let blockDataHash = miningJob.blockDataHash;
    let blockHashForCheck = crypto.calculateSHA256([blockDataHash, nonce, dateCreated]);//toISO
    let isValid = validator.validateBlockHash(blockHashForCheck, blockHash, difficulty);

    if(isValid){
        let newBlock = new Block(
            miningJob.index,
            miningJob.transactions,
            difficulty,
            miningJob.prevBlockHash,
            minerAddress,
            blockDataHash,
            nonce,
            dateCreated,
            blockHash
        )

        this.addBlock(newBlock);
        return true;
    }

    return false;
}

module.exports.getLatestBlock = () => main.blockchain[main.blockchain.length - 1];