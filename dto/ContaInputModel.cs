using System.ComponentModel.DataAnnotations;



namespace BankSystem.API.dto
{
   
    public class ContaInputModel
    {
        [Required, Range(1, int.MaxValue)]
        public int Numero { get; set; }

        [Required, StringLength(100)]
        public string Titular { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Saldo { get; set; }


    }
}
