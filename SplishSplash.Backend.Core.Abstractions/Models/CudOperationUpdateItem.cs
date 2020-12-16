using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Core.Abstractions.Models
{
    public class CudOperationUpdateItem<TOuter, TInner>
    {
        public CudOperationUpdateItem(TOuter outer, TInner inner)
        {
            Outer = outer;
            Inner = inner;
        }

        public TOuter Outer { get; set; }

        public TInner Inner { get; set; }
    }
}
