using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.IO;
using System.Threading.Tasks;
using PasqualeSite.Data.Entities;
using System.Data.Entity;

namespace PasqualeSite.Services
{
    public class ImageService : DisposableService
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer container;

        public ImageService()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("images");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        public async Task<string> UploadImage(string imageName, HttpPostedFileBase photoToUpload)
        {         
            string fullPath = null;
            imageName = imageName.ToLower();
            try
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                // Upload image to Blob Storage
                blockBlob.Properties.ContentType = photoToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(photoToUpload.InputStream);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                var uriBuilder = new UriBuilder(blockBlob.Uri);
                uriBuilder.Scheme = "http";
                fullPath = uriBuilder.ToString();

                await SaveImageToDb(imageName);
                return fullPath;
            }

            catch (Exception ex)
            {
                // LOG IT
                return fullPath;
            }          
        }

        public async Task<List<PostImage>> GetImages()
        {
            var images = await db.PostImages.ToListAsync();
            return images;
        }

        public async Task<PostImage> SaveImageToDb(string imageName)
        {
            var image = await db.PostImages.Where(x => x.Name == imageName).FirstOrDefaultAsync();

            if (image != null)
            {
                var newImage = image;
                newImage.Name = imageName;
                newImage.Path = "http://www.thesqualls.com/images/" + imageName;
                db.Entry(image).CurrentValues.SetValues(newImage);
                await db.SaveChangesAsync();
                return newImage;
            }
            else
            {
                var newImage = new PostImage();
                newImage.Name = imageName;
                newImage.Path = "http://www.thesqualls.com/images/" + imageName;
                db.PostImages.Add(newImage);
                await db.SaveChangesAsync();
                return newImage;
            }        
        }
    }
}