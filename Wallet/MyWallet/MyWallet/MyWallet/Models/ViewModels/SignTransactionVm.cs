namespace Wallet.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TransactionModel 
    {
        [Required(ErrorMessage = "Date Required")]
        [DataType(DataType.Text, ErrorMessage = "Invalid Date Format")]
        [StringLength(40, MinimumLength = 40)]
        public string From { get; set; }

        [Required(ErrorMessage = "Date Required")]
        [DataType(DataType.Text, ErrorMessage = "Invalid Date Format")]
        [StringLength(40, MinimumLength = 40)]
        public string To { get; set; }

        [Required(ErrorMessage = "Date Required")]
        [Range(1, int.MaxValue)]
        [DataType(DataType.Currency)]
        public int Value { get; set; }

        public int Fee { get; set; }

        public DateTime DateCreated { get; set; }

        public string SenderPubKey { get; set; }

        public string Info { get; set; }

        public string Response { get; set; }
    }
}
