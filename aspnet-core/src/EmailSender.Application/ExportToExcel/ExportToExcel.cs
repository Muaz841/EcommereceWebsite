using Abp.AspNetCore.Mvc.Controllers;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace EmailSender.Export
{
    public class ExportToExcel : AbpController
    {
        public async Task<IActionResult> ExportExcelFile<T>(IEnumerable<T> data, string fileName)
        {
           
            var dataTable = ToDataTable(data);

           
            using (var stream = new MemoryStream())
            {
                using (SLDocument sl = new SLDocument())
                {
                   
                    sl.ImportDataTable(1, 1, dataTable, true);
                    sl.SaveAs(stream);
                }
               
                string filePath = Path.Combine("C:\\Users\\lenovo\\Downloads", fileName + ".xlsx");
                System.IO.File.WriteAllBytes(filePath, stream.ToArray());
                
                return Ok($"File saved at:{filePath}");
            }
        }

        private DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            DataTable table = new DataTable();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        
            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                if (Nullable.GetUnderlyingType(propertyType) != null)
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }
                table.Columns.Add(property.Name, propertyType);
            }


            foreach (var item in data)
            {
                DataRow row = table.NewRow();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item, null);
                    row[property.Name] = value ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
