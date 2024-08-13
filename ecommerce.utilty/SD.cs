using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ecommerce.utility
{
    public class SD
    {
        public const string Role_User_Indi = "Individual";
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";


        public const string StatusPending = "pending";
		public const string StatusApproved = "approved";
		public const string StatusInProcess = "Processing";
		public const string StatusShipped = "shipped";
		public const string StatusCancelled = "cancelled";
		public const string StatusRefunded = "refunded";

        public const string PaymentStatusPending = "pending";
		public const string PaymentStatusApproved = "approved";
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejected = "rejected";

	}
}
