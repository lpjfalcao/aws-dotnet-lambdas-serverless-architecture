namespace Curriculo4Dev.Core.Domain.Exceptions
{
    public class Curriculo4DevException : Exception
    {
        public Curriculo4DevException()
        {
            
        }

        public Curriculo4DevException(string message) : base(message)
        {
            
        }

        public Curriculo4DevException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
