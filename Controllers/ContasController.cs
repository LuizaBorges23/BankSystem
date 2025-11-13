using BankSystem.API.data;
using BankSystem.API.dto;
using BankSystem.API.model;
using BankSystem.APII.repository;
using BankSystem.APII.Service;
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
        private readonly IContaService _service;

        public ContasControllers(IContaService contaService)
        {
            _service = contaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContasViewModel>>> GetAll(
             [FromQuery] int page = 1, [FromQuery] int size = 10,
             [FromQuery] string orderBy = "numero", [FromQuery] string? titular = null)
        {
            
            var viewModels = await _service.GetAllAsync(page, size, orderBy, titular);
            return Ok(viewModels);
        }

        [HttpGet("{numero}")]
        public async Task<ActionResult<ContasViewModel>> Get(int numero)
        {
            try
            {
              
                var contaview = await _service.GetByNumeroAsync(numero);
                return Ok(contaview);
            }
            catch (KeyNotFoundException ex)
            {
               
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContasViewModel>> CriarConta([FromBody] ContaInputModel novaConta)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                
                var contaview = await _service.CriarContaAsync(novaConta);
                return CreatedAtAction(nameof(Get), new { numero = contaview.NumeroConta }, contaview);
            }
            catch (InvalidOperationException ex)
            {
                
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{numero}/depositar")]
        public async Task<ActionResult> Depositar(int numero, [FromQuery] decimal saldoDeposito)
        {
            try
            {
                await _service.DepositarAsync(numero, saldoDeposito);
                return Ok(new { message = "Depósito realizado com sucesso" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{numero}/sacar")]
        public async Task<ActionResult> Saque(int numero, [FromQuery] decimal saldoSaque)
        {
            try
            {
                await _service.SacarAsync(numero, saldoSaque);
                return Ok(new { message = "Saque realizado com sucesso" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{numero}")]
        public async Task<ActionResult> DeleteAccount(int numero)
        {
            try
            {
                await _service.DeleteAccountAsync(numero);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}