const express = require('express')
const bodyParser = require('body-parser')
const blocksRoutes = require('./routes/blocks')
const balanceRoutes = require('./routes/balance')
const transactionRoutes = require('./routes/transactions')
const peersRoutes = require('./routes/peers')
const infoRoutes = require('./routes/info')

const app = express()

const port = 5555

app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())

// routes
app.use('/info', infoRoutes)
app.use('/blocks', blocksRoutes)
app.use('/balance', balanceRoutes)
app.use('/transactions', transactionRoutes)
app.use('/peers', peersRoutes)


app.listen(port, () => {
  console.log(`Server running on port ${port}...`)
})