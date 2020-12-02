using LS.LSInjector;

public class RemoteAssistantConfig : LSInjectorConfig
{
    public override void Setup(LSInjector injector)
    {
        injector.AddService(new UIService.UIService());

        
    }
}