namespace Beryllium.Mobile.Core.Behaviors
{
   using MvvmCross;

   [Preserve(AllMembers = true)]
   public interface IAction
   {
      bool Execute(object sender, object parameter);
   }
}
