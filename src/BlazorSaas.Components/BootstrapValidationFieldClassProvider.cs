namespace BlazorSaas.Components;

public class BootstrapValidationFieldClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        return editContext.GetValidationMessages(fieldIdentifier).Any()
            ? "is-invalid"
            : editContext.IsModified(fieldIdentifier)
                ? "is-valid"
                : "needs-validation";
    }
}