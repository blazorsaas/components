using System.Collections;
using System.Reflection;

namespace BlazorSaas.Components.FluentValidation;

internal static class PropertyPathHelper
{
    public static string ToFluentPropertyPath(EditContext editContext, FieldIdentifier fieldIdentifier)
    {
        var nodes = new Stack<Node>();
        nodes.Push(new Node
        {
            ModelObject = editContext.Model
        });

        while (nodes.Any())
        {
            var currentNode = nodes.Pop();
            var currentModelObject = currentNode.ModelObject;

            if (currentModelObject == fieldIdentifier.Model)
            {
                return BuildPropertyPath(currentNode, fieldIdentifier);
            }

            var nonPrimitiveProperties = currentModelObject?.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => !prop.PropertyType.IsPrimitive || prop.PropertyType.IsArray) ?? new List<PropertyInfo>();

            foreach (var nonPrimitiveProperty in nonPrimitiveProperties)
            {
                var instance = nonPrimitiveProperty.GetValue(currentModelObject);

                if (instance == fieldIdentifier.Model)
                {
                    var node = new Node
                    {
                        Parent = currentNode,
                        PropertyName = nonPrimitiveProperty.Name,
                        ModelObject = instance
                    };

                    return BuildPropertyPath(node, fieldIdentifier);
                }

                if (instance is IEnumerable enumerable)
                {
                    var itemIndex = 0;
                    foreach (var item in enumerable)
                    {
                        nodes.Push(new Node
                        {
                            ModelObject = item,
                            Parent = currentNode,
                            PropertyName = nonPrimitiveProperty.Name,
                            Index = itemIndex++
                        });
                    }
                }
                else if (instance is not null)
                {
                    nodes.Push(new Node
                    {
                        ModelObject = instance,
                        Parent = currentNode,
                        PropertyName = nonPrimitiveProperty.Name
                    });
                }
            }
        }

        return string.Empty;
    }

    private static string BuildPropertyPath(Node currentNode, FieldIdentifier fieldIdentifier)
    {
        var pathParts = new List<string> { fieldIdentifier.FieldName };
        var next = currentNode;

        while (next is not null)
        {
            if (!string.IsNullOrEmpty(next.PropertyName))
            {
                pathParts.Add(next.Index is not null
                    ? $"{next.PropertyName}[{next.Index}]"
                    : next.PropertyName);
            }

            next = next.Parent;
        }

        pathParts.Reverse();

        return string.Join('.', pathParts);
    }

    private class Node
    {
        public Node? Parent { get; set; }
        public object? ModelObject { get; set; }
        public string? PropertyName { get; set; }
        public int? Index { get; set; }
    }
}