class Transaction{
    constructor(from,
                to,
                value,
                fee,
                dateCreated,
                senderPubKey,
                senderSignature,
                transactionHash,
                minedInBlockIndex,
                transferSuccessful){      
                    this.from = from;
                    this.to = to;
                    this.value = value;
                    this.fee = fee;
                    this.dateCreated = dateCreated
                    this.senderPubKey = senderPubKey;
                    this.senderSignature = senderSignature;
                    this.transactionHash = transactionHash;
                    this.minedInBlockIndex = minedInBlockIndex;
                    this.transferSuccessful = transferSuccessful
                }
}

module.exports = Transaction