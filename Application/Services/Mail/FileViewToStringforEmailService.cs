using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;

namespace Application.Services.Mail
{
    public class FileViewToStringforEmailService : IFileViewToStringforEmailService
    {
        private readonly IWebHostEnvironment _env;

        public FileViewToStringforEmailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetHtmlFileContent(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_env.WebRootPath, "htmlfiles", fileName);
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
            }
            catch (Exception ex)
            {
                //ignore
            }
            return "";
        }
    }

}
