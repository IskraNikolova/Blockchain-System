const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/:tranHash/info', (req, res) => {
    const transactionInfo = handlerNode.getTransactionInfo(req, res);     
    res.status(200).json(transactionInfo)
})

//TODO
router.post('/new', (req, res) => {
    const result = handlerNode.postTransaction(req, res);  

    res.status(200).json({
      success: true,
      message: 'Product editted successfuly.',
      result
    })
  })  

module.exports = router