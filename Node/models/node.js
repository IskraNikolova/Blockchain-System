class Node {
    constructor(peers, 
        blocks, 
        pendingTransactions, 
        difficulty, 
        miningJobs){
            this.peers = peers;
            this.blocks = blocks;
            this.pendingTransactions = pendingTransactions;
            this.difficulty = difficulty;
            this.miningJobs = miningJobs;
    }
}

module.exports = Node