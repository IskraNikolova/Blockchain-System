const express = require('express')
const bodyParser = require('body-parser')
const blocksRoutes = require('./routes/blocks')
const balanceRoutes = require('./routes/balance')
const transactionRoutes = require('./routes/transactions')
const peersRoutes = require('./routes/peers')
const infoRoutes = require('./routes/info')
const miningRoutes = require('./routes/mining')

let Block = require("./models/block")
const app = express()

const port = 5555

app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())

let getGenesisBlock = () => {
    return new Block(
         0, 
         [],
         5, 
         "d279fa6a31ae4fb07cfd9cf7f35cc013cf20a", 
         "f582d57711a618e69d588ce93895d749858fa95b", 
         "5d845cddcd4404ecfd5476fd6b1cf0ea80cd3", 
         2455432, 
         "2018-02-01T23:23:56.337Z", 
         '816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7');
};

module.exports.blockchain = [getGenesisBlock()];
module.exports.pendingTransactions = [];
module.exports.miningJobs = [];
module.exports.difficulty = 5;

// routes
app.use('/info', infoRoutes)
app.use('/blocks', blocksRoutes)
app.use('/balance', balanceRoutes)
app.use('/transactions', transactionRoutes)
app.use('/peers', peersRoutes)
app.use('./mining', miningRoutes)


app.listen(port, () => {
  console.log(`Server running on port ${port}...`)
})