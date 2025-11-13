using BankSystem.API.dto;
using BankSystem.API.model;
using BankSystem.APII.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.APII.Service
{
    public class ContaService : IContaService
    {
       
        private readonly IContaRepository _repositorio;

        public ContaService(IContaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        
        public async Task<IEnumerable<ContasViewModel>> GetAllAsync(int page, int size, string orderBy, string? titular)
        {
          
            var contas = await _repositorio.GetAllAsync(page, size, orderBy, titular);

            
            return contas.Select(c => MapToViewModel(c));
        }

      
        public async Task<ContasViewModel> GetByNumeroAsync(int numero)
        {
            var conta = await _repositorio.GetByNumeroAsync(numero);

           
            if (conta == null)
            {
                
                throw new KeyNotFoundException("Número da conta não encontrado.");
            }

            
            return MapToViewModel(conta);
        }

       
        public async Task<ContasViewModel> CriarContaAsync(ContaInputModel novaConta)
        {
            if (novaConta.Saldo < 0)
                throw new InvalidOperationException("Saldo inicial não pode ser negativo.");

            var jaExiste = await _repositorio.GetByNumeroAsync(novaConta.Numero);
            if (jaExiste != null)
                throw new InvalidOperationException("Este número de conta já está em uso.");

            
            var conta = MapToEntity(novaConta);

           
            conta.DataCriacao = DateTime.UtcNow;
            conta.Status = Status.Ativa;
            conta.Tipo = Tipo.Corrente; 

            await _repositorio.AddAsync(conta);
            await _repositorio.SaveChangesAsync();

            
            return MapToViewModel(conta);
        }


        public async Task DepositarAsync(int numero, decimal valor)
        {
            if (valor <= 0)
                throw new InvalidOperationException("Valor do depósito deve ser maior que zero.");

            var conta = await _repositorio.GetByNumeroAsync(numero);
            if (conta == null)
                throw new KeyNotFoundException($"Conta {numero} não encontrada.");

            conta.Saldo += valor;
            await _repositorio.SaveChangesAsync();
        }

       
        public async Task SacarAsync(int numero, decimal valor)
        {
            if (valor <= 0)
                throw new InvalidOperationException("Valor do saque deve ser maior que zero.");

            var conta = await _repositorio.GetByNumeroAsync(numero);
            if (conta == null)
                throw new KeyNotFoundException($"Conta {numero} não encontrada.");

            if (conta.Saldo < valor)
                throw new InvalidOperationException("Saldo insuficiente.");

            conta.Saldo -= valor;
            await _repositorio.SaveChangesAsync();
        }

       
        public async Task DeleteAccountAsync(int numero)
        {
            var conta = await _repositorio.GetByNumeroAsync(numero);
            if (conta == null)
                throw new KeyNotFoundException("Número da conta não encontrado.");

            _repositorio.Delete(conta);
            await _repositorio.SaveChangesAsync();
        }


        

        private ContasViewModel MapToViewModel(Conta conta)
        {
            return new ContasViewModel
            {
                Id = conta.Id,
                NumeroConta = conta.NumeroConta,
                Saldo = conta.Saldo,
                Titular = conta.Titular
            };
        }

        private Conta MapToEntity(ContaInputModel input)
        {
            return new Conta
            {
                NumeroConta = input.Numero,
                Saldo = input.Saldo,
                Titular = input.Titular
            };
        }

    }
}
