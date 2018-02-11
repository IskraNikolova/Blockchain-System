const express = require('express')
const router = new express.Router()
const crypto = require('./../utils/crypto')
const main = require('../index');
const pendingTransactions = require('./../handlers/pendingTransaction')
const Transaction = require('./../models/transaction')

//GET Transaction Info By Transaction-Hash
router.get('/:tranHash/info', (req, res) => { 
  let transaction = main.pendingTransactions
            .filter(tr => tr.transactionHash == req.params['tranHash'])[0];

  res.setHeader('Content-Type', 'application/json'); 
  if(transaction){
    res.status(200).json({
      success: true,
      transaction
    })
  }else{
    res.status(400)
  }
})

//POST New Transaction
router.post('/new', (req, res) => {
  res.setHeader('Content-Type', 'application/json');
  let from = req.body.from;
  let to = req.body.to;
  let value = req.body.value;
  let senderPubKey = req.body.senderPubKey;
  let senderSignature = req.body.senderSignature;

  let transactionHash = crypto
      .calculateSHA256([from, to, value, senderPubKey, senderSignature]);
  let dateReceived = new Date();
  let minedInBlock = undefined;
  let paid = false;

  let transaction = new Transaction(
      from,
      to,
      value,
      senderPubKey,
      senderSignature,
      transactionHash,
      dateReceived,
      minedInBlock,
      paid);   
  
  pendingTransactions.insertTransaction(transaction);
  result = {
    "dateReceived": new Date(transaction.dateReceived).toISOString(),
    "transactionHash": transaction.transactionHash
  }

  res.status(200).json({
    success: true,
    message: 'Transaction posted successfuly.',
    result
  })
})  

module.exports = router