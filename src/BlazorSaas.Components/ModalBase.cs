using BlazorBootstrap;

namespace BlazorSaas.Components;

public abstract class ModalBase : ComponentBase
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    // ReSharper disable once MemberCanBePrivate.Global
    protected Modal Modal = null!;

    public virtual async Task ShowAsync()
    {
        await this.Modal.ShowAsync();
    }

    public async Task HideAsync()
    {
        await this.Modal.HideAsync();
    }
}