using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Utility
{
    public class SD
    {
        public const string Admin = "Admin";
        public const string Caissier = "Caissier";
        public const string Agent_Livraison = "Agent_Livraison";

        public const string ssShoppingCartCount = "ssCartCount";

        public const string StatusSubmitted = "Etablie";
        public const string StatusInProcess = "Encours de preparation";
        public const string StatusReady = "Pret";
        public const string StatusCompleted = "Complet";
        public const string StatusCancelled = "Cancelled";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }

}
