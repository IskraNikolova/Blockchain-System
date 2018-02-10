class Node {
    constructor(peers, 
        blocks, 
        pendingTransactions, 
        balances, 
        difficulty, 
        miningJobs){
            this.peers = peers;
            this.blocks = blocks;
            this.pendingTransactions = pendingTransactions;
            this.balances = balances;
            this.difficulty = difficulty;
            this.miningJobs = miningJobs;
    }
}

module.exports = Node