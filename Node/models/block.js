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
            this.difficulty = difficulty;
            this.prevBlockHash = prevBlockHash.toString();
            this.minedBy = minedBy;
            this.blockDataHash = blockDataHash.toString();         
            this.nonce = nonce;
            this.dateCreated = dateCreated;
            this.blockHash = blockHash;
    }
}

module.exports = Block