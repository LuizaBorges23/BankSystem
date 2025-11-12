using BankSystem.API.model; // Namespace do Client (corrigido)
using System;

namespace BankSystem.API.model
{
    
    

    public class Conta
    {
        

        public int Id { get; set; } 

        public int NumeroConta { get; set; }
        public decimal Saldo { get; set; }

        
        public Tipo Tipo { get; set; }
        public Status Status { get; set; }

        public DateTime DataCriacao { get; set; }
        public string? Titular { get; set; } = null;

       
        public int ClientId { get; set; } 
        public Client Client { get; set; } = null!;

      
    }
}