using Libs.Base;
using System;
using System.Text;

namespace Libs.Exceptions
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Formata uma mensagem de erro detalhada para a exceção fornecida e retorna um DataResult com essa mensagem.
        /// </summary>
        /// <param name="ex">A exceção a ser formatada.</param>
        /// <returns>Um DataResult contendo a mensagem de erro detalhada.</returns>
        public static DataResult CreateErrorResult(Exception ex)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine($"{ex.Message}");
            if (ex.InnerException != null)
            {
                errorMessage.AppendLine();
                errorMessage.AppendLine("Inner Exception:");
                errorMessage.AppendLine($"Message: {ex.InnerException.Message}");
            }

            return new DataResult
            {
                Data = null,
                Success = false,
                Message = errorMessage.ToString()
            };
        }

        /// <summary>
        /// Formata uma mensagem de erro detalhada para a exceção fornecida e retorna um DataResult com essa mensagem.
        /// </summary>
        /// <param name="ex">A exceção a ser formatada.</param>
        /// <returns>Um DataResult contendo a mensagem de erro detalhada.</returns>
        public static DataPagedResult CreatePagedErrorResult(Exception ex)
        {
            var errorMessage = new StringBuilder("An unexpected error occurred.");

            errorMessage.AppendLine($"Message: {ex.Message}");

            if (ex.InnerException != null)
            {
                errorMessage.AppendLine();
                errorMessage.AppendLine("Inner Exception:");
                errorMessage.AppendLine($"Message: {ex.InnerException.Message}");
            }

            return new DataPagedResult
            {
                Data = null,
                Success = false,
                Message = errorMessage.ToString(),
                PageIndex = 1,
                PageSize = 1
            };
        }
    }
}