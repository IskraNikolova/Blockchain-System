const express = require('express')
const router = new express.Router()
const handlerNode = require('./../handlers/node')

router.get('/', (req, res) => {
    const info = handlerNode.getNodeInfo(req, res);
    res.status(200).json(info)
 })

module.exports = router