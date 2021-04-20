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
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PictureController : ControllerBase
    {
        IConfiguration _configuration;
        string _image_path = string.Empty;
        public PictureController(IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            _configuration = configuration;
            try
            {
                _image_path = _configuration["ImagePath"];
            }
            catch(Exception)
            { 
            }
            //if(string.IsNullOrEmpty(_image_path))
            {
                _image_path = Path.Combine(appEnvironment.WebRootPath, "images");
            }
        }

        // GET: api/<PictureController>/5
        [HttpGet("Raw/{path}")]
        public IActionResult Get(string path)
        {
            Byte[] b;
            
            b = System.IO.File.ReadAllBytes(@"C:\WORK\1.bmp");
            
            return File(b, "image/jpeg");
        }

        // GET api/<PictureController>/5
        [HttpGet("{path}")]
        public string GetAsBase64(string path)
        {
            return "value";
        }


        // PUT api/<PictureController>/5
        [HttpPut("{path}")]
        public void PutAsBase64(string path, [FromBody] string value)
        {
        }

        // DELETE api/<PictureController>/5
        [HttpDelete("{path}")]
        public void Delete(string path)
        {
        }
    }
}
