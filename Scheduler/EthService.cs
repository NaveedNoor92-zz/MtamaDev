using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json.Linq;

namespace Mtama.Scheduler
{
    public class EthService
    {
        Web3 _web3 = null;
        Contract _contract = null;
        HexBigInteger _gas = null;
        string _contractOwner = null;
        public async void Init()
        {
            _contractOwner = ConfigurationManager.GetAppSetting("Eth:Owner");

            string contractKey = ConfigurationManager.GetAppSetting("Eth:Key");            
            string contractNetwork = ConfigurationManager.GetAppSetting("Eth:Network");
            string contractAddress = ConfigurationManager.GetAppSetting("Eth:Contract");

            var dir = Startup.WebRootPath;
            var contractFile = dir + @"\abi\mtama2.json";
            var contractJson = File.ReadAllText(contractFile);
            if (String.IsNullOrEmpty(contractJson)) throw new Exception("Contract Json could not be read.");

            var bytecode = "";
            var abi = "";

            JObject jo = JObject.Parse(contractJson);
            foreach (JProperty prop in jo.Properties())
            {
                if (prop.Name == "abi")
                {
                    abi = prop.Value.ToString();
                }
                if (prop.Name == "bytecode")
                {
                    bytecode = prop.Value.ToString();
                }
            }

            var account = new Account(contractKey);
            _web3 = new Web3(account, contractNetwork);

            _contract = _web3.Eth.GetContract(abi, contractAddress);            
        }

        public async Task<string> Execute(string txGuid, bool isRefund)
        {
            var functionParams = new object[] { txGuid, isRefund};

            var releaseEscrowFunction = _contract.GetFunction("releaseEscrow");
            _gas = await releaseEscrowFunction.EstimateGasAsync(_contractOwner, null, null, functionParams).ConfigureAwait(false);

            //var receipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(Owner, gas, null, null, Cap).ConfigureAwait(false);
            var txHash = await releaseEscrowFunction.SendTransactionAsync(_contractOwner, _gas, null, null, functionParams).ConfigureAwait(false);
            return txHash;
        }
        
    }
}
