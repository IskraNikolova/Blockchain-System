class MiningJob {
    constructor(index, 
        expectedReward, 
        transactions, 
        transactionsHash, 
        prevBlockHash, 
        difficulty){
            this.index = index;
            this.expectedReward = expectedReward;
            this.transactions = transactions;
            this.transactionsHash = transactionsHash;
            this.prevBlockHash = prevBlockHash;
            this.difficulty = difficulty;
    }
}

module.exports = MiningJob