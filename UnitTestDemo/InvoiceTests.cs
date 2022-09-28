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
            Assert.Equal(110, inv.NetAmount);

            // Re-Arrange
            inv.TaxInclusive = true;
            // Act
            inv.Compute();

            // Assert
            Assert.Equal(100, inv.NetAmount);

            // Testing different behavior in one test is a bad practice
            //inv.Cancel();
            //Assert.Equal(true, inv.IsCanceled);
        }
    }
}