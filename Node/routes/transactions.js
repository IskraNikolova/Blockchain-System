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
  if(!transaction){
    transaction = main.confirmedTransactions
            .filter(tr => tr.transactionHash == req.params['tranHash'])[0];
  }
  
  res.setHeader('Content-Type', 'application/json'); 
  if(transaction){
    res.status(200).json({
      success: true,
      transaction
    })
  }else{
    res.status(404)
    res.send("Ooops :( Not Found")
    res.end()
  }
})

//POST Send Transaction
router.post('/send', (req, res) => {
  res.setHeader('Content-Type', 'application/json');
      let from = req.body.from;
      let to = req.body.to;
      let value = req.body.value;
      let fee = req.body.fee;
      let dateCreated = req.body.dateCreated;
      let senderPubKey = req.body.senderPubKey;
      let senderSignature = req.body.senderSignature;
    
      //Calculate transaction hash SHA256
      let transactionHash = crypto
          .calculateSHA256([from, to, value, fee, dateCreated, senderPubKey, senderSignature]);

      let minedInBlockIndex = undefined;
      let transferSuccessful = false;
    
      //let sameTransaction = pendingTransactions.getPendingTransactions()
           // .filter(t => t.transactionHash == transactionHash)[0];

      //if(!sameTransaction){
            let transaction = new Transaction(
            from, to, value, fee,
            dateCreated, senderPubKey, senderSignature,
            transactionHash, minedInBlockIndex, transferSuccessful);   
            //TODO Validate transaction & Send to other peers

            pendingTransactions.insertTransaction(transaction);
          
            res.status(201).json({
              message: 'Transaction successfully sent.',
              transactionHash
            })
            res.end()
      //}else{
        //res.status(400).json({
        //  message: "Duplicated Transaction!!!"
       // })  
     // }
})  

module.exports = router