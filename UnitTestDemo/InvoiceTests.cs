using FluentAssertions;
using UnitTestDemo.Infrustracture;

namespace UnitTestDemo
{
    public class InvoiceTests
    {
        [Fact]
        public void TestTaxCalculation()
        {
            // Arrange
            var inv = new Invoice();
            inv.Amount = 100;
            inv.TaxRate = 0.10m;
            inv.TaxInclusive = false;

            // Act
            inv.Compute();

            // Assert
            //Assert.Equal(110, inv.NetAmount);
            inv.NetAmount.Should().Be(110m);
        }
    }
}