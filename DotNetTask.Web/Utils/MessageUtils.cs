using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DotNetTask.Web.Utils
{
    public static class MessageUtils
    {
        public static Dictionary<string, List<string>> ExtractModelErrors(ModelStateDictionary modelState)
        {
            return modelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                        .Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? "Invalid value" : e.ErrorMessage)
                        .ToList()
                );
        }
    }
}
