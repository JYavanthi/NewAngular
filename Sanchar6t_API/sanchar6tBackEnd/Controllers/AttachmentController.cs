using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Models;
using Microsoft.AspNetCore.Authorization;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly Sanchar6tDbContext _context;

        public AttachmentController(Sanchar6tDbContext context)
        {
            this._context = context;
        }
        [HttpPost("addFile")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadFile([FromForm] AddAttachment attachment)
        {
            try
            {
                if (attachment.file == null || attachment.file.Length == 0)
                {
                    return BadRequest("No file selected for upload.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Sanchar6tFile", "Uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var attachFile = new Models.Attachment
                {
                    AttachmentName = attachment.AttachmentName,
                    AttachmentFile = attachment.file.FileName,
                    UserId = attachment.UserID,
                    Section = attachment.Section,
                    Sortorder = attachment.Sortorder,
                    PackageId = attachment.PackageID,
                    CreatedBy = attachment.CreatedBy,
                    CreatedDt = DateTime.Now,
                    ModifiedBy = attachment.CreatedBy,
                    ModifiedDt = DateTime.Now
                };

                if (!(attachment.UserID == 0))
                {
                    attachFile.UserId = attachment.UserID;
                }
                _context.Attachments.Add(attachFile);
                await _context.SaveChangesAsync();

                var attachId = attachFile.AttachmentId;

                var fileName = $"{attachId}_{attachment.file.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachment.file.CopyToAsync(stream);
                }

                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetFileData")]
        [AllowAnonymous]
        public IActionResult GetFileData(int PackageID, string Section)
        {
            try
            {
                if (PackageID == 0)
                {
                    return BadRequest("Service ID and fileName are required");
                }

                var attachmentDataList = _context.Attachments
                            .Where(m => m.Section == Section && m.PackageId == PackageID)
                            .ToList(); return Ok(attachmentDataList);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpDelete("Delete/{attachmentId}")]
        public async Task<IActionResult> DeleteFile(int attachmentId)
        {
            try
            {
                var attachFile = await _context.Attachments.FindAsync(attachmentId);
                if (attachFile == null)
                {
                    return NotFound("File not found.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Sanchar6tFile", "Uploads");
                var fileName = $"{attachmentId}_{attachFile.AttachmentName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.Attachments.Remove(attachFile);
                await _context.SaveChangesAsync();

                return Ok(new { message = "File deleted successfully." });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet("Download/{attachId}")]
        public async Task<IActionResult> DownloadFile(int attachId)
        {
            try
            {
                var attachFile = await _context.Attachments.FindAsync(attachId);
                if (attachFile == null)
                {
                    return NotFound("File not found.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Sanchar6tFile", "Uploads");
                var fileName = $"{attachId}_{attachFile.AttachmentFile}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found on the server.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "application/octet-stream", attachFile.AttachmentFile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet("View/{attachId}")]
        //public async Task<IActionResult> viewFile(int attachId)
        //{
        //    try
        //    {
        //        var attachFile = await _context.Attachments.FindAsync(attachId);
        //        if (attachFile == null)
        //        {
        //            return NotFound("File not found.");
        //        }

        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Sanchar6tFile", "Uploads");
        //        var fileName = $"{attachId}_{attachFile.AttachmentFile}";
        //        var filePath = Path.Combine(uploadsFolder, fileName);

        //        if (!System.IO.File.Exists(filePath))
        //        {
        //            return NotFound("File not found on the server.");
        //        }

        //        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

        //        var contentType = GetContentType(filePath);

        //        return File(fileBytes, contentType, attachFile.AttachmentFile);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpGet("View/{attachmentId}")]
        public async Task<IActionResult> View(int attachmentId)
        {
            // 1. Lookup metadata
            var attachFile = await _context.Attachments.FindAsync(attachmentId);
            if (attachFile == null)
                return NotFound("Attachment not found.");

            // 2. Build physical path (must match your Uploads folder)
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Sanchar6tFile", "Uploads");
            var fileName = $"{attachmentId}_{attachFile.AttachmentFile}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File on disk not found.");

            // 3. Stream it back
            //    You can use PhysicalFile() for brevity, or manually load into memory:
            var ext = Path.GetExtension(attachFile.AttachmentFile)?.ToLowerInvariant();
            var contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };

            return PhysicalFile(filePath, contentType);
        }
        private string GetContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();

            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

    }
}

