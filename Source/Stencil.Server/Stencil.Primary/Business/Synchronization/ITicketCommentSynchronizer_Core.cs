//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Synchronization
{
    public partial interface ITicketCommentSynchronizer : ISynchronizer
    {
        void SynchronizeItem(Guid primaryKey, Availability availability);
    }
}

