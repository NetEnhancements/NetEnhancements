using System.ComponentModel.DataAnnotations.Schema;

namespace NetEnhancements.ClosedXML.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Arrange

            var input = new List<TestInput>()
                            {
                                new TestInput {FirstName = "1", LastName = "2", DateOfBorth = DateTime.Now, PinCode = 1234},
                                new TestInput {FirstName = "3", LastName = "4", DateOfBorth = DateTime.Now, PinCode = 3456},
                                new TestInput {FirstName = "5", LastName = "6", DateOfBorth = DateTime.Now, PinCode = 5678},
                            };

            var classUnderTest = ExcelGenerator.GenerateExcel(input);

            var output = new RowParser<TestOutput>().ParseRow(classUnderTest.Worksheets.First().Rows().Skip(1).First());

            // Act

            // Assert
            Assert.Pass();
        }
    }

    public class TestInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ExcelColumnName("Date of Borth")]
        public DateTime DateOfBorth { get; set; }

        [ExcelColumnDisabled]
        public int PinCode { get; set; }
    }

    public class TestOutput
    {
        [Column("A ")]
        public string FirstName { get; set; }
        [Column("B")]
        public string LastName { get; set; }
        [Column("C")]
        public DateTime DateOfBorth { get; set; }
    }
}