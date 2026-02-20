using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Microsoft.EntityFrameworkCore;
//using QuestPDF.Helpers;de
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using Microsoft.AspNetCore.Authorization;


namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusBookingExportController : ControllerBase
    {
        private readonly Sanchar6tDbContext _context;

        public BusBookingExportController(Sanchar6tDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetExcel")]
        [AllowAnonymous]
        public IActionResult GetExcel()
        {
            var data = _context.VwBusBookingSeats.AsNoTracking().ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("BusBookingSeat");

            sheet.Cells["A1"].LoadFromCollection(data, true, OfficeOpenXml.Table.TableStyles.Light1);

            return File(package.GetAsByteArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "BusBookingSeat.xlsx");
        }

    }
}
