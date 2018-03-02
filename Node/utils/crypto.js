const cryptoJs = require('crypto-js')
const ecdsa = require('elliptic')
const ec = new ecdsa.ec('secp256k1')
const RPMD160 = require('ripemd160')
const {randomBytes} = require('crypto')
const secp256k1 = require('secp256k1')

module.exports.calculateSHA256 = (obj) => {
    return cryptoJs.SHA256(obj).toString()
}

module.exports.hashAndBuffer = (obj) => {
    let hash = this.calculateSHA256(obj);
    return this.converHexToUint(hash)
}

module.exports.checkSign = (message, signature, publicKey) => {
    let sign = secp256k1.signatureImport(signature);
    let messageToCheck = this.hashAndBuffer(message);
    let keyAsArray = this.converHexToUint(publicKey);
    let result = secp256k1
        .verify(messageToCheck, sign, keyAsArray);

    return result
}

module.exports.convertUIntToHex = (uint) => {
    let hex = Buffer.from(uint).toString('hex');
    return hex
}

module.exports.converHexToUint = (text) => {
    let buffer = Buffer
        .from(text.toString(cryptoJs.enc.Hex), 'hex');
    let array = new Uint8Array(buffer);
    return array
}

