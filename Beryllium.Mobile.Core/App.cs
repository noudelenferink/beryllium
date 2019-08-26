namespace Beryllium.Mobile.Core
{
   using MvvmCross.IoC;
   using MvvmCross.ViewModels;
   using Beryllium.Mobile.Core.ViewModels.Root;

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
