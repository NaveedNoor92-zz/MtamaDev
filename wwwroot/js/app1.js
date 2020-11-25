//import { ethers } from 'ethers';

App1 = {

    init: async function () {
        let data = await $.getJSON('/abi/Mtama2.json');
        //let ropstenProvider = ethers.getDefaultProvider('ropsten');
        let provider = new ethers.providers.Web3Provider(web3.currentProvider);
        let contractAddress = "0x7Fb7b84ce2f7C5e1fdBca22b8f1546d064eD4a34";
        //let privateKey = 'E7b0c41d261791a3bf7c76725003625439a364885313b786bb76a9b52e1af99e';
        let privateKey = "72d66c6568508f781c1131555489111c725697da62c26bbd137944fc0e0e5bb1"; //local

        let wallet = new ethers.Wallet(privateKey, provider);
        var contract = new ethers.Contract(contractAddress, data.abi, wallet);
        
        let _gasPrice = await provider.getGasPrice();
        var _price = App1.convertPrice(15000);
        //let tx = await contract.hello();
        let tx = await contract.startEscrow2("000001", "0xc6264F854fC0B21cfc6e30E9e6BC8683c7d0ad44", {
            gasPrice: _gasPrice, gasLimit: ethers.utils.hexlify(8000000), value: _price });
        //let tx = await contract.hello();
        console.log(tx.hash);
        var result = await tx.wait();
        console.log(result);
    },

    convertPrice: function (fiatPrice) {
        var price = parseInt(fiatPrice);
        price = (price / 15000) * Math.pow(10, 18); //assuming 1 ETH = 15000 KSH
        return price;
    }
};