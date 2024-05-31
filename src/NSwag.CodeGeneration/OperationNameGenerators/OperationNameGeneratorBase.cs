using System.Linq;

namespace NSwag.CodeGeneration.OperationNameGenerators
{
    /// <summary>Generates the client name and operation name based on the Open Api Document information.</summary>
    public abstract class OperationNameGeneratorBase : IOperationNameGenerator
    {
        /// <summary>Gets a value indicating whether the generator supports multiple client classes.</summary>
        public abstract bool SupportsMultipleClients { get; }

        /// <summary>Gets the client name for a given operation (may be empty).</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="path">The HTTP path.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="operation">The operation.</param>
        /// <returns>The client name.</returns>
        public abstract string GetClientName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation);

        /// <summary>Gets the operation name for a given operation.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="path">The HTTP path.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="mediaType">The media type produced by the operation.</param>
        /// <param name="operation">The operation.</param>
        /// <returns>The operation name.</returns>
        public abstract string GetOperationName(OpenApiDocument document, string path, string httpMethod, string mediaType, OpenApiOperation operation);

        /// <summary>Converts the path to an operation name.</summary>
        /// <param name="path">The HTTP path.</param>
        /// <returns>The operation name.</returns>
        public virtual string ConvertPathToName(string path)
        {
            return path
                .Split('/')
                .Where(p => !p.Contains("{") && !string.IsNullOrWhiteSpace(p))
                .Reverse()
                .FirstOrDefault() ?? "Index";
        }
    }
}
