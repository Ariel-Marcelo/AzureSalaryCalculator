using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SalaryCalculator.App.Application;
using SalaryCalculator.App.Domain;
using SalaryCalculator.App.Shared.Exceptions;
using SalaryCalculator.App.Shared.Utils;
using SalaryCalculator.EntryPoints.Dto;

namespace SalaryCalculator.EntryPoints
{
    public class SalaryCalculatorFunction(ILogger<SalaryCalculatorFunction> logger)
    {
        private readonly ILogger<SalaryCalculatorFunction> _logger = logger;

        [Function("SalaryCalculator")]
        public async Task<ActionResult> RunSalaryCalculatorFunction([HttpTrigger(AuthorizationLevel.Function, "post", "get", Route = "salary")] HttpRequestData req)
        {
            try
            {
                var request = await req.BindModelAsync<SalaryCalculatorRequest>();
                var service = new SalaryCalculatorUseCase();
                return new OkObjectResult(
                    new SuccessResponseDto(
                        service.CalculateSalaryEstimation(request)
                    )
                );
            }
            catch (BindingException ex)
            {
                return new BadRequestObjectResult(new ErrorResponseDto(ex.Message, ex.Errors));
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ErrorResponseDto(ex.Message))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
