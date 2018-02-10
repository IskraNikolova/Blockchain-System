const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/', (req, res) => {
    const blocks = handlerNode.getAllBlocks(req, res)
    res.status(200).json(blocks)
 })

 router.get('/:index', (req, res) => {
    let index = req.params['index'];
    const block = handlerNode.getBlockByIndex(req, res, index);
    res.status(200).json(block)
 })

 router.post('/notify', (req, res) => {
    let message = handlerNode.postNotifyForBlock(req, res);

    res.status(200).json({
      success: true,
      message
    })
  })  


 module.exports = router