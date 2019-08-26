namespace Beryllium.Mobile.Core.ViewModels
{
   using MvvmCross.ViewModels;

   public abstract class RankixBaseViewModel<TParameter, TResult> : RankixBaseViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
