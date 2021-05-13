using Stencil.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.UnitTests
{
    public class TestStencilContext : StencilContext
    {
        public TestStencilContext(DbConnection connection)
            : base(connection)
        {
        }

        protected override void Dispose(bool disposing)
        {
            // DO NOTHING
        }

        public void RealDispose()
            => base.Dispose(true);
    }
}
