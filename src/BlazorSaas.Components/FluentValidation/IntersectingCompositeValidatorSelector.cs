using FluentValidation;
using FluentValidation.Internal;

namespace BlazorSaas.Components.FluentValidation;

internal class IntersectingCompositeValidatorSelector(IEnumerable<IValidatorSelector> selectors)
    : IValidatorSelector
{
    public bool CanExecute(IValidationRule rule, string propertyPath, IValidationContext context)
    {
        return selectors.All(s => s.CanExecute(rule, propertyPath, context));
    }
}