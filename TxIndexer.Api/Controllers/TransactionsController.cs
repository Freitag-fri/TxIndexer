using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<PagedResponse<TransactionResponse>>> GetPage(int page = 1, int pageSize = 30, CancellationToken ct = default)
        {
            return await _transactionService.GetPageAsync(page, pageSize, ct);
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionByHash(string hash, CancellationToken ct = default)
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
