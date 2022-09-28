using System;
using Xunit.Abstractions;

namespace UnitTestDemo.Infrustracture
{
    public class InvoiceFixture : IDisposable
    {
        public Invoice Invoice { get; private  set; }
        public InvoiceFixture()
        {
            Invoice = new Invoice();
            Invoice.Load();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}

