//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.Stores
{
    public class MockSurveyStoreLocator : ISurveyStoreLocator
    {
        public ISurveyStore GetStoreReturnValue { get; set; }
        public ISurveyStore GetStore()
        {
            return GetStoreReturnValue;
        }
    }
}