namespace BankSystem.API.dto
{
    public class ContasViewModel
    {
        public int Id { get; set; }
        public int NumeroConta { get; set; }
        public string Titular { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public ContasViewModel(int id, int numero, string titular, decimal saldo)
        {
            Id = id;
            NumeroConta = numero;
            Titular = titular;
            Saldo = saldo;
        }

        public ContasViewModel() { }

    }
}