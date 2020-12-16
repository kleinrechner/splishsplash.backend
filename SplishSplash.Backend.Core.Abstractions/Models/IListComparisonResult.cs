using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Core.Abstractions.Models
{
    public interface IListComparisonResult<TOuter, TInner>
    {
        TInner[] Added { get; set; }

        TOuter[] Removed { get; set; }

        CudOperationUpdateItem<TOuter, TInner>[] Updated { get; set; }
    }
}
