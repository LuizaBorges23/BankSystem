using BankSystem.API.dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankSystem.APII.Service
{
    public interface IContaService
    {
        Task<IEnumerable<ContasViewModel>> GetAllAsync(int page, int size, string orderBy, string? titular);
        Task<ContasViewModel> GetByNumeroAsync(int numero);


        Task<ContasViewModel> CriarContaAsync(ContaInputModel novaConta);
        Task DepositarAsync(int numero, decimal valor);
        Task SacarAsync(int numero, decimal valor);
        Task DeleteAccountAsync(int numero);
    }
}
