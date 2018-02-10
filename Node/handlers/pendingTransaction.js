let CryptoJS = require("crypto-js");
let main = require('../index');

module.exports.getPendingTransactions = () => {
    return main.pendingTransactions;
}

module.exports.insertTransaction = (transaction) => {
    return main.pendingTransactions.push(transaction);
}