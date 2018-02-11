const express = require('express')
const router = new express.Router()
const main = require('./../index');

//GET All blocks
router.get('/', (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    let blocks = main.blockchain;
    res.status(200).json(blocks)
 })

 //GET block by index
 router.get('/:index', (req, res) => {
    let index = req.params['index'];
    const block = main.blockchain.find(b => b.index == index);
    res.setHeader('Content-Type', 'application/json');   
    if(block){
      res.send(block)
      res.status(200).json(block)
    }else{
      res.status(404)
      res.end()
    }
 })

 //POST Notification for new block
 router.post('/notify', (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    const newBlockIndex = req.body.index;
//TODO
    res.status(200).json({
      success: true,
    })
  })  

 module.exports = router