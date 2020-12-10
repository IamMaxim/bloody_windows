using LSInjector;
using Services.UI.PopupUIService;

public class BloodyWindowsConfig : LSInjectorConfig
{
    public override void Setup(LSInjector.LSInjector injector)
    {
        injector.AddService(new UIService.UIService());
        injector.AddService(new PopupUIService());
    }
}