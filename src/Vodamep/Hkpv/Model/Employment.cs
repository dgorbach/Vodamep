using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.Hkpv.Model
{
    public partial class Employment 
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }
    }
}
