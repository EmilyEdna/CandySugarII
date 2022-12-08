namespace CandySugar.Logic
{
    public class LogicModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IService, Service>();
        }
    }
}
