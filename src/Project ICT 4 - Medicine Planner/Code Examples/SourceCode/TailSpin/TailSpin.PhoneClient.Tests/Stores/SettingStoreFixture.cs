//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.IO.IsolatedStorage;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.Stores
{
    [TestClass]
    public class SettingStoreFixture
    {
        [TestMethod]
        public void SettingPasswordUsesEncryptionServiceAndPersistsIntoIsoStore()
        {
            var encoder = new UTF8Encoding();
            var mockProtectDataAdapter = new MockProtectDataAdapter();
            mockProtectDataAdapter.ProtectTestCallback =
                (userData, optionalEntropy) =>
                {
                    var stringUserData = encoder.GetString(userData, 0, userData.Length);
                    return encoder.GetBytes(string.Format("ENCRYPTED: {0}", stringUserData));
                };
            var target = new SettingsStore(mockProtectDataAdapter);
            IsolatedStorageSettings.ApplicationSettings["PasswordSetting"] = null;

            target.Password = "testpassword";

            var encryptedByteArray = (byte[])IsolatedStorageSettings.ApplicationSettings["PasswordSetting"];
            Assert.AreEqual("ENCRYPTED: testpassword", encoder.GetString(encryptedByteArray, 0, encryptedByteArray.Length));
        }

        [TestMethod]
        public void GettingPasswordUsesEncryptionServiceAndIsoStore()
        {
            var encoder = new UTF8Encoding();
            var mockProtectDataAdapter = new MockProtectDataAdapter();
            mockProtectDataAdapter.UnProtectTestCallback = 
                (encryptedData, optionalEntropy) =>
                {
                    var stringEncryptedData = encoder.GetString(encryptedData, 0, encryptedData.Length);
                    return encoder.GetBytes(stringEncryptedData.Substring(11));
                };
            var target = new SettingsStore(mockProtectDataAdapter);
            IsolatedStorageSettings.ApplicationSettings["PasswordSetting"] = encoder.GetBytes("ENCRYPTED: testpassword");

            Assert.AreEqual("testpassword", target.Password);
        }
    }
}
