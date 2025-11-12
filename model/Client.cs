using BankSystem.API.model;
using System.Collections.Generic;
using System;

namespace BankSystem.API.model 
{
    public class Client
    {
        public int Id { get; set; }
        public string? Nome { get; set; } = null;
        public string? Documento { get; set; } = null;
        public DateTime DataCriacao { get; set; }

        public ICollection<Conta> Contas { get; set; } = new List<Conta>();
    }
}