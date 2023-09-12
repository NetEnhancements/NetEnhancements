using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
	/// <summary>
	/// Provides extension methods for working with Excel workbooks.
	/// </summary>
	public static class WorkbookExtensions
	{
		/// <summary>
		/// Saves the workbook as a byte array in the Excel file format.
		/// </summary>
		/// <param name="workbook">The workbook to save.</param>
		/// <returns>A <see cref="byte"/> array representing the Excel file.</returns>
		public static byte[] ToBytes(this IXLWorkbook workbook)
		{
			using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			return stream.ToArray();
		}

		/// <summary>
		/// Adds a new worksheet to the workbook containing the data from a collection of objects.
		/// </summary>
		/// <typeparam name="T">The type of the objects in the collection.</typeparam>
		/// <param name="workbook">The workbook to add the worksheet to.</param>
		/// <param name="dataList">The collection of objects to add to the worksheet.</param>
		/// <param name="printHeaders">Whether to print the column header names above the data.</param>
		/// <param name="startingRow">The row where the data table should start</param>
		/// <param name="startingColumn">The column where the data table should start</param>
		/// <param name="sheetName">The name to give to the new worksheet.</param>
		/// <param name="createTable">Wheter to create a table of the data and create filters and basic styling</param>
		/// <returns>A <see cref="XLWorkbook" /> workbook with the new worksheet added.</returns>
		public static IXLWorkbook AddAndPopulateSheet<T>(
			this IXLWorkbook workbook,
			IReadOnlyCollection<T> dataList,
			bool printHeaders = true,
			int startingRow = 1,
			int startingColumn = 1,
			string? sheetName = null,
			bool createTable = false)
		{
			var sheet = workbook.Worksheets.Add();

			if (!string.IsNullOrWhiteSpace(sheetName))
			{
				sheet.Name = sheetName;
			}

			sheet.Populate(dataList, printHeaders, startingRow, startingColumn, createTable);

			return workbook;
		}

		/// <summary>
		/// Adjusts the width of all columns on every sheet based on its contents.
		/// </summary>
		/// <param name="workbook">The workbook to adjust the columns on.</param>
		/// <returns>A <see cref="XLWorkbook" /> workbook with the adjusted column width.</returns>
		public static IXLWorkbook AdjustToContents(this IXLWorkbook workbook)
		{
			foreach (var sheet in workbook.Worksheets)
			{
				sheet.AdjustToContents();
			}

			return workbook;
		}
	}
}
