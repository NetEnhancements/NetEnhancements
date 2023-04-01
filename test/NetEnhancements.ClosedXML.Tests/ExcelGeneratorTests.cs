using ClosedXML.Excel;

using System.Data;

namespace NetEnhancements.ClosedXML.Tests
{
    public class ExcelGeneratorTests
    {
        [Test]
        public void GenerateExcel_EmptyList_ReturnsEmptyWorkbook()
        {
            // Arrange
            var dataList = new List<MyClass>();

            // Act
            var workbook = ExcelGenerator.GenerateExcel(dataList);

            // Assert
            Assert.IsInstanceOf<XLWorkbook>(workbook);
            Assert.AreEqual(0, workbook.Worksheets.Count);
        }

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
            Assert.AreEqual(1, workbook.Worksheets.Count);
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
            Assert.AreEqual(1, workbook.Worksheets.Count);
            Assert.AreEqual(dataList.Count + 1, workbook.Worksheets.First().Rows().Count());
            Assert.AreEqual(2, workbook.Worksheets.First().Columns().Count());
            Assert.AreEqual("Prop1", workbook.Worksheets.First().Row(1).Cell(1).Value.ToString());
            Assert.AreEqual("Prop2", workbook.Worksheets.First().Row(1).Cell(2).Value.ToString());

            for (int i = 0; i < dataList.Count; i++)
            {
                Assert.AreEqual(dataList[i].Prop1, workbook.Worksheets.First().Row(i + 2).Cell(1).Value.ToString());
                Assert.AreEqual(dataList[i].Prop2.ToString(), workbook.Worksheets.First().Row(i + 2).Cell(2).Value.ToString());
            }
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
            Assert.AreEqual(1, workbook.Worksheets.Count);
            Assert.AreEqual(dataList.Count + 1, workbook.Worksheets.First().Rows().Count());
            Assert.AreEqual(2, workbook.Worksheets.First().Columns().Count());
            Assert.AreEqual("Property 1", workbook.Worksheets.First().Row(1).Cell(1).Value.ToString());
            Assert.AreEqual("Prop2", workbook.Worksheets.First().Row(1).Cell(2).Value.ToString());

            for (int i = 0; i < dataList.Count; i++)
            {
                Assert.AreEqual(dataList[i].Prop1, workbook.Worksheets.First().Row(i + 2).Cell(1).Value.ToString());
                Assert.AreEqual(dataList[i].Prop2.ToString(), workbook.Worksheets.First().Row(i + 2).Cell(2).Value.ToString());
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
            Assert.AreEqual(1, dataSet.Tables.Count);
            Assert.AreEqual(0, dataSet.Tables[0].Rows.Count);
            Assert.AreEqual(2, dataSet.Tables[0].Columns.Count);
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
            Assert.IsInstanceOf<DataSet>(dataSet);
            Assert.AreEqual(1, dataSet.Tables.Count);
            Assert.AreEqual(dataList.Count, dataSet.Tables[0].Rows.Count);
            Assert.AreEqual(2, dataSet.Tables[0].Columns.Count);
            Assert.AreEqual("Prop1", dataSet.Tables[0].Columns[0].ColumnName);
            Assert.AreEqual("Prop2", dataSet.Tables[0].Columns[1].ColumnName);
            CollectionAssert.AreEquivalent(dataList.Select(x => new object[] { x.Prop1, x.Prop2 }).ToList(), dataSet.Tables[0].AsEnumerable().Select(x => x.ItemArray).ToList());
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
    }
}