using Matrip.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Matrip.Web.Libraries.Archive
{
    public class PhotoManager
    {
        public static async Task AddTripImage(IFormFile file, ma13tripphoto tripphoto)
        {
            string imagePath = "/images/tripphotos/";

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var FileName = tripphoto.FK1305idtrip + "-" + tripphoto.ma13photoquantity + ".jpg";
            var FullPath = uploadPath + FileName;
            imagePath = imagePath + @"\";


            using (var fileStream = new FileStream(FullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var filePath = @".." + Path.Combine(imagePath, FileName);
        }


        public static bool ExcludeTripImage(string imagePath, int photoquantity, int position, int tripID)
        {
            string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
            if (File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
                if(photoquantity > position)
                {
                    for (int i = position + 1; i <= photoquantity; i++)
                    {
                        string[] imagePathSplit = imagePath.Split("-");
                        imagePathSplit[1] = string.Format("{0}.jpg", i);
                        imagePath = imagePathSplit[0] +"-"+ imagePathSplit[1];
                        var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

                        imagePathSplit[1] = string.Format("{0}.jpg", i - 1);
                        imagePath = imagePathSplit[0] + "-" + imagePathSplit[1];
                        var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

                        File.Move(sourcePath, destinationPath);
                        File.Delete(sourcePath);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task UploadHeaderCityPhoto(IFormFile file, int PhotoID, int CityID)
        {
            string imagePath = "/images/cityphotos/headers/"+ PhotoID+"-"+CityID+"-filter.jpg";
            string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
            using (var fileStream = new FileStream(FullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}
