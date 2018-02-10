class Block {
    constructor(index, 
        transactions, 
        difficulty, 
        prevBlockHash, 
        minedBy, 
        blockDataHash, 
        nonce, 
        dateCreated, 
        blockHash) {

            this.index = index;
            this.transactions = transactions;
            this.prevBlockHash = prevBlockHash.toString();
            this.blockDataHash = blockDataHash.toString();
            this.minedBy = minedBy;
            this.nonce = nonce;
            this.difficulty = difficulty;
            this.dateCreated = dateCreated;
            this.blockHash = blockHash;
    }
}

module.exports = Block