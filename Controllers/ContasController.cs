using BankSystem.API.data;
using BankSystem.API.dto;
using BankSystem.API.model;
using BankSystem.APII.repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace BankSystem.API
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ContasControllers : ControllerBase
    {
        private readonly IContaRepository _repositorio;
        public ContasControllers(IContaRepository contaRepository)
        {
            
            _repositorio = contaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContasViewModel>>> GetAll(
             [FromQuery] int page = 1, [FromQuery] int size = 10,
             [FromQuery] string orderBy = "numero", [FromQuery] string? titular = null)
        {
         
            var contas = await _repositorio.GetAllAsync(page, size, orderBy, titular);

           
            var viewModels = contas.Select(static c => new ContasViewModel
            {
                Id = c.Id,
                NumeroConta = c.NumeroConta,
                Saldo = c.Saldo,
                Titular = c.Titular
            });

            return Ok(viewModels);
        }

        [HttpGet("{numero}")]
        public async Task<ActionResult<ContasViewModel>> Get(int numero)
        {
            
            var conta = await _repositorio.GetByNumeroAsync(numero);

            if (conta == null)
            {
                return NotFound(new { message = "Número da conta não encontrado." });
            }

            var contaview = new ContasViewModel
            {
                Id = conta.Id,
                NumeroConta = conta.NumeroConta,
                Saldo = conta.Saldo,
                Titular = conta.Titular 
            };

            return Ok(contaview);
        }

        [HttpPost]
        public async Task<ActionResult<ContasViewModel>> CriarConta( [FromBody] ContaInputModel novaConta)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            
            if (novaConta.Saldo < 0)
                return BadRequest(new { message = "Saldo inicial não pode ser negativo." });
            if (novaConta.Numero <= 0)
                return BadRequest(new { message = "Número da conta deve ser maior que zero." });

            var jaExiste = await _repositorio.GetByNumeroAsync(novaConta.Numero);
            if (jaExiste != null)
            {
                return BadRequest(new { message = "Este número de conta já está em uso." });
            }
            
            var conta = new Conta
            {
                NumeroConta = novaConta.Numero,
                Saldo = novaConta.Saldo,
                Titular = novaConta.Titular, 
                DataCriacao = DateTime.UtcNow,
                Status = Status.Ativa,
                Tipo = Tipo.Corrente
            };

           
            await _repositorio.AddAsync(conta);
            await _repositorio.SaveChangesAsync();

            
            var contaview = new ContasViewModel
            {
                Id = conta.Id,
                NumeroConta = conta.NumeroConta,
                Saldo = conta.Saldo,
                Titular = conta.Titular,
            };

            return CreatedAtAction(nameof(Get), new { numero = contaview.NumeroConta }, contaview);
        }

        [HttpPatch("{numero}/depositar")]
        public async Task<ActionResult> Depositar(int numero, [FromQuery] decimal saldoDeposito)
        {
            if (saldoDeposito <= 0)
                return BadRequest(new { message = "Valor do depósito deve ser maior que zero." });

            var conta = await _repositorio.GetByNumeroAsync(numero);
            if (conta == null)
                return NotFound(new { message = $"Conta {numero} não encontrada." });

            conta.Saldo += saldoDeposito;

           
            await _repositorio.SaveChangesAsync();

            return Ok(new { message = "Depósito realizado com sucesso" });
        }

        [HttpPatch("{numero}/sacar")]
        public async Task<ActionResult> Saque(int numero, [FromQuery] decimal saldoSaque)
        {
            if (saldoSaque <= 0)
                return BadRequest(new { message = "Valor do saque deve ser maior que zero." });

            var conta = await _repositorio.GetByNumeroAsync(numero);
            if (conta == null)
                return NotFound(new { message = $"Conta {numero} não encontrada." });

            if (conta.Saldo < saldoSaque)
                return BadRequest(new { message = "Saldo insuficiente." });

            conta.Saldo -= saldoSaque;

            await _repositorio.SaveChangesAsync();

            return Ok(new { message = "Saque realizado com sucesso" });
        }

        [HttpDelete("{numero}")]
        public async Task<ActionResult> DeleteAccount(int numero)
        {
            var conta = await _repositorio.GetByNumeroAsync(numero);
            if (conta == null)
                return NotFound(new { message = "Número da conta não encontrado" });

            _repositorio.Delete(conta);
            await _repositorio.SaveChangesAsync();

            return NoContent(); 
        }
    }
}