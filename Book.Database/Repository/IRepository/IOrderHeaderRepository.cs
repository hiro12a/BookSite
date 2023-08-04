using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Database.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatue(int id, string orderStatus, string? paymentStatus = null);

        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    }
}
