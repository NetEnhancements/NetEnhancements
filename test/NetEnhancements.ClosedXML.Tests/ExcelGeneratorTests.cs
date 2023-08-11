using ClosedXML.Excel;

using System.Data;

namespace NetEnhancements.ClosedXML.Tests
{
    public class ExcelGeneratorTests
    {
        [Test]
        public void GenerateExcel_NonEmptyList_ReturnsWorkbookWithOneWorksheet()
        {
            // Arrange
            var dataList = new List<MyClass>
                               {
                                   new() { Prop1 = "A", Prop2 = 1 },
                                   new() { Prop1 = "B", Prop2 = 2 },
                                   new() { Prop1 = "C", Prop2 = 3 }
                               };

            // Act
            var workbook = ExcelGenerator.GenerateExcel(dataList);

            // Assert
            Assert.IsInstanceOf<XLWorkbook>(workbook);
            Assert.That(workbook.Worksheets.Count, Is.EqualTo(1));
        }

        [Test]
        public void GenerateExcel_NonEmptyList_ReturnsWorkbookWithOneWorksheet_WithFourRowsAndTwoColumns()
        {
            // Arrange
            var dataList = new List<MyClass>
                               {
                                   new() { Prop1 = "A", Prop2 = 1 },
                                   new() { Prop1 = "B", Prop2 = 2 },
                                   new() { Prop1 = "C", Prop2 = 3 }
                               };

            // Act
            var workbook = ExcelGenerator.GenerateExcel(dataList);

            // Assert
            Assert.IsInstanceOf<XLWorkbook>(workbook);
            Assert.That(workbook.Worksheets.Count, Is.EqualTo(1));
            Assert.That(workbook.Worksheets.First().Rows().Count(), Is.EqualTo(dataList.Count + 1));
            Assert.That(workbook.Worksheets.First().Columns().Count(), Is.EqualTo(2));
            Assert.That(workbook.Worksheets.First().Row(1).Cell(1).Value.ToString(), Is.EqualTo("Prop1"));
            Assert.That(workbook.Worksheets.First().Row(1).Cell(2).Value.ToString(), Is.EqualTo("Prop2"));

            for (int i = 0; i < dataList.Count; i++)
            {
                Assert.That(workbook.Worksheets.First().Row(i + 2).Cell(1).Value.ToString(), Is.EqualTo(dataList[i].Prop1));
                Assert.That(workbook.Worksheets.First().Row(i + 2).Cell(2).Value.ToString(), Is.EqualTo(dataList[i].Prop2.ToString()));
            }
        }

        [Test]
        public void GenerateExcel_Uses_First_Item_Type_ReturnsWorkbookWithOneWorksheet_WithFourRowsAndTwoColumns()
        {
            var dataList = new List<IRandomInterface>
            {
                new MyClassWithInterface() { Prop1 = "A", Prop2 = 1 },
                new MyClassWithInterface() { Prop1 = "B", Prop2 = 2 },
                new MyClassWithInterface() { Prop1 = "C", Prop2 = 3 }
            };

            // Act
            var workbook = ExcelGenerator.GenerateExcel(dataList, createTable: true);

            // Assert
            Assert.IsInstanceOf<XLWorkbook>(workbook);
            Assert.That(workbook.Worksheets.Count, Is.EqualTo(1));
            Assert.That(workbook.Worksheets.First().Rows().Count(), Is.EqualTo(dataList.Count + 1));
            Assert.That(workbook.Worksheets.First().Columns().Count(), Is.EqualTo(2));
            Assert.That(workbook.Worksheets.First().Row(1).Cell(1).Value.ToString(), Is.EqualTo("Prop1"));
            Assert.That(workbook.Worksheets.First().Row(1).Cell(2).Value.ToString(), Is.EqualTo("Prop2"));
        }

        [Test]
        public void GenerateExcel_NonEmptyList_ReturnsWorkbookWithOneWorksheet_WithExcelAttributes()
        {
            // Arrange
            var dataList = new List<MyClassWithAttribute>
                               {
                                   new() { Prop1 = "A", Prop2 = 1, Prop3 = "Disabled" },
                                   new() { Prop1 = "B", Prop2 = 2, Prop3 = "Disabled" },
                                   new() { Prop1 = "C", Prop2 = 3, Prop3 = "Disabled" }
                               };

            // Act
            var workbook = ExcelGenerator.GenerateExcel(dataList);

            // Assert
            Assert.IsInstanceOf<XLWorkbook>(workbook);
            Assert.That(workbook.Worksheets.Count, Is.EqualTo(1));
            Assert.That(workbook.Worksheets.First().Rows().Count(), Is.EqualTo(dataList.Count + 1));
            Assert.That(workbook.Worksheets.First().Columns().Count(), Is.EqualTo(2));
            Assert.That(workbook.Worksheets.First().Row(1).Cell(1).Value.ToString(), Is.EqualTo("Property 1"));
            Assert.That(workbook.Worksheets.First().Row(1).Cell(2).Value.ToString(), Is.EqualTo("Prop2"));

            for (int i = 0; i < dataList.Count; i++)
            {
                Assert.That(workbook.Worksheets.First().Row(i + 2).Cell(1).Value.ToString(), Is.EqualTo(dataList[i].Prop1));
                Assert.That(workbook.Worksheets.First().Row(i + 2).Cell(2).Value.ToString(), Is.EqualTo(dataList[i].Prop2.ToString()));
            }
        }

        [Test]
        public void InsertDataInternal_NonEmptyList_ReturnsWorkbookWithOneWorksheet_WithExcelAttributes()
        {
            // Arrange
            var dataList = new List<MyClassWithStyleAttribute>
                               {
                                   new() { Prop1 = "A", MagicNumber = 12345789.123456789m, Prop2 = 1, Prop3 = "Disabled" },
                                   new() { Prop1 = "B", MagicNumber = 12345789.123456789m, Prop2 = 2, Prop3 = "Disabled" },
                                   new() { Prop1 = "C", MagicNumber = 12345789.123456789m, Prop2 = 3, Prop3 = "Disabled" }
                               };

            // Act
            var wb = new XLWorkbook();
            wb.AddAndPopulateSheet(dataList, sheetName: "Fancy");

            // Assert
            Assert.IsInstanceOf<XLWorkbook>(wb);
            Assert.That(wb.Worksheets.Count, Is.EqualTo(1));
            Assert.That(wb.Worksheets.First().Rows().Count(), Is.EqualTo(dataList.Count + 1));
            Assert.That(wb.Worksheets.First().Columns().Count(), Is.EqualTo(3));
            Assert.That(wb.Worksheets.First().Row(1).Cell(1).Value.ToString(), Is.EqualTo("Prop1"));
            Assert.That(wb.Worksheets.First().Row(1).Cell(2).Value.ToString(), Is.EqualTo("MagicNumber"));

            for (int i = 0; i < dataList.Count; i++)
            {
                Assert.That(wb.Worksheets.First().Row(i + 2).Cell(1).Style.Alignment.Horizontal, Is.EqualTo(XLAlignmentHorizontalValues.Right));
                Assert.That(wb.Worksheets.First().Row(i + 2).Cell(2).Style.Alignment.Horizontal, Is.EqualTo(XLAlignmentHorizontalValues.General));
                Assert.That(wb.Worksheets.First().Row(i + 2).Cell(2).Style.NumberFormat.Format, Is.EqualTo("#,##0.00"));
            }
        }

        [Test]
        public void ToDataSet_EmptyList_ReturnsDataSetWithEmptyTable()
        {
            // Arrange
            var dataList = new List<MyClass>();

            // Act
            var dataSet = ExcelGenerator.ToDataSet(dataList);

            // Assert
            Assert.IsInstanceOf<DataSet>(dataSet);
            Assert.That(dataSet.Tables.Count, Is.EqualTo(1));
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(0));
            Assert.That(dataSet.Tables[0].Columns.Count, Is.EqualTo(2));
        }

        [Test]
        public void ToDataSet_NonEmptyList_ReturnsDataSetWithTableContainingObjectData()
        {
            // Arrange
            var dataList = new List<MyClass>
                               {
                                   new() { Prop1 = "A", Prop2 = 1 },
                                   new() { Prop1 = "B", Prop2 = 2 },
                                   new() { Prop1 = "C", Prop2 = 3 }
                               };

            // Act
            var dataSet = ExcelGenerator.ToDataSet(dataList);

            // Assert
            Assert.That(dataSet, Is.InstanceOf<DataSet>());
            Assert.That(dataSet.Tables, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(dataSet.Tables[0].Rows, Has.Count.EqualTo(dataList.Count));
                Assert.That(dataSet.Tables[0].Columns, Has.Count.EqualTo(2));
            });
            Assert.Multiple(() =>
            {
                Assert.That(dataSet.Tables[0].Columns[0].ColumnName, Is.EqualTo("Prop1"));
                Assert.That(dataSet.Tables[0].Columns[1].ColumnName, Is.EqualTo("Prop2"));
            });
            CollectionAssert.AreEquivalent(dataList.Select(x => new object[] { x.Prop1, x.Prop2 }).ToList(), dataSet.Tables[0].AsEnumerable().Select(x => x.ItemArray).ToList());
        }

        private interface IRandomInterface
        {
        }

        private class MyClassWithInterface : MyClass, IRandomInterface
        {
        }

        private class MyClass
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        private class MyClassWithAttribute
        {
            [ExcelColumnName("Property 1")]
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            [ExcelColumnDisabled]
            public string Prop3 { get; set; }
        }

        private class MyClassWithStyleAttribute
        {
            [ExcelColumnStyle(HorizontalAlignment = XLAlignmentHorizontalValues.Right)]
            public string Prop1 { get; set; }

            [ExcelColumnStyle(NumberFormat = "#,##0.00")]
            public decimal MagicNumber { get; set; }
            [ExcelColumnStyle(FillColor = "#000000")]
            public int Prop2 { get; set; }
            [ExcelColumnDisabled]
            public string Prop3 { get; set; }
        }
    }
}