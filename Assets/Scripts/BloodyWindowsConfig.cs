using LS.LSInjector;

public class BloodyWindowsConfig : LSInjectorConfig
{
    public override void Setup(LSInjector injector)
    {
        injector.AddService(new UIService.UIService());

        
    }
}