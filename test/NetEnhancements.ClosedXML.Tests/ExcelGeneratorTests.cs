using ClosedXML.Excel;

using System.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Local
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
            Assert.That(workbook, Is.InstanceOf<XLWorkbook>());
            Assert.That(workbook.Worksheets, Has.Count.EqualTo(1));
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
            Assert.That(workbook, Is.InstanceOf<XLWorkbook>());
            Assert.That(workbook.Worksheets, Has.Count.EqualTo(1));

            var sheet = workbook.Worksheets.First();

            Assert.That(sheet.Rows().Count(), Is.EqualTo(dataList.Count + 1));

            Assert.Multiple(() =>
            {
                Assert.That(sheet.Columns().Count(), Is.EqualTo(2));
                Assert.That(sheet.Row(1).Cell(1).Value.ToString(), Is.EqualTo("Prop1"));
                Assert.That(sheet.Row(1).Cell(2).Value.ToString(), Is.EqualTo("Prop2"));
            });

            for (int i = 0; i < dataList.Count; i++)
            {
                var row = sheet.Row(i + 2);

                Assert.Multiple(() =>
                {
                    Assert.That(row.Cell(1).Value.ToString(), Is.EqualTo(dataList[i].Prop1));
                    Assert.That(row.Cell(2).Value.ToString(), Is.EqualTo(dataList[i].Prop2.ToString()));
                });
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
            Assert.That(workbook, Is.InstanceOf<XLWorkbook>());
            Assert.That(workbook.Worksheets, Has.Count.EqualTo(1));

            var sheet = workbook.Worksheets.First();
            
            Assert.Multiple(() =>
            {
                Assert.That(sheet.Rows().Count(), Is.EqualTo(dataList.Count + 1));
                Assert.That(sheet.Columns().Count(), Is.EqualTo(2));
                Assert.That(sheet.Row(1).Cell(1).Value.ToString(), Is.EqualTo("Prop1"));
                Assert.That(sheet.Row(1).Cell(2).Value.ToString(), Is.EqualTo("Prop2"));
            });
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
            Assert.That(workbook, Is.InstanceOf<XLWorkbook>());
            Assert.That(workbook.Worksheets, Has.Count.EqualTo(1));

            var sheet = workbook.Worksheets.First();

            Assert.Multiple(() =>
            {
                Assert.That(sheet.Rows().Count(), Is.EqualTo(dataList.Count + 1));
                Assert.That(sheet.Columns().Count(), Is.EqualTo(2));
                Assert.That(sheet.Row(1).Cell(1).Value.ToString(), Is.EqualTo("Property 1"));
                Assert.That(sheet.Row(1).Cell(2).Value.ToString(), Is.EqualTo("Prop2"));
            });

            for (int i = 0; i < dataList.Count; i++)
            {
                var row = sheet.Row(i + 2);

                Assert.Multiple(() =>
                {
                    Assert.That(row.Cell(1).Value.ToString(), Is.EqualTo(dataList[i].Prop1));
                    Assert.That(row.Cell(2).Value.ToString(), Is.EqualTo(dataList[i].Prop2.ToString()));
                });
            }
        }

        [Test]
        public void GenerateExcel_WithoutHeaders_KeepsCount()
        {
            // Arrange
            var dataList = new List<MyClassWithAttribute>
                               {
                                   new() { Prop1 = "A", Prop2 = 1, Prop3 = "Disabled" },
                                   new() { Prop1 = "B", Prop2 = 2, Prop3 = "Disabled" },
                                   new() { Prop1 = "C", Prop2 = 3, Prop3 = "Disabled" }
                               };

            // Act
            var workbook = ExcelGenerator.GenerateExcel(dataList, printHeaders: false);
            
            // Assert
            Assert.That(workbook, Is.InstanceOf<XLWorkbook>());
            Assert.That(workbook.Worksheets, Has.Count.EqualTo(1));

            var sheet = workbook.Worksheets.First();

            Assert.Multiple(() =>
            {
                Assert.That(sheet.Rows().Count(), Is.EqualTo(dataList.Count));
                Assert.That(sheet.Columns().Count(), Is.EqualTo(2));
            });

            for (int i = 0; i < dataList.Count; i++)
            {
                var row = sheet.Row(i + 1);

                Assert.Multiple(() =>
                {
                    Assert.That(row.Cell(1).Value.ToString(), Is.EqualTo(dataList[i].Prop1));
                    Assert.That(row.Cell(2).Value.ToString(), Is.EqualTo(dataList[i].Prop2.ToString()));
                });
            }

            Assert.That(sheet.Row(4).Cell(1).Value.ToString(), Is.EqualTo(""));
        }

        [Test]
        public void InsertDataInternal_NonEmptyList_ReturnsWorkbookWithOneWorksheet_WithExcelAttributes()
        {
            // Arrange
            var dataList = new List<MyClassWithStyleAttributes>
                               {
                                   new() { RightAligned = "A", MagicNumber = 12345789.123456789m, ConditionalAndUnconditional = 1, Ignored = "Disabled", ConditionalOnly = "Food" },
                                   new() { RightAligned = "B", MagicNumber = 12345789.123456789m, ConditionalAndUnconditional = 2, Ignored = "Disabled", ConditionalOnly = "Bar" },
                                   new() { RightAligned = "C", MagicNumber = 12345789.123456789m, ConditionalAndUnconditional = 3, Ignored = "Disabled", ConditionalOnly = null },
                                   new() { RightAligned = "D", MagicNumber = 12345789.123456789m, ConditionalAndUnconditional = -4, Ignored = "Disabled", ConditionalOnly = "" }
                               };

            // Act
            var workbook = new XLWorkbook();
            workbook.AddAndPopulateSheet(dataList, sheetName: "Fancy");

            // Assert
            Assert.That(workbook, Is.InstanceOf<XLWorkbook>());
            Assert.That(workbook.Worksheets, Has.Count.EqualTo(1));

            var sheet = workbook.Worksheets.First();
            
            Assert.Multiple(() =>
            {
                Assert.That(sheet.Rows().Count(), Is.EqualTo(dataList.Count + 1));
                Assert.That(sheet.Columns().Count(), Is.EqualTo(4));
                Assert.That(sheet.Row(1).Cell(1).Value.ToString(), Is.EqualTo("RightAligned"));
                Assert.That(sheet.Row(1).Cell(2).Value.ToString(), Is.EqualTo("MagicNumber"));
            });

            for (int i = 0; i < dataList.Count; i++)
            {
                var row = sheet.Row(i + 2);
                
                Assert.Multiple(() =>
                {
                    Assert.That(row.Cell(1).Style.Alignment.Horizontal, Is.EqualTo(XLAlignmentHorizontalValues.Right));
                    Assert.That(row.Cell(2).Style.Alignment.Horizontal, Is.EqualTo(XLAlignmentHorizontalValues.General));
                    Assert.That(row.Cell(2).Style.NumberFormat.Format, Is.EqualTo("#,##0.00"));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(sheet.ConditionalFormats.First().Style.Fill.BackgroundColor.ToString(), Is.EqualTo("FFFF60CC"));
                Assert.That(sheet.ConditionalFormats.First().Range.RangeAddress.FirstAddress.ColumnNumber, Is.EqualTo(3));
            });
        }

        [Test]
        public void ToDataSet_EmptyList_ReturnsDataSetWithEmptyTable()
        {
            // Arrange
            IReadOnlyCollection<MyClass> dataList = new List<MyClass>();

            // Act
            var dataSet = ExcelGenerator.ToDataSet(dataList);

            // Assert
            Assert.That(dataSet, Is.InstanceOf<DataSet>());
            Assert.That(dataSet.Tables, Has.Count.EqualTo(1));
            
            Assert.Multiple(() =>
            {
                Assert.That(dataSet.Tables[0].Rows, Is.Empty);
                Assert.That(dataSet.Tables[0].Columns, Has.Count.EqualTo(2));
            });
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

            var list1 = dataList.Select(x => new object?[] { x.Prop1, x.Prop2 }).ToList();
            var list2 = dataSet.Tables[0].AsEnumerable().Select(x => x.ItemArray).ToList();

            CollectionAssert.AreEquivalent(list1, list2);
        }

        private interface IRandomInterface
        {
        }

        private class MyClassWithInterface : MyClass, IRandomInterface
        {
        }

        private class MyClass
        {
            public string? Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        private class MyClassWithAttribute
        {
            [ExcelColumnName("Property 1")]
            public string? Prop1 { get; set; }
            public int Prop2 { get; set; }
            [ExcelColumnIgnored]
            public string? Prop3 { get; set; }
        }

        private class MyClassWithStyleAttributes
        {
            [ExcelColumnStyle(HorizontalAlignment = XLAlignmentHorizontalValues.Right)]
            public string? RightAligned { get; set; }

            [ExcelColumnStyle(NumberFormat = "#,##0.00")]
            public decimal MagicNumber { get; set; }
            
            [ExcelColumnConditionalStyle(Condition = Condition.WhenLessThan, Value = "0", FillColor = "#FF60CC")]
            [ExcelColumnStyle(FillColor = "#000000")]
            [ExcelColumnConditionalStyle(Condition = Condition.WhenEqualOrGreaterThan, Value = "0", FillColor = "#CCFF60")]
            public int ConditionalAndUnconditional { get; set; }
            
            [ExcelColumnIgnored]
            public string? Ignored { get; set; }

            [ExcelColumnConditionalStyle(Condition = Condition.WhenContains, Value = "foo", FillColor = "#60CCFF")]
            public string? ConditionalOnly { get; set; }
        }
    }
}