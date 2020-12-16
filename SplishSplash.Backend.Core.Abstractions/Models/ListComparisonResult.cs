using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Core.Abstractions.Models
{
    public class ListComparisonResult<TOuter, TInner> : IListComparisonResult<TOuter, TInner>
    {
        public ListComparisonResult()
        {
            Added = new TInner[0];
            Updated = new CudOperationUpdateItem<TOuter, TInner>[0];
            Removed = new TOuter[0];
        }

        public TInner[] Added { get; set; }

        public CudOperationUpdateItem<TOuter, TInner>[] Updated { get; set; }

        public TOuter[] Removed { get; set; }
    }
}
