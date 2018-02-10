class Transaction{
    constructor(addressFrom,
                addressTo,
                value,
                senderPubKey,
                senderSignature,
                transactionHash,
                dateReceived,
                minedInBlock,
                paid){      
                    this.addressFrom = addressFrom;
                    this.addressTo = addressTo;
                    this.value = value;
                    this.senderPubKey = senderPubKey;
                    this.senderSignature = senderSignature;
                    this.transactionHash = transactionHash;
                    this.dateReceived = dateReceived;
                    this.minedInBlock = minedInBlock;
                    this.paid = paid
                }
}

module.exports = Transaction