using Stencil.Data.Sql;
using Stencil.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public partial class ProductBusiness : IProductBusiness
    {
        public IEnumerable<Product> GetAffectedProductsByTicketID(Guid ticket_id)
        {
            return base.ExecuteFunction(nameof(GetAffectedProductsByTicketID), delegate ()
            {
                if (ticket_id == Guid.Empty)
                {
                    return Enumerable.Empty<Product>();
                }

                using (var db = this.CreateSQLContext())
                {
                    List<dbProduct> affectedProducts = db.dbAffectedProducts.Where(ap => ap.ticket_id == ticket_id)
                                                                            .Select(ap => ap.Product)
                                                                            .ToList();
                    return affectedProducts.Select(pp => pp.ToDomainModel()).ToList();
                }
            });
        }
    }
}
