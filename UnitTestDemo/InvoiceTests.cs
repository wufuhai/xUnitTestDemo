using FluentAssertions;
using UnitTestDemo.Infrustracture;
using Xunit.Abstractions;

namespace UnitTestDemo
{
    public class InvoiceTests : IDisposable, IClassFixture<InvoiceFixture>
    {
        Invoice _inv => _fixture.Invoice; //new Invoice();
        private readonly ITestOutputHelper _output;
        private readonly InvoiceFixture _fixture;

        public InvoiceTests(ITestOutputHelper output, InvoiceFixture fixture)
        {
            _output = output;
            _fixture = fixture;
            //_inv.Load();
            _output.WriteLine("Creating InvoiceTests class instance");
        }

        //[Fact]
        [Theory]
        //[InlineData(0, 0.06, false, 0)]
        //[InlineData(0.01, 0.06, false, 0.01)]
        //[InlineData(0.01, 0.06, true, 0.01)]
        //[InlineData(100, 0.06, false, 106)]
        //[InlineData(100, 0.06, true, 100)]
        [ExcelData("InvoiceTestCases.xlsx", StartRow = 1)]
        public void TestTaxCalculation(string caseId, decimal amount, decimal rate, bool inclusive, decimal expected)
        {
            // Arrange
            _inv.Amount = amount;
            _inv.TaxRate = rate;
            _inv.TaxInclusive = inclusive;

            // Act
            _inv.Compute();

            // Assert
            _inv.NetAmount.Should().Be(expected);
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
            _output.WriteLine("Disposing InvoiceTests class instance");
        }
    }
}