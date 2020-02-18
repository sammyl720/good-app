using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace GoodApp.Backend.Helpers
{
    internal class AzureStorageHelper
    {
        private readonly CloudBlobClient _blobClient;
        private readonly IDictionary<FileUsage, CloudBlobContainer> _containers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageHelper"/> class.
        /// </summary>
        public AzureStorageHelper(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
            _containers = new Dictionary<FileUsage, CloudBlobContainer>();
        }

        /// <summary>
        /// Gets the cloud blob container.
        /// </summary>
        /// <param name="fileUsage">The file usage.</param>
        /// <returns>The cloud blob container.</returns>
        private async Task<CloudBlobContainer> GetContainer(FileUsage fileUsage)
        {
            if (!_containers.ContainsKey(fileUsage))
            {
                var container = _blobClient.GetContainerReference(fileUsage.ToString().ToLower());
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                _containers.Add(fileUsage, container);
            }

            return _containers[fileUsage];
        }

        /// <summary>
        /// Saves the file to cloud storage.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileUsage">The file usage.</param>
        /// <returns>
        /// The remote file url.
        /// </returns>
        public async Task<string> SaveFile(HttpPostedFileBase file, string fileName,
            FileUsage fileUsage = FileUsage.UserFiles)
        {
            var result = await SaveFileStream(file.InputStream, fileName, fileUsage);
            return result;
        }

        /// <summary>
        /// Saves the file stream to cloud storage.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileUsage">The file usage.</param>
        /// <returns>the remote file url</returns>
        public async Task<string> SaveFileStream(Stream fileStream, string fileName,
            FileUsage fileUsage = FileUsage.UserFiles)
        {
            var container = await GetContainer(fileUsage);
            var blockBlob = container.GetBlockBlobReference(fileName);
            fileStream.Seek(0, SeekOrigin.Begin);
            await blockBlob.UploadFromStreamAsync(fileStream);
            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task<bool> DeleteFile(string fileName, FileUsage fileUsage)
        {
            var container = await GetContainer(fileUsage);
            var blockBlob = container.GetBlockBlobReference(fileName);
            return await blockBlob.DeleteIfExistsAsync();
        }

        public enum FileUsage
        {
            UserFiles,
            UserPhotos
        }
    }
}
