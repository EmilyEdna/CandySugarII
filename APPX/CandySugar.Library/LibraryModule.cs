namespace CandySugar.Library
{
    public class LibraryModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            DbContext.InitTabel();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
