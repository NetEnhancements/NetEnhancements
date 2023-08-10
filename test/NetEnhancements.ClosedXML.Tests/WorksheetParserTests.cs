using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML.Tests
{
    public class WorksheetParserTests
    {
        private class OrderData
        {
            [ExcelColumnAddress("A")]
            public string? OrderNumber { get; set; }

            [ExcelColumnAddress("B")]
            public string? ClientName { get; set; }
            
            [ExcelColumnAddress(4)]
            public decimal TotalOrderAmount { get; set; }
        }

        [Test]
        public async Task ParseRowsAsync()
        {
            // Arrange
            using var workbook = new XLWorkbook("Data/OrderData.xlsx");
            var worksheet = workbook.Worksheets.First();
            var recordList = new List<OrderData>();

            // Act
            await foreach (var row in worksheet.ParseRowsAsync<OrderData>(rowsToSkip: 1))
            {
                recordList.Add(row);
            }

            // Assert
            Assert.That(recordList, Has.Count.EqualTo(9));

            var sixthRecord = recordList[5];

            Assert.That(sixthRecord, Is.Not.Null);

            Assert.Multiple(()=>
            {
                Assert.That(sixthRecord.OrderNumber, Is.EqualTo("0006"));
                Assert.That(sixthRecord.ClientName, Is.EqualTo("Relation #6"));
                Assert.That(sixthRecord.TotalOrderAmount, Is.EqualTo(37.48));
            });
        }
    }
}
