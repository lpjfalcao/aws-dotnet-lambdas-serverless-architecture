using Curriculo4Dev.Core.Domain.Enums;
using FluentValidation.Results;

namespace Curriculo4Dev.Core.Domain.Common
{
    public class MessageHelper<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public IList<string> Validations { get; set; } = [];

        public void Ok(T data)
        {
            Data = data;
            Success = true;
            StatusCode = (int)StatusCodeEnum.Ok;
            Message = "Operação realizada com sucesso!";
        }

        public void NotFound(string message)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.NotFound;
            Message = message;
        }

        public void BadRequest(IList<string> validations)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.BadRequest;
            Validations = validations;
        }

        public void Error(Exception ex)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.InternalServerError;
            Message = ex.Message;
            StackTrace = ex.StackTrace ?? "Não foi possível encontrar a StackTrace";
        }

        public void Created(T data)
        {
            Data = data;
            Success = true;
            StatusCode = (int)StatusCodeEnum.Created;
            Message = "Recurso cadastrado com sucesso";
        }

        public void BadRequest(Exception ex)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.BadRequest;
            Message = ex.Message;
        }
    }
    public class MessageHelper
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public IList<string> Validations { get; set; } = [];

        public void Ok()
        {
            Success = true;
            StatusCode = (int)StatusCodeEnum.Ok;
            Message = "Operação realizada com sucesso!";
        }

        public void NotFound(string message)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.NotFound;
            Message = message;
        }

        public void BadRequest(IList<string> validations)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.BadRequest;
            Validations = validations;
        }

        public void BadRequest(List<ValidationFailure> validations)
        {
            foreach (var validation in validations)
            {
                Validations.Add(validation.ErrorMessage);
            }

            Success = false;
            StatusCode = (int)StatusCodeEnum.BadRequest;
            Message = "Ocorreu um ou mais erros de validação. Veja os detalhes na propriedade 'Validations'";
        }

        public void BadRequest(Exception ex)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.BadRequest;
            Message = ex.Message;
        }

        public void BadRequest(string  mensagem)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.BadRequest;
            Message = mensagem;
        }


        public void Error(Exception ex)
        {
            Success = false;
            StatusCode = (int)StatusCodeEnum.InternalServerError;
            Message = ex.Message;
            StackTrace = ex.StackTrace ?? "Não foi possível encontrar a StackTrace";
        }
    }
}
