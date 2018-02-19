using System.ComponentModel;

namespace Wallet.Models.BindingModels
{
    public class AcountBalanceBm
    {
        public string Address { get; set; }

        [DisplayName("Blockcahin Node")]
        public string BlockChainNode { get; set; }
    }
}
