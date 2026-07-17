using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace GestaoPedidos.Api.OpenApi;

/// <summary>
/// Faz os enums aparecerem como string (ex.: "Pago") no documento OpenAPI,
/// em vez do valor numérico subjacente. Alinha o schema ao JsonStringEnumConverter.
/// </summary>
public class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        if (type.IsEnum)
        {
            schema.Type = JsonSchemaType.String;
            schema.Format = null;
            schema.Enum ??= [];
            schema.Enum.Clear();

            foreach (var name in Enum.GetNames(type))
                schema.Enum.Add(JsonValue.Create(name));
        }

        return Task.CompletedTask;
    }
}