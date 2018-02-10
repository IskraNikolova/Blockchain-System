const express = require('express')
const router = new express.Router()

router.get('/', (req, res) => {
    const peers = 
    [
        "http://212.50.11.109:5555",
        "http://af6c7a.ngrok.org:5555"
    ]
      
      
    res.status(200).json(peers)
 })

 router.post('/', (req, res) => {
    const peer = 
    {
        "url": "http://212.50.11.109:5555"
    }
      
    res.status(200).json({
      success: true,
      message: 'Added peer...',
    })
  })

module.exports = router