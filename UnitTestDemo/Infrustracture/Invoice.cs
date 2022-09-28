using System;
namespace UnitTestDemo.Infrustracture
{
    public class Invoice
    {
        public Invoice()
        {
        }

        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public bool TaxInclusive { get; set; }
        public decimal NetAmount { get; set; }

        public bool IsCanceled { get; set; }

        public void Compute()
        {
            NetAmount = TaxInclusive ? Amount : Amount * (1 + TaxRate);
        }

        public void Cancel()
        {
            IsCanceled = true;
        }
    }
}

