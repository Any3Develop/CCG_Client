using Startup.Startup;
using Zenject;

namespace Startup.Infrastructure
{
    public class StartupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<StartupApplication>()
                .AsSingle()
                .NonLazy();
        }
    }
}