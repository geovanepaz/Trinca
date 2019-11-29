using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Errors
{
    public class ValidationResultModel
    {
        public object Error { get; }
        public string Message { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            var internalErrors = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage))).ToList();

            Message = "Dados inválidos";

            Error = internalErrors.GroupBy(o => o.Field, (chave, valor) => new
            {
                Field = chave,
                Message = internalErrors.Where(oo => oo.Field == chave).Select(ooo => ooo.Message).ToList()
            });
        }
    }
}