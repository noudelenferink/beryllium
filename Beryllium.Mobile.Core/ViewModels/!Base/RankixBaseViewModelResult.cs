using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace Beryllium.Mobile.Core.ViewModels
{
    public abstract class RankixBaseViewModelResult<TResult> : RankixBaseViewModel, IMvxViewModelResult<TResult>
    {
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource?.Task.IsCompleted == false && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
    }
}
