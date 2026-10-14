using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace TxIndexer.Api.Infrastructure
{
    sealed class ActionTimingFilter : IAsyncActionFilter
    {
        private readonly ILogger<ActionTimingFilter> _logger;
        private const int SlowActionThresholdMs = 500;

        public ActionTimingFilter(ILogger<ActionTimingFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            await next();

            if(stopwatch.ElapsedMilliseconds > SlowActionThresholdMs)
            {
                _logger.LogWarning($"Slow action {context.ActionDescriptor.DisplayName} took {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
