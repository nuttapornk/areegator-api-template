using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Helpers.NSwag;

public class AddRequiredHeaderParameter : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        context.OperationDescription.Operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = "sender",
                Kind = OpenApiParameterKind.Header,
                Schema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.String },
                IsRequired = true,
                Default = "areegator"
            }
        );
        context.OperationDescription.Operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = "forward",
                Kind = OpenApiParameterKind.Header,
                Schema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.String },
                IsRequired = true,
                Default = "127.0.0.1"
            });

        context.OperationDescription.Operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = "refer",
                Kind = OpenApiParameterKind.Header,
                Schema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.String },
                IsRequired = true,
                Default = "no user"
            });
        return true;
    }
}
