class Transaction{
    constructor(addressFrom,
                addressTo,
                value,
                senderPubKey,
                senderSignature,
                transactionHash,
                dataReceived,
                minedInBlock,
                paid){
                  
                    this.addressFrom = addressFrom;
                    this.addressTo = addressTo;
                    this.value = value;
                    this.senderPubKey = senderPubKey;
                    this.senderSignature = senderSignature;
                    this.transactionHash = transactionHash;
                    this.dataReceived = dataReceived;
                    this.minedInBlock = minedInBlock;
                    this.paid = paid;
                }
}