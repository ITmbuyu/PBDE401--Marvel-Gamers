using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PBDE401.Models
{
    public class IndividualButtonPartial
    {
        public string ButtonType { get; set; }
        public string Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }

        public int? CategoryId { get; set; }
        public int? MedicineId { get; set; }
        public int? CustomerId { get; set; }
        public int? PatientId { get; set; }
        public int? MembershipTypeId { get; set; }
        public string UserId { get; set; }

        public int? SubscriptionId { get; set; }

        public string ActionParameter
        {
            get
            {
                var param = new StringBuilder(@"/");

                if (MedicineId != null && MedicineId > 0)
                {
                    param.Append(String.Format("{0}", MedicineId));
                }
                if (CategoryId != null && CategoryId > 0)
                {
                    param.Append(String.Format("{0}", CategoryId));
                }
                if (MembershipTypeId != null && MembershipTypeId > 0)
                {
                    param.Append(String.Format("{0}", MembershipTypeId));
                }
                if (PatientId != null && PatientId > 0)
                {
                    param.Append(String.Format("{0}", PatientId));
                }
                if (UserId != null && UserId.Trim().Length > 0)
                {
                    param.Append(String.Format("{0}", UserId));
                }
                if (SubscriptionId != null && SubscriptionId > 0)
                {
                    param.Append(String.Format("{0}", SubscriptionId));
                }
                if (CustomerId != null && CustomerId > 0)
                {
                    param.Append(String.Format("{0}", CustomerId));
                }

                return param.ToString();
            }
        }
    }
}