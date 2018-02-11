let main = require('../index');

module.exports.getPendingTransactions = () => {
    return main.pendingTransactions;
}

module.exports.insertTransaction = (transaction) => {
    main.pendingTransactions.push(transaction);
}