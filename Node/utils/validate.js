module.exports.validateBlockHash = (blockHashForCheck, blockDataHash, difficulty) => {
    let isValidDifficulty = blockDataHash.substr(0, difficulty);
    let validString = '0'.repeat(difficulty);
    if(blockDataHash === blockHashForCheck && 
        isValidDifficulty == validString){
        return true
    }
    return false;
}
