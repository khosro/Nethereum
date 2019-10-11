using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Xunit;
using Nethereum.Web3.Accounts.Managed;

namespace Nethereum.Tutorials
{
    public class Deployment
    {
		[Fact]
        public async Task ShouldBeAbleToDeployAContract()
        {
          var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
          var password = "password";
          //  var abi = @"[{'constant':false,'inputs':[{'name':'val','type':'int256'}],'name':'multiply','outputs':[{'name':'d','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[{'name':'multiplier','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";
            var abi = @"[{'constant':false,'inputs':[{'name':'val','type':'int256'}],'name':'multiply','outputs':[{'name':'d','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[{'name':'multiplier','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";
            var byteCode =
                "608060405234801561001057600080fd5b506040516100db3803806100db8339818101604052602081101561003357600080fd5b50516000556095806100466000396000f3fe6080604052348015600f57600080fd5b506004361060285760003560e01c80631df4f14414602d575b600080fd5b604760048036036020811015604157600080fd5b50356059565b60408051918252519081900360200190f35b600054029056fea265627a7a72305820f3a508f287af40445f0ae0dd5332808116a68e878c7669173e633a828d2a53ac64736f6c634300050a0032";
          var multiplier = 7;
          //a managed account uses personal_sendTransanction with the given password, this way we don't need to unlock the account for a certain period of time
          var account = new ManagedAccount(senderAddress, password);

          //using the specific geth web3 library to allow us manage the mining.
          var web3 = new Geth.Web3Geth(account);

          var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, senderAddress, new HexBigInteger(3000000), null, multiplier);

            //assumed we are mining already, no need to manage it using Nethereum
            // start mining
            // await web3.Miner.Start.SendRequestAsync(6);


            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

          while(receipt == null){
              Thread.Sleep(1000);
              receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
          }
          var contractAddress = receipt.ContractAddress;

          var contract = web3.Eth.GetContract(abi, contractAddress);

          var multiplyFunction = contract.GetFunction("multiply");

          var result = await multiplyFunction.CallAsync<int>(7);

          Assert.Equal(49, result);
        }

    }
}
