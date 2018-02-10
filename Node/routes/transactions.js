const express = require('express')
const router = new express.Router()

router.get('/:tranHash/info', (req, res) => {
    const transactionInfo = 
    {
        "from": req.params['tranHash'],
        "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
        "value": 25.00,
        "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
        "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"],
        "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b…176a",
        "paid": true,
        "dateReceived": "2018-02-01T07:47:51.982Z",
        "minedInBlockIndex": 7
      }
      
    res.status(200).json(transactionInfo)
 })
      //TODO
router.post('/new', (req, res) => {
    const transaction = 
    {
        "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
        "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
        "value": 25.00,
        "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
        "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"]
      }

      const result = { 
        "dateReceived": "2018-02-01T23:17:02.744Z",
        "transactionHash": "cd8d9a345bb208c6f9b8acd6b8eefe6…20c8a" }      

    res.status(200).json({
      success: true,
      message: 'Product editted successfuly.',
      result
    })
  })  

module.exports = router