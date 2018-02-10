const express = require('express')
const router = new express.Router()

router.get('/', (req, res) => {
    const blocks = [
        {
            "index": 17,
            "transactions": [{
                "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
                "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
                "value": 25.00,
                "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
                "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"],
                "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b…176a",
                "paid": true,
                "dateReceived": "2018-02-01T07:47:51.982Z",
                "minedInBlockIndex": 7
              }],
            "difficulty": 5,
            "prevBlockHash": "d279fa6a31ae4fb07cfd9cf7f35cc01f…3cf20a",
            "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
            "blockDataHash": "5d845cddcd4404ecfd5476fd6b1cf0e5…a80cd3",
            "nonce": 2455432,
            "dateCreated": "2018-02-01T23:23:56.337Z",
            "blockHash": "00000abf2f3d86d5c000c0aa7a425a6a4a65…cf4c"
        }
    ]

    res.status(200).json(blocks)
 })

 router.get('/:index', (req, res) => {
    const block = [
        {
            "index": req.params['index'],
            "transactions": [{
                "from": "44fe0696beb6e24541cc0e8728276c9ec3af2675",
                "to": "9a9f082f37270ff54c5ca4204a0e4da6951fe917",
                "value": 25.00,
                "senderPubKey": "2a1d79fb8743d0a4a8501e0028079bcf82a4f…eae1",
                "senderSignature": ["e20c…a3c29d3370f79f", "cf92…0acd0c132ffe56"],
                "transactionHash": "4dfc3e0ef89ed603ed54e47435a18b836b…176a",
                "paid": true,
                "dateReceived": "2018-02-01T07:47:51.982Z",
                "minedInBlockIndex": 7
              }],
            "difficulty": 5,
            "prevBlockHash": "d279fa6a31ae4fb07cfd9cf7f35cc01f…3cf20a",
            "minedBy": "f582d57711a618e69d588ce93895d749858fa95b",
            "blockDataHash": "5d845cddcd4404ecfd5476fd6b1cf0e5…a80cd3",
            "nonce": 2455432,
            "dateCreated": "2018-02-01T23:23:56.337Z",
            "blockHash": "00000abf2f3d86d5c000c0aa7a425a6a4a65…cf4c"
        }
    ]

    res.status(200).json(block)
 })

 router.post('/notify', (req, res) => {
    const newBlockIndex = 
    {
        "index": 9987
    }

    res.status(200).json({
      success: true,
      message: 'Thank you!',
    })
  })  


 module.exports = router