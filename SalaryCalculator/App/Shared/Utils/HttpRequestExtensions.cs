using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using SalaryCalculator.App.Shared.Exceptions;

namespace SalaryCalculator.App.Shared.Utils
{
    public static class HttpRequestExtensions
    {
        public static async Task<T> BindModelAsync<T>(this HttpRequestData request)
        {
            if (request.Body == null)
            {
                throw new BindingException("No se ha proporcionado un cuerpo a la petción");
            };
            try
            {
                var body = await new StreamReader(request.Body).ReadToEndAsync();
                var settings = new JsonSerializerSettings
                {
                    Converters = { new DateOnlyJsonConverter() },
                    Error = (sender, args) =>
                    {
                        args.ErrorContext.Handled = true;
                    }
                };
                var model = JsonConvert.DeserializeObject<T>(body, settings);
                var modelState = ModelValidator.Validate(model);
                if (!modelState.isValid)
                {
                    throw new BindingException("Campos Invalidos", modelState.errors );
                }
                return model;
            }
            catch (BindingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BindingException($"Error al mapear el modelo {typeof(T).Name}", ex);
            }
            
        }
    }
}
