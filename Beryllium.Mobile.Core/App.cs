using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Beryllium.Mobile.Core.ViewModels.Root;

namespace Beryllium.Mobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<RootViewModel>();
        }
    }
}
