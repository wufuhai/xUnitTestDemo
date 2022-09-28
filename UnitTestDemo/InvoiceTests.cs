using FluentAssertions;
using UnitTestDemo.Infrustracture;
using Xunit.Abstractions;

namespace UnitTestDemo
{
    public class InvoiceTests : IDisposable
    {
        Invoice _inv = new Invoice();
        private readonly ITestOutputHelper _output;

        public InvoiceTests(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Creating InvoiceTests class instance");
        }

        [Fact]
        public void TestTaxCalculation()
        {
            // Arrange
            _inv.Amount = 100;
            _inv.TaxRate = 0.10m;
            _inv.TaxInclusive = false;

            // Act
            _inv.Compute();

            // Assert
            //Assert.Equal(110, inv.NetAmount);
            _inv.NetAmount.Should().Be(110m);
        }

        [Fact]
        public void TestCancel()
        {
            // Arrange
            _inv.Amount = 100;
            _inv.TaxRate = 0.10m;
            _inv.TaxInclusive = false;

            // Act
            _inv.Compute();

            // Assert
            //Assert.Equal(110, inv.NetAmount);
            _inv.NetAmount.Should().Be(110m);
        }

        public void Dispose()
        {
            //_output.WriteLine("Disposing InvoiceTests class instance");
        }
    }
}