---
title: ClosedXML Import & Export
---

# ClosedXML Enhancements

[ClosedXML](https://github.com/ClosedXML/ClosedXML) is a popular, MIT-licensed library to read and write Excel files. 

We have got some extensions that make it easier to use.

## Exporting rows to Excel files

This section is subject to change.

Given a list of class instances containing your data:

```csharp
private class MyClassWithAttribute
{
    [ExcelColumnName("Property 1")]
    public string Prop1 { get; set; }
    
    public int Prop2 { get; set; }
    
    [ExcelColumnDisabled]
    public string Prop3 { get; set; }
}

var dataList = new List<MyClassWithAttribute>
{
    new() { Prop1 = "A", Prop2 = 1, Prop3 = "Disabled" },
    new() { Prop1 = "B", Prop2 = 2, Prop3 = "Disabled" },
    new() { Prop1 = "C", Prop2 = 3, Prop3 = "Disabled" }
};
```

You can call one of a couple of extension methods:

* [`var workbook = ExcelGenerator.GenerateExcel(dataList);`](/api/NetEnhancements.ClosedXML.ExcelGenerator.html#NetEnhancements_ClosedXML_ExcelGenerator_GenerateExcel__1_System_Collections_Generic_IReadOnlyCollection___0__System_Boolean_System_Int32_System_Int32_System_String_)
* [`IXLWorkbook book = ...; book.AddAndPopulateSheet(dataList);`](/api/NetEnhancements.ClosedXML.WorkbookExtensions.html#NetEnhancements_ClosedXML_WorkbookExtensions_AddAndPopulateSheet__1_ClosedXML_Excel_XLWorkbook_System_Collections_Generic_IReadOnlyCollection___0__System_Boolean_System_Int32_System_Int32_System_String_)
* [`IXLWorksheet sheet = ...; sheet.Populate(dataList);`](/api/NetEnhancements.ClosedXML.WorksheetExtensions.html#NetEnhancements_ClosedXML_WorksheetExtensions_Populate__1_ClosedXML_Excel_IXLWorksheet_System_Collections_Generic_IReadOnlyCollection___0__System_Boolean_System_Int32_System_Int32_)

All of these will result in the sheet being populated with the data from the `dataList`.

## Parsing rows to objects
When you've got an Excel file containing some data and you want to parse that to a collection of objects, you can use the extension method [`IXLWorksheet.ParseRowsAsync<TRow>()`](https://netenhancements.github.io/api/NetEnhancements.ClosedXML.WorksheetExtensions.html#NetEnhancements_ClosedXML_WorksheetExtensions_ParseRowsAsync__1_ClosedXML_Excel_IXLWorksheet_System_Int32_).

Given a class to parse into, having a parameterless constructor:

```csharp
public class YourRowClass
{
    [ExcelColumnAddress("A")]
    public string? OrderedProduct { get; set; }

    [ExcelColumnAddress("B")]
    public decimal? TotalAmount { get; set; }
}
```

Now you can parse the data from a worksheet in a workbook like this:

```csharp
using var workbook = new XLWorkbook("Path/To/Data.xlsx");

var worksheet = workbook.Worksheets.First();

var rows = new List<YourRowClass>();

await foreach (var row in worksheet.ParseRowsAsync<YourRowClass>(rowsToSkip: 1).WithCancellation(cancellationToken))
{
    rows.Add(row);
}
```

Now `rows` contains all data.

