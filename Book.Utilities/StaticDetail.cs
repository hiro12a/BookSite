using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Utilities
{
    public class StaticDetail
    {
        // Company
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company"; // Gives companies more days to make paymen
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        // Order Status
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCanceled = "Canceled";
        public const string StatusRefunded = "Refunded";

        // Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";

        // Session
        public const string SessionCart = "SessionShoppingCart";
    }
}
