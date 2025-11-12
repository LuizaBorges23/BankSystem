namespace BankSystem.API.dto
{
    public class Contas
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Titular { get; set; } = string.Empty;
        public decimal Saldo { get; set; }


    }
}