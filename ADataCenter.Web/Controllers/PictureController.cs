using ADataCenter.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADataCenter.Web
{
    [Route("IncidentApi/[controller]")]
    //[Route("IncidentApi")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        IConfiguration _configuration;
        string _image_path = string.Empty;
        IUnitOfWorkReport<ReportPage> _unit_of_work_repo;
        public PictureController(IConfiguration configuration, IWebHostEnvironment appEnvironment,
            IUnitOfWorkReport<ReportPage> repo)
        {
            _unit_of_work_repo = repo;
            _configuration = configuration;
            try
            {
                _image_path = _configuration["ImagePath"];
            }
            catch(Exception)
            { 
            }
            
            if(string.IsNullOrEmpty(_image_path))
            {
                _image_path = Path.Combine(appEnvironment.WebRootPath, "images");
            }
        }

        // GET: api/<PictureController>/5
        [HttpGet("Raw/{path}")]
        public IActionResult Get(string path)
        {
            Byte[] b;

            string ipath = Path.Combine(_image_path, path);
            b = System.IO.File.ReadAllBytes(ipath);
            
            return File(b, "image/jpeg");
        }

        // GET api/<PictureController>/5
        [HttpGet("GetAsBase64/{path}")]
        public ActionResult<ImageData> GetAsBase64(string path)
        {
            ImageData temp = _unit_of_work_repo.GetAsBase64(path);
            return Ok(temp);
        }


        [HttpPost]
        [Route("PostAsBase64")]
        public ActionResult PostAsBase64(ImageData value)
        {
            string ipath = Path.Combine(_image_path, value.path);
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(ipath));
            var bytes = Convert.FromBase64String(value.image_data);
            using (var imageFile = new FileStream(ipath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return Ok();
        }

        // DELETE api/<PictureController>/5
        [HttpDelete("{path}")]
        public ActionResult Delete(string path)
        {
            string ipath = Path.Combine(_image_path, path);
            System.IO.File.Delete(ipath);
            return Ok();
        }
    }
}
