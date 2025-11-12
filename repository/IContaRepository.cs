using BankSystem.API.model; 
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BankSystem.APII.repository
{
    
    public interface IContaRepository
    {
        Task<IEnumerable<Conta>> GetAllAsync(int page, int size, string orderBy, string? titular);
        Task<Conta?> GetByNumeroAsync(int numero);

        Task<IEnumerable<Conta>> GetByClienteIdAsync(int clienteId);

        Task AddAsync(Conta conta);
        void Update(Conta conta);
        void Delete(Conta conta);

        Task<bool> SaveChangesAsync();
    }
}
