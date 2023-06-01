using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OkVip.ManagementDataMarketing.Commons.Constants;
using OkVip.ManagementDataMarketing.Services;
using System;
using System.Data;
using System.IO;
using System.Data.OleDb;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Collections;
using OfficeOpenXml;
using System.Net.Http;
using OfficeOpenXml.Style;
using System.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Models.DbModels;
using System.Collections.Generic;

namespace OkVip.ManagementDataMarketing.Controllers
{
    [Authorize]
    public class DataMarketingController : Controller
    {
        private readonly IDataMarketingServices _dataMarketingService;
        private readonly IMapper _mapper;
        private IConfiguration Configuration;
        private IWebHostEnvironment Environment;
        public DataMarketingController(IDataMarketingServices dataMarketingService, IMapper mapper, IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            _dataMarketingService = dataMarketingService;
            _mapper = mapper;
            Environment = _environment;
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            try
            {
                return View(_dataMarketingService.GetAll());
            }
            catch (Exception ex)
            {
                TempData[TempDataConstants.TEMP_DATA_ERROR_MESSAGE] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportDataAsync(IFormFile postedFile)
        {
            try
            {
                int countDuplicate = 0;
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await postedFile.CopyToAsync(stream);
                    }

                    //Read the connection string for the Excel file.
                    string conString = this.Configuration.GetConnectionString("ExcelConString");
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                                //Read Data from First Sheet.
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);

                                connExcel.Close();
                            }
                        }
                    }

                    System.Data.DataColumn isDuplicate = new System.Data.DataColumn("IsDuplicate", typeof(System.Boolean));
                    isDuplicate.DefaultValue = false;
                    dt.Columns.Add(isDuplicate);

                    System.Data.DataColumn isDeleted = new System.Data.DataColumn("IsDeleted", typeof(System.Boolean));
                    isDeleted.DefaultValue = false;
                    dt.Columns.Add(isDeleted);
                    System.Data.DataColumn createDate = new System.Data.DataColumn("CreatedDate", typeof(System.DateTime));
                    createDate.DefaultValue = DateTime.Now;
                    dt.Columns.Add(createDate);
                    System.Data.DataColumn updateDate = new System.Data.DataColumn("UpdatedDate", typeof(System.DateTime));
                    updateDate.DefaultValue = DateTime.Now;
                    dt.Columns.Add(updateDate);

                    //foreach (DataRow item in dt.Rows)
                    //{
                    //    item["IsDuplicate"] = false;
                    //    item["IsDeleted"] = false;
                    //    item["CreatedDate"] = DateTime.Now;
                    //    item["UpdatedDate"] = DateTime.Now;
                    //}

                    //dt = RemoveDuplicateRows(dt, "SDT", out countDuplicate);
                    //List<DataMarketing> listData = new List<DataMarketing>();
                    //foreach (DataRow dRow in dt.Rows)
                    //{
                    //    var checkDuplicate = false;
                    //    if(_dataMarketingService.CheckExist(dRow["SDT"].ToString()))
                    //    {
                    //        checkDuplicate = true;
                    //    }
                    //    DataMarketing data = new DataMarketing()
                    //    {
                    //        PhoneNumber = dRow["SDT"].ToString(),
                    //        EmployeeName = dRow["NHÂN VIÊN"].ToString(),
                    //        BuyDate = dRow["NGÀY MUA"].ToString(),
                    //        DataBuyOfWeb = dRow["DATA TRANG WEB"].ToString(),
                    //        Note = dRow["GHI CHÚ"].ToString(),
                    //        IsDuplicate = checkDuplicate,
                    //        IsDeleted = false,
                    //        CreatedDate =  DateTime.Now,
                    //        UpdatedDate= DateTime.Now
                    //    };
                    //    listData.Add(data);
                    //}

                    //_dataMarketingService.Insert(listData);

                    //Insert the Data read from the Excel file to Database Table.
                    conString = this.Configuration.GetConnectionString("DefaultConnection");
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.DataMarketing";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add("SDT", "PhoneNumber");
                            sqlBulkCopy.ColumnMappings.Add("NHÂN VIÊN", "EmployeeName");
                            sqlBulkCopy.ColumnMappings.Add("NGÀY MUA", "BuyDate");
                            sqlBulkCopy.ColumnMappings.Add("DATA TRANG WEB", "DataBuyOfWeb");
                            sqlBulkCopy.ColumnMappings.Add("GHI CHÚ", "Note");
                            sqlBulkCopy.ColumnMappings.Add("IsDuplicate", "IsDuplicate");
                            sqlBulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                            sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                            sqlBulkCopy.ColumnMappings.Add("UpdatedDate", "UpdatedDate");

                            con.Open();
                            await sqlBulkCopy.WriteToServerAsync(dt);
                            con.Close();
                        }
                    }
                    TempData[TempDataConstants.TEMP_DATA_INFO_MESSAGE] = "Thêm mới thành công \"" + dt.Rows.Count.ToString() + "\" bản ghi ";
                }
            }
            catch (Exception ex)
            {
                TempData[TempDataConstants.TEMP_DATA_ERROR_MESSAGE] = ex.Message;
            }

            return RedirectToAction("Index", "DataMarketing");

        }

        public IActionResult ExportToExcel()
        {

            var users = _dataMarketingService.GetAllNoDuplicate();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Users");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                const int startRow = 5;
                var row = startRow;

                //Create Headers and format them
                worksheet.Cells["A1"].Value = "SDT";
                worksheet.Cells["B1"].Value = "NV";
                worksheet.Cells["C1"].Value = "BuyDate";
                worksheet.Cells["D1"].Value = "DataOfWeb";
                worksheet.Cells["E1"].Value = "Note";


                row = 2;
                foreach (var user in users)
                {
                    worksheet.Cells[row, 1].Value = user.PhoneNumber;
                    worksheet.Cells[row, 2].Value = user.EmployeeName;
                    worksheet.Cells[row, 3].Value = user.BuyDate;
                    worksheet.Cells[row, 4].Value = user.DataBuyOfWeb;
                    worksheet.Cells[row, 5].Value = user.Note;
                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Danh sách dữ liệu đã lọc";
                xlPackage.Workbook.Properties.Author = "Akemi OKVip";
                xlPackage.Workbook.Properties.Subject = "Danh sách dữ liệu đã lọc";
                // save the new spreadsheet
                xlPackage.SaveAsync();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data-" + DateTime.Now.ToShortDateString() + ".xlsx");
        }

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName, out int countDuplicate)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            countDuplicate = 0;
            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
            {
                dRow["IsDuplicate"] = true;
                countDuplicate++;
            }
            //Datatable which contains unique records will be return as output.
            return dTable;
        }

        public IActionResult ExportDuplicateDataToExcel()
        {

            var users = _dataMarketingService.GetDuplicateData();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Users");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                const int startRow = 5;
                var row = startRow;

                //Create Headers and format them
                worksheet.Cells["A1"].Value = "SDT";
                worksheet.Cells["B1"].Value = "NV";
                worksheet.Cells["C1"].Value = "BuyDate";
                worksheet.Cells["D1"].Value = "DataOfWeb";
                worksheet.Cells["E1"].Value = "Note";


                row = 2;
                foreach (var user in users)
                {
                    worksheet.Cells[row, 1].Value = user.PhoneNumber;
                    worksheet.Cells[row, 2].Value = user.EmployeeName;
                    worksheet.Cells[row, 3].Value = user.BuyDate;
                    worksheet.Cells[row, 4].Value = user.DataBuyOfWeb;
                    worksheet.Cells[row, 5].Value = user.Note;
                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Danh sách dữ liệu trùng";
                xlPackage.Workbook.Properties.Author = "Akemi OKVip";
                xlPackage.Workbook.Properties.Subject = "Danh sách dữ liệu trùng";
                // save the new spreadsheet
                xlPackage.SaveAsync();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data-" + DateTime.Now.ToShortDateString() + ".xlsx");
        }
        public IActionResult DeleteDuplicateData()
        {
            try
            {
                _dataMarketingService.DeleteDuplicate();
                return RedirectToAction("Index", "DataMarketing");
            }
            catch (Exception ex)
            {
                TempData[TempDataConstants.TEMP_DATA_ERROR_MESSAGE] = ex.Message;
            }
            return View();
        }
    }
}
