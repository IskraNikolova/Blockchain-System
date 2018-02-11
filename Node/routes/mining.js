const express = require('express')
const router = new express.Router()
const blockchain = require('./../handlers/blockchain')
const main = require('../index')

//GET Block For Mining
router.get('/get-block/:address', (req, res) => {
    let minerAddress = req.params['address'];
    let miningJob = blockchain.miningJob(minerAddress);

    res.setHeader('Content-Type', 'application/json');
    res.status(200).json(miningJob)
 })

 //POST Block
 router.post('/submit-block/:address', (req, res) => {
    const minerAddress = req.params['address'];
    let isSuccess = blockchain.submitBlock(req, minerAddress);
    res.setHeader('Content-Type', 'application/json');
    if(isSuccess)
    {
        let block = main.miningJobs[minerAddress];
        let exReward = block.expectedReward;
        let result  = {
            "status": "accepted",
            "message": `Block accepted, expected reward: ${exReward} coins`     
        }
        res.status(200).json(result)
    }else{
        res.status(400).json({"message": "error"})
    }
 })

module.exports = router