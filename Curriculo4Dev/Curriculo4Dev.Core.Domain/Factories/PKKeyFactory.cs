using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Domain.Factories
{
    public class PKKeyFactory
    {
        public static string CreateKey(Type entityType)
        {
            if (entityType == typeof(Usuario))
            {
                return $"USER#{Guid.NewGuid()}";
            }

            if (entityType == typeof(Template))
            {
                return "TEMPLATES";
            }

            if (entityType == typeof(Plano))
            {
                return "PLANOS";
            }

            return Guid.NewGuid().ToString();
        }
    }
}
