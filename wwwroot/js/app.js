
const contractAddress = "0xfdFD40896748d659F521a5A7B49b0FF0fbad8465"; //ROPSTEN
//const contractAddress = "0x7Fb7b84ce2f7C5e1fdBca22b8f1546d064eD4a34"; //LOCAL

App = {
    web3Provider: null,
    contract: null,
    contractInstance: null,   //ArtToken instance
    account: null, //current metamask account
    tokenContractOwner: null, //account who deployed the token contract (optional)
    emptyAdress: "0x0000000000000000000000000000000000000000",

    gasLimit: 4000000, //4 million
    gasPrice: 20000000000, //50 GWEI - price per unit. 40 GWEI is hgih, 20GWEI is moderate

    initMetamask: async function() {    
        await App.initWeb3();    
    },

    initWeb3: async function() {
        // Is there an injected web3 instance?
        if (typeof web3 !== 'undefined') {
            App.web3Provider = web3.currentProvider;
        } else {
            // If no injected web3 instance is detected, fall back to Ganache //this fallback is not secure for production scenarios
            //App.web3Provider = new Web3.providers.HttpProvider('http://localhost:7545');
            //alert("Make sure to connect MetaMask browser extension.");
            console.log("Make sure to connect MetaMask browser extension.");
            App.noMetaMask();
            return;
        }
    
        web3 = new Web3(App.web3Provider);

        App.initAccount();	
    },

    initAccount: function() {
        //init Account details    
        web3.eth.getAccounts(function(error, accounts) {
            if (error) {
            console.log(error);
            alert("Error in getting account details.");
            return;
            }

            App.account = accounts[0];
            console.log("Current Account: " + App.account);

            if (App.account === undefined) { //metamask installed but not signed in scenario
                App.noMetaMask();
            }
            else {
                $("#txtWalletAddress").val(App.account);
            }
        });
    },

    noMetaMask: function () {        
        alert("Please sign into Metamask.");
    },

    initContract: async function() {
      try {

          if (App.contractInstance === null) {
              let data = await $.getJSON('/abi/mtama2.json');
              App.contract = TruffleContract(data);              
              App.contract.setProvider(App.web3Provider);
              App.contractInstance = await App.contract.at(contractAddress);
              //App.contractInstance = new web3.eth.Contract(data.abi, contractAddress);
          }
      }
      catch (err) {
          throw err;
      }
    },

    startEscrow: async function () {
        try {
            $("#btnSend").hide();

            await App.initContract();

            var _guid = $("input.clsTxGuid").val();
            if (!_guid) throw new Error("Unique Transaction Guid not generated");

            var _to = $("#txtReceiverWallet").val();
            if (!_to) throw new Error("Wallet Address for Receiver not found");

            //var _priceInEther = await App.convertPrice(1500);
            //$("#txtAmountInEther").val(_priceInEther);
            var _priceInEther = $('#AmountInEther').val();
            var _priceInWei = _priceInEther * Math.pow(10, 18);

            App.showWait();

            let result = await App.contractInstance.startEscrow(_guid, _to, { from: App.account, value: _priceInWei, gas: App.gasLimit, gasPrice: App.gasPrice });
            if (result.receipt.status === 0) {
                console.log("Error in Starting Escrow: " + result);
                throw new Error("Transaction Receipt Error. Status = 0");
            } else {
                if (result.receipt.transactionHash) {
                    $("input.clsTxHash").val(result.receipt.transactionHash);
                    $("#frmMakePayments").submit();
                } else { alert("Blockchain Transaction Hash not found."); }
            }
            
        }
        catch (ex) {
            console.log('Error in Starting Escrow: ' + ex);
            alert(ex);
            $("#btnSend").show();
            App.hideWait();
        }
    },

    showWait: function () {
        $('.loading-overlay').show();
    },
    hideWait: function () {
        $('.loading-overlay').hide();
    },

    upload: function (attachment1, attachment, btnsubmit) {
        document.getElementById(attachment1).disabled = true;

        var blobUri = "https://mtamadev.blob.core.windows.net/";
        //var sas = "?sv=2019-12-12&ss=bfqt&srt=co&sp=rwlacupx&se=2021-07-28T20:13:11Z&st=2020-07-28T12:13:11Z&spr=https&sig=pncBkCZpWBTilzNPYdLKEhZAtkjhGKY3YZBysdZOD1U%3D";
        var container = "payments";

        var transactionId = document.getElementById('inputId').value;
        var file = document.getElementById(attachment1).files[0];

        if (file == null || file == undefined) {
            alert("Please select a file to upload.");
            return;
        }


        showSpinnerViewTransaction();
        
        var temp = attachment == "senderattachment" ? "s" : "r";
        transactionId = temp + transactionId + "_" + file.name;
        var fileurl = blobUri + container + '/' + transactionId;

        var blobService = AzureStorage.Blob.createBlobServiceWithSas(blobUri, sas);

        //use block sizes of 4MB for file sizes > 32 MB. otherwise 512KB
        var customBlockSize = file.size > 1024 * 1024 * 32 ? 1024 * 1024 * 4 : 1024 * 512;
        blobService.singleBlobPutThresholdInBytes = customBlockSize;

        var finishedOrError = false;
        blobService.createBlockBlobFromBrowserFile(container, transactionId, file, { blockSize: customBlockSize }, function (error, result, response) {
            finishedOrError = true;
            if (error) {
                // Upload blob failed
               alert("Upload Failed. Please reload the page. ");
                // alert(error);
                exitSpinnerViewTransaction();

            } else {
                // Upload successfully
                document.getElementById(attachment).value = fileurl + sas;
                document.getElementById(btnsubmit).click();


            }
        });

   
    },

    uploadAttachment: function (attachment1, attachment, btnsubmit) {
        //document.getElementById(attachment1).disabled = true;

        var blobUri = "https://mtamadev.blob.core.windows.net/";
        var sas = document.getElementById("sasToken").value;
        var container = "payments";

        var file = document.getElementById(attachment1).files[0];

        if (file == null || file == undefined) {
            alert("Please choose a file to upload.");
            return;
        }

        showSpinnerAttachment();
        var transactionId = file.name;
        var fileurl = blobUri + container + '/' + transactionId;
        var blobService = AzureStorage.Blob.createBlobServiceWithSas(blobUri, sas);

        //use block sizes of 4MB for file sizes > 32 MB. otherwise 512KB
        var customBlockSize = file.size > 1024 * 1024 * 32 ? 1024 * 1024 * 4 : 1024 * 512;
        blobService.singleBlobPutThresholdInBytes = customBlockSize;

        var finishedOrError = false;
        blobService.createBlockBlobFromBrowserFile(container, transactionId, file, { blockSize: customBlockSize }, function (error, result, response) {
            finishedOrError = true;
            if (error) {
                // Upload blob failed
                alert("Upload Failed. Please reload the page. ");
                $('#spinnerattachment').hide();
                // alert(error);

            } else {
                // Upload successfully
                document.getElementById(attachment).value = fileurl + sas;
                document.getElementById(btnsubmit).click();


            }
        });


    },


    
};