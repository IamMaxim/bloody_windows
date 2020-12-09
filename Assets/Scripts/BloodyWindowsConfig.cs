using LSInjector;

public class BloodyWindowsConfig : LSInjectorConfig
{
    public override void Setup(LSInjector.LSInjector injector)
    {
        injector.AddService(new UIService.UIService());

        
    }
}