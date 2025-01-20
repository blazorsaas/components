namespace BlazorSaas.Components;

public abstract class EditorBase<TModel> : ComponentBase
    where TModel : class
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected EditContext EditContext = null!;

    [Parameter] [EditorRequired] public TModel Model { get; set; } = null!;
    [Parameter] [EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public bool HideButtons { get; set; }

    protected override void OnParametersSet()
    {
        this.EditContext = new EditContext(Model);
        if (AddBootstrapValidationStyles())
        {
            this.EditContext.SetFieldCssClassProvider(new BootstrapValidationFieldClassProvider());
        }

        this.EditContext.ShouldUseFieldIdentifiers = true;
    }

    protected virtual bool AddBootstrapValidationStyles()
    {
        return true;
    }

    // ReSharper disable once UnusedMember.Global
    protected async Task Submit()
    {
        await OnSubmit.InvokeAsync();
    }

    // ReSharper disable once UnusedMember.Global
    protected async Task Cancel()
    {
        await OnCancel.InvokeAsync();
    }
}