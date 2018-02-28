const main = require('./../index');

module.exports.getBalanceWithConfirm = (userAddress, confirmationIndex) => {
  let blockChain = main.blockchain;
  let balanceWitConfirmation = {
      confirmations: confirmationIndex,
      balance: 0
  };
  let maxConfirmation = confirmationIndex;

  for(let blockIndex = blockChain.length - 1; blockIndex >= 0; blockIndex--){
      let block = blockChain[blockIndex];
      block.transactions.forEach(transaction => {
        if(transaction.from == userAddress || 
          transaction.to == userAddress){
            let confirmation = blockChain.length - blockIndex;
            if(confirmation >= confirmationIndex){
                if(maxConfirmation < confirmation){
                    maxConfirmation = confirmation;
                }
                if(transaction.from == userAddress){
                    balanceWitConfirmation.balance -= transaction.value;
                }else if(transaction.to == userAddress){
                    balanceWitConfirmation.balance += transaction.value;
                }
            }
        }
      });
   }

   balanceWitConfirmation.confirmations = maxConfirmation;
   return balanceWitConfirmation;
}

module.exports.getBalanceWithPendingTransaction = (userAddress) => {
    let blockChain = main.blockchain;
    let pendingTransactions = main.pendingTransactions;
    let balance = 0;

    blockChain.forEach(block => {
        block.transactions.forEach(transaction => {
            if(transaction.from == userAddress){               
                balance -= Number(transaction.value);
            }else if(transaction.to == userAddress){        
                balance += Number(transaction.value);            
            }
        });
    });

    pendingTransactions.forEach(transaction => {
        if(transaction.from == userAddress){
            balance -= Number(transaction.value);
        }else if(transaction.to == userAddress){
            balance += Number(transaction.value);
        }
    });

     return balance;
}