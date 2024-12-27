using System.ComponentModel.DataAnnotations;
using SalaryCalculator.App.Shared.Exceptions;

namespace SalaryCalculator.App.Shared.Utils
{
    public static class ModelValidator
    {
        public static ModelValidatorState Validate<T>(T model)
        {
            try
            {
                var context = new ValidationContext(model!);
                var results = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(model!, context, results, true);

                var errors = results.Select(r => r.ErrorMessage).ToList();
                return new ModelValidatorState(isValid, errors!);
            }
            catch (Exception e)
            {
                throw new BindingException($"Error al validar el modelo {typeof(T).Name}", e );
            }
        }

        public class ModelValidatorState
        {
            public readonly bool isValid = false;
            public readonly List<string> errors;

            public ModelValidatorState(bool isValid, List<string> errors)
            {
                this.isValid = isValid;
                this.errors = errors;
            }
        }
    }
}
