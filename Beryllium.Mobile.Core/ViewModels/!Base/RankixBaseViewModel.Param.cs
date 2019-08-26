using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;

namespace Beryllium.Mobile.Core.ViewModels
{
    public abstract class RankixBaseViewModel<TParameter> : RankixBaseViewModel, IMvxViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
