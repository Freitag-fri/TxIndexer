using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TxIndexer.Api.Contracts;
using TxIndexer.Api.Services;

namespace TxIndexer.Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TransactionResponse>>> GetPage([FromQuery] TransactionQuery query, CancellationToken ct = default)
        {
            return await _transactionService.GetPageAsync(query, ct);
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionByHash([RegularExpression("^0x[0-9a-fA-F]{64}$", ErrorMessage = "Hash must be 0x followed by 64 hex characters.")] string hash, CancellationToken ct = default)
        {
            var transaction = await _transactionService.GetTransactionByHashAsync(hash, ct);
            if(transaction is null)
            {
                return NotFound();
            }

            return transaction;
        }
    }
}
