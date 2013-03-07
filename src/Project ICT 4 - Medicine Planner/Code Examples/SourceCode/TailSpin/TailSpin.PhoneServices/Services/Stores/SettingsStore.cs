//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;
using TailSpin.Phone.Adapters;

namespace TailSpin.PhoneServices.Services.Stores
{
    public class SettingsStore : ISettingsStore
    {
        private readonly IProtectData protectDataAdapter;
        private const bool LocationServiceSettingDefault = false;
        private const string LocationServiceSettingKeyName = "LocationService";
        private const bool BackgroundTasksSettingDefault = false;
        private const string BackgroundTasksSettingKeyName = "BackgroundTasks";
        private const string PasswordSettingDefault = "";
        private const string PasswordSettingKeyName = "PasswordSetting";
        private const bool PushNotificationSettingDefault = false;
        private const string PushNotificationSettingKeyName = "PushNotification";
        private const string UserNameSettingDefault = "";
        private const string UserNameSettingKeyName = "UserNameSetting";
        private readonly IsolatedStorageSettings isolatedStore;
        private UTF8Encoding encoding;

        public SettingsStore(IProtectData protectDataAdapter)
        {
            this.protectDataAdapter = protectDataAdapter;
            isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            encoding = new UTF8Encoding();
        }

        public string Password
        {
            get
            {
                return PasswordByteArray.Length == 
                    0 ? PasswordSettingDefault : encoding.GetString(PasswordByteArray, 0, PasswordByteArray.Length);
            }
            set
            {
                PasswordByteArray = encoding.GetBytes(value);
            }
        }

        private byte[] PasswordByteArray
        {
            get
            {
                byte[] encryptedValue = GetValueOrDefault(PasswordSettingKeyName, new byte[0]);
                if (encryptedValue.Length == 0)
                    return new byte[0];
                return protectDataAdapter.Unprotect(encryptedValue, null);
            }
            set
            {
                byte[] encryptedValue = protectDataAdapter.Protect(value, null);
                AddOrUpdateValue(PasswordSettingKeyName, encryptedValue);
            }
        }

        public bool SubscribeToPushNotifications
        {
            get { return GetValueOrDefault(PushNotificationSettingKeyName, PushNotificationSettingDefault); }
            set { AddOrUpdateValue(PushNotificationSettingKeyName, value); }
        }

        public bool LocationServiceAllowed
        {
            get { return GetValueOrDefault(LocationServiceSettingKeyName, LocationServiceSettingDefault); }
            set { AddOrUpdateValue(LocationServiceSettingKeyName, value); }
        }

        public bool BackgroundTasksAllowed
        {
            get { return GetValueOrDefault(BackgroundTasksSettingKeyName, BackgroundTasksSettingDefault); }
            set { AddOrUpdateValue(BackgroundTasksSettingKeyName, value); }
        }

        public event EventHandler UserChanged;

        public string UserName
        {
            get { return this.GetValueOrDefault(UserNameSettingKeyName, UserNameSettingDefault); }
            set 
            { 
                this.AddOrUpdateValue(UserNameSettingKeyName, value);
                var handler = this.UserChanged;
                if(handler != null)
                {
                    UserChanged(this, null);
                }
            }
        }

        private void AddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;

            try
            {
                // if new value is different, set the new value.
                if (isolatedStore[key] != value)
                {
                    isolatedStore[key] = value;
                    valueChanged = true;
                }
            }
            catch (KeyNotFoundException)
            {
                isolatedStore.Add(key, value);
                valueChanged = true;
            }
            catch (ArgumentException)
            {
                isolatedStore.Add(key, value);
                valueChanged = true;
            }

            if (valueChanged)
            {
                Save();
            }
        }

        private T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            try
            {
                value = (T)isolatedStore[key];
            }
            catch (KeyNotFoundException)
            {
                value = defaultValue;
            }
            catch (ArgumentException)
            {
                value = defaultValue;
            }

            return value;
        }

        private void Save()
        {
            isolatedStore.Save();
        }
    }
}
