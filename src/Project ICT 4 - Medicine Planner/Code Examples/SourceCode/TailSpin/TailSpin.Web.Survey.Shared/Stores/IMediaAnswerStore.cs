//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Stores
{
    using System.IO;
    using TailSpin.Web.Survey.Shared.Models;

    public interface IMediaAnswerStore
    {
        void Initialize();
        string SaveMediaAnswer(Stream media, QuestionType questionType);
        void DeleteMediaAnswer(string id, QuestionType questionType);
    }
}