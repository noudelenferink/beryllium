using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Beryllium.Core.ViewModels.Root;

namespace Beryllium.Core
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
