class MiningJob {
    constructor(
        index, 
        transactions, 
        difficulty,
        prevBlockHash,
        blockDataHash){
            this.index = index;
            this.transactions = transactions;
            this.difficulty = difficulty;
            this.prevBlockHash = prevBlockHash;
            this.blockDataHash = blockDataHash;
    }
}

module.exports = MiningJob