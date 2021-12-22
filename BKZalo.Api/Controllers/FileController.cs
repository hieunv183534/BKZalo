using BKZalo.Core.Entities;
using BKZalo.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BKZalo.Api.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class FileController : ControllerBase
    {

        protected IBaseService<Account> _accountService;
        public FileController(IBaseService<Account> accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("uploadFiles")]
        public async Task<IActionResult> UploadFiles()
        {
            try
            {
                List<string> fileUrls = new List<string>();

                var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
                var accountId = ((Account)_accountService.GetByProp("PhoneNumber", phoneNumber).Response.Data).AccountId;

                var files = HttpContext.Request.Form.Files;
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images");
                foreach (var file in files)
                {
                    var newFileName = $"{accountId}X{Guid.NewGuid()}";
                    string filePath = Path.Combine(uploads, newFileName + Path.GetExtension(file.FileName));
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    fileUrls.Add(newFileName + Path.GetExtension(file.FileName));
                }
                return Ok(fileUrls);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }       
        }

        [HttpPost("deleteFiles")]
        public IActionResult DeleteFiles([FromBody] List<string> fileNames)
        {
            try
            {
                string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images");
                foreach (var fileName in fileNames)
                {
                    var fullPath = Path.Combine(fileDirectory, fileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("getFile/{fileName}")]
        public IActionResult GetFile([FromRoute] string fileName)
        {
            try
            {
                 var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images", fileName);
                 var imageFileStream = System.IO.File.OpenRead(path);
                 return File(imageFileStream, "image/jpeg");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
