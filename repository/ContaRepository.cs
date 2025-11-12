using BankSystem.API.data; 
using BankSystem.API.model; 
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankSystem.APII.repository
{
    public class ContaRepository : IContaRepository
    {
        private readonly BankContext _context;

        public ContaRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Conta>> GetAllAsync(int page, int size, string orderBy, string? titular)
        {
            
            IQueryable<Conta> query = _context.Contas.AsNoTracking(); 

          
            if (!string.IsNullOrWhiteSpace(titular))
            {
                query = query.Where(c => c.Titular.Contains(titular));
            }

           
            switch (orderBy.ToLower())
            {
                case "titular_desc": query = query.OrderByDescending(c => c.Titular); break;
                case "titular": query = query.OrderBy(c => c.Titular); break;
                case "saldo_desc": query = query.OrderByDescending(c => c.Saldo); break;
                case "saldo": query = query.OrderBy(c => c.Saldo); break;
                case "numero_desc": query = query.OrderByDescending(c => c.NumeroConta); break;
                default: query = query.OrderBy(c => c.NumeroConta); break;
            }

        
            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<Conta?> GetByNumeroAsync(int numero)
        {
            
            return await _context.Contas
                .FirstOrDefaultAsync(c => c.NumeroConta == numero);
        }

        public async Task<IEnumerable<Conta>> GetByClienteIdAsync(int clienteId)
        {
      
            return await _context.Contas
                .AsNoTracking()
                .Where(c => c.ClientId == clienteId)
                .ToListAsync();
        }

        public async Task AddAsync(Conta conta)
        {
           
            await _context.Contas.AddAsync(conta);
        }

        public void Update(Conta conta)
        {
            
            _context.Entry(conta).State = EntityState.Modified;
        }

        public void Delete(Conta conta)
        {
            
            _context.Contas.Remove(conta);
        }

        public async Task<bool> SaveChangesAsync()
        {
            
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
