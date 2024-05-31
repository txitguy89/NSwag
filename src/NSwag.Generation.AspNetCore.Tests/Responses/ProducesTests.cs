using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.NewtonsoftJson.Generation;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Responses;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Responses
{
    public class ProducesTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_produces_is_defined_on_all_operations_then_it_is_added_to_the_document()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaSettings = new NewtonsoftJsonSchemaGeneratorSettings { SchemaType = SchemaType.OpenApi3 } };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(TextProducesController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "TextProduces_ProducesOnOperation").Operation;

            Assert.Contains("text/html", document.Produces);
            Assert.Contains("text/html", operation.ActualProduces);
            Assert.Null(operation.Produces);
        }
        
        [Fact]
        public async Task When_operation_produces_is_different_in_several_controllers_then_they_are_added_to_the_operation()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaSettings = new NewtonsoftJsonSchemaGeneratorSettings { SchemaType = SchemaType.OpenApi3 } };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(TextProducesController), typeof(JsonProducesController));
            var json = document.ToJson();

            // Assert
            const string expectedTextContentType = "text/html";
            const string expectedJsonJsonType = "application/json";

            var textOperation = document.Operations.First(o => o.Operation.OperationId == "TextProduces_ProducesOnOperation").Operation;
            var jsonOperation = document.Operations.First(o => o.Operation.OperationId == "JsonProduces_ProducesOnOperation").Operation;

            Assert.DoesNotContain(expectedTextContentType, document.Produces);
            Assert.DoesNotContain(expectedJsonJsonType, document.Produces);

            Assert.Contains(expectedTextContentType, textOperation.Produces);
            Assert.Contains(expectedTextContentType, textOperation.ActualProduces);

            Assert.Contains(expectedJsonJsonType, jsonOperation.Produces);
            Assert.Contains(expectedJsonJsonType, jsonOperation.ActualProduces);
        }

        [Fact]
        public async Task When_operation_produces_multiple_types_that_vary_by_controller_then_they_are_added_to_the_operations()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaSettings = new NewtonsoftJsonSchemaGeneratorSettings { SchemaType = SchemaType.OpenApi3 } };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(AcceptHeaderProducesController), typeof(TextProducesController));
            var json = document.ToJson();

            // Assert
            const string expectedTextContentType = "text/html";
            const string expectedStandardJsonType = "application/json";
            const string expectedExtendedJsonType = "application/type1+json";

            var textOperation = document.Operations.First(o => o.Operation.OperationId == "TextProduces_ProducesOnOperation").Operation;
            var acceptHeaderOperation = document.Operations.First(o => o.Operation.OperationId == "AcceptHeaderProduces_ProducesOnOperation").Operation;

            Assert.DoesNotContain(expectedTextContentType, document.Produces);
            Assert.DoesNotContain(expectedStandardJsonType, document.Produces);
            Assert.DoesNotContain(expectedExtendedJsonType, document.Produces);

            Assert.Contains(expectedTextContentType, textOperation.Produces);
            Assert.Contains(expectedTextContentType, textOperation.ActualProduces);

            Assert.Contains(expectedStandardJsonType, acceptHeaderOperation.Produces);
            Assert.Contains(expectedStandardJsonType, acceptHeaderOperation.ActualProduces);

            Assert.Contains(expectedExtendedJsonType, acceptHeaderOperation.Produces);
            Assert.Contains(expectedExtendedJsonType, acceptHeaderOperation.ActualProduces);
        }

        [Fact]
        public async Task When_operation_produces_multiple_types_defined_on_all_operations_then_they_are_added_to_the_document()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaSettings = new NewtonsoftJsonSchemaGeneratorSettings { SchemaType = SchemaType.OpenApi3 } };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(AcceptHeaderProducesController));
            var json = document.ToJson();

            // Assert
            const string expectedStandardJsonType = "application/json";
            const string expectedExtendedJsonType = "application/type1+json";

            var operation = document.Operations.First(o => o.Operation.OperationId == "AcceptHeaderProduces_ProducesOnOperation").Operation;

            Assert.Contains(expectedStandardJsonType, document.Produces);
            Assert.Contains(expectedExtendedJsonType, document.Produces);
            Assert.Null(operation.Produces);
            Assert.Contains(expectedStandardJsonType, operation.ActualProduces);
            Assert.Contains(expectedExtendedJsonType, operation.ActualProduces);
        }
    }
}
