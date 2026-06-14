using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Domain.Factories
{
    public class SortKeyFactory
    {
        public static string CreateKey(Type entityType)
        {
            if (entityType == typeof(Usuario))
            {
                return "PROFILE";
            }

            if (entityType == typeof(Template))
            {
                return $"TEMPLATE#{Guid.NewGuid()}";
            }

            if (entityType == typeof(Documento))
            {
                return $"DOC#{Guid.NewGuid()}";
            }

            if (entityType == typeof(Plano))
            {
                return $"PLANO#{Guid.NewGuid()}";
            }

            return "METADATA";
        }
    }
}
