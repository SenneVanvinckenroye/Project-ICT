﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Phone.Reactive;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services.Clients;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.Services.Surveys.Surveys;

namespace TailSpin.PhoneServices.Services.SurveysService
{
    public class SurveysServiceClient : ISurveysServiceClient
    {
        private readonly Uri serviceUri;
        private readonly ISettingsStore settingsStore;
        private readonly IHttpClient httpClient;

        public SurveysServiceClient(Uri serviceUri, ISettingsStore settingsStore, IHttpClient httpClient)
        {
            this.serviceUri = serviceUri;
            this.settingsStore = settingsStore;
            this.httpClient = httpClient;
        }

        public IObservable<IEnumerable<SurveyTemplate>> GetNewSurveys(string lastSyncDate)
        {
            var surveysPath = string.Format(CultureInfo.InvariantCulture, "Surveys?lastSyncUtcDate={0}", lastSyncDate);
            var uri = new Uri(serviceUri, surveysPath);

            return
                httpClient
                    .GetJson<IEnumerable<SurveyDto>>(new HttpWebRequestAdapter(uri), settingsStore.UserName, settingsStore.Password)
                    .Select(ToSurveyTemplate);
        }

        public IObservable<Unit> SaveSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
            var surveyAnswersDto = ToSurveyAnswersDto(surveyAnswers);

            var saveAndUpdateMediaAnswersObservable = SaveAndUpdateMediaAnswers(surveyAnswersDto);

            var uri = new Uri(serviceUri, "SurveyAnswers");
            var saveSurveyAnswerObservable =
                httpClient
                    .PostJson(new HttpWebRequestAdapter(uri), settingsStore.UserName, settingsStore.Password, surveyAnswersDto);

            return saveAndUpdateMediaAnswersObservable.SelectMany(u => saveSurveyAnswerObservable);
        }

        private static IEnumerable<SurveyTemplate> ToSurveyTemplate(IEnumerable<SurveyDto> surveyEntities)
        {
            return surveyEntities.Select(
                s => new SurveyTemplate
                {
                    Title = s.Title,
                    SlugName = s.SlugName,
                    Tenant = s.Tenant,
                    IconUrl = s.IconUrl,
                    Length = s.Length,
                    CreatedOn = s.CreatedOn,
                    Questions = s.Questions.Select(
                        q => new Question
                        {
                            Text = q.Text,
                            Type = QuestionTypeParser.Parse(q.Type),
                            PossibleAnswers =
                                q.PossibleAnswers == null ? new List<string>() : q.PossibleAnswers.Split('\n').ToList()
                        }).ToList()
                }).ToList();
        }

        private static IEnumerable<SurveyAnswerDto> ToSurveyAnswersDto(IEnumerable<SurveyAnswer> surveyAnswers)
        {
            return surveyAnswers.Select(
                a => new SurveyAnswerDto
                {
                    Title = a.Title,
                    SlugName = a.SlugName,
                    Tenant = a.Tenant,
                    StartLocation = GetLocationAsString(a.StartLocation),
                    CompleteLocation = GetLocationAsString(a.CompleteLocation),
                    QuestionAnswers = a.Answers.Select(
                        qa => new QuestionAnswerDto
                        {
                            Answer = qa.Value,
                            QuestionText = qa.QuestionText,
                            QuestionType = Enum.GetName(typeof(QuestionType), qa.QuestionType),
                            PossibleAnswers = GetPossibleAnswersAsString(qa.PossibleAnswers)
                        }).ToArray()
                }).ToList();
        }

        private static string GetLocationAsString(GeoCoordinate location)
        {
            if (location == null || location.IsUnknown)
            {
                return string.Empty;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0},{1}", location.Latitude, location.Longitude);
        }

        private static string GetPossibleAnswersAsString(List<string> possibleAnswers)
        {
            var builder = new StringBuilder();
            possibleAnswers.ForEach(s => builder.Append(s).Append('\n'));
            return builder.ToString();
        }

        private static byte[] GetFile(string filename)
        {
            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = new IsolatedStorageFileStream(filename, FileMode.Open, filesystem))
                {
                    var fileBytes = new byte[fileStream.Length];
                    fileStream.Read(fileBytes, 0, Convert.ToInt32(fileStream.Length));
                    return fileBytes;
                }
            }
        }

        private IObservable<Unit> SaveAndUpdateMediaAnswers(IEnumerable<SurveyAnswerDto> surveyAnswersDto)
        {
            var mediaAnswers =
                from surveyAnswer in surveyAnswersDto
                from answer in surveyAnswer.QuestionAnswers
                where
                    answer.Answer != null &&
                    (answer.QuestionType == Enum.GetName(typeof(QuestionType), QuestionType.Picture) ||
                     answer.QuestionType == Enum.GetName(typeof(QuestionType), QuestionType.Voice))
                select answer;

            var mediaAnswerObservables = new List<IObservable<Unit>>();
            foreach (var answer in mediaAnswers)
            {
                var mediaAnswerPath = string.Format(
                    CultureInfo.InvariantCulture,
                    "MediaAnswer?type={0}",
                    answer.QuestionType);
                var mediaAnswerUri = new Uri(serviceUri, mediaAnswerPath);
                byte[] mediaFile = GetFile(answer.Answer);

                var request = httpClient.GetRequest(new HttpWebRequestAdapter(mediaAnswerUri), settingsStore.UserName, settingsStore.Password);
                request.Method = "POST";
                request.Accept = "application/json";
                QuestionAnswerDto answerCopy = answer;
                var saveFileAndUpdateAnswerObservable =
                    Observable
                        .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                        .SelectMany(
                            requestStream =>
                            {
                                using (requestStream)
                                {
                                    requestStream.Write(mediaFile, 0, mediaFile.Length);
                                    requestStream.Close();
                                }

                                return
                                    Observable.FromAsyncPattern<WebResponse>(
                                        request.BeginGetResponse,
                                        request.EndGetResponse)();
                            },
                            (requestStream, webResponse) =>
                            {
                                using (var responseStream = webResponse.GetResponseStream())
                                {
                                    var responseSerializer = new DataContractJsonSerializer(typeof(string));
                                    answerCopy.Answer = (string)responseSerializer.ReadObject(responseStream);
                                }

                                return new Unit();
                            });

                mediaAnswerObservables.Add(saveFileAndUpdateAnswerObservable);
            }

            return mediaAnswerObservables.ForkJoin().Select(u => new Unit());
        }
    }
}
