//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class FilesBlobContainer : IAzureBlobContainer<byte[]>
    {
        private readonly CloudStorageAccount account;
        private readonly CloudBlobContainer container;
        private readonly string contentType;

        public FilesBlobContainer(CloudStorageAccount account, string containerName, string contentType)
        {
            this.account = account;
            this.contentType = contentType;

            var client = this.account.CreateCloudBlobClient();
            client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

            this.container = client.GetContainerReference(containerName);
        }

        public void EnsureExist()
        {
            this.container.CreateIfNotExist();
            this.container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        public void Save(string objId, byte[] obj)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            blob.Properties.ContentType = this.contentType;
            blob.UploadByteArray(obj);
        }

        byte[] IAzureBlobContainer<byte[]>.Get(string objId)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            try
            {
                return blob.DownloadByteArray();
            }
            catch (StorageClientException)
            {
                return null;
            }
        }

        public IEnumerable<byte[]> GetAll()
        {
            IEnumerable<IListBlobItem> blobItems = this.container.ListBlobs();

            return blobItems.Select(blobItem => ((IAzureBlobContainer<byte[]>)this).Get(blobItem.Uri.ToString())).ToList();
        }

        public Uri GetUri(string objId)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            return blob.Uri;
        }

        public void Delete(string objId)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            blob.DeleteIfExists();
        }

        public void DeleteContainer()
        {
            this.container.Delete();
        }
    }
}