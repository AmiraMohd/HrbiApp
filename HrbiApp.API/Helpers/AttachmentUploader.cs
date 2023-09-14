using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

using HrbiApp.API.Helpers;
using DBContext;

namespace Tzwed.API.Helpers
{
    public class AttachmentUploader
    {
        IConfiguration _configuration;
        ExceptionHandler _ex;
        ApplicationDBContext _db;
        string applicationPath = "";
        public AttachmentUploader(IConfiguration configuration, ApplicationDBContext db)
        {
            _configuration = configuration;
            _ex = new ExceptionHandler(db);
            _db = db;
            applicationPath = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
            //applicationPath = Directory.GetCurrentDirectory();
        }
        public string GetSettingValue(string name)
        {
            try
            {
                var setting = _db.Settings.FirstOrDefault(s => s.Name == name);
                return setting.Value;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }

        public async Task<(bool Result,string Message)> UploadProfileImage(IFormFile file)
        {
            try
            {
                List<string> allowedExtentions = new List<string>() { ".png", ".jpg", ".jepg" };
                var imagesPath = GetSettingValue(Consts.ProfileImagesPathUploadSetting);
              
                _db.SaveChanges();
                return await UploadFileToPath(file, imagesPath, allowedExtentions);
               
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,ex.Message);
            }
        }
        
        private async Task<(bool Result,string Message)> UploadFileToPath(IFormFile file, string path, List<string> allowedExtensions)
        {
            try
            {
                var fileName = "";
                 
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    return (false,Messages.NotAllowedExtension);
                }
                fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(applicationPath,path,fileName);
                using (var stream = new FileStream(path, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }   
                _db.SaveChanges();
                return (true,fileName);
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,ex.Message);
            }
        }

    }
   
}
