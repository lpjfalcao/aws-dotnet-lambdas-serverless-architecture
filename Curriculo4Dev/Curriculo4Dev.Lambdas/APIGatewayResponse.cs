using Amazon.Lambda.APIGatewayEvents;

namespace Curriculo4Dev.Lambdas
{
    public class APIGatewayResponse
    {
        public static APIGatewayProxyResponse CreateResponse(string result, int statusCode)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = result,
                Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" },
                        {
                            "Access-Control-Allow-Headers",
                            "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token"
                        },
                        { "Access-Control-Allow-Methods", "GET,POST,OPTIONS" },
                        { "Access-Control-Allow-Credentials", "true" }
                    }
            };
        }
    }
}
