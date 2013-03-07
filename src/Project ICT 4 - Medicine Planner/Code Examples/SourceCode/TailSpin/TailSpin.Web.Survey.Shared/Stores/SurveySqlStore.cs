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
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Models;

    public class SurveySqlStore : ISurveySqlStore
    {
        public void SaveSurvey(string connectionString, SurveyData surveyData)
        {
            using (var dataContext = new SurveySqlDataContext(connectionString))
            {
                dataContext.SurveyDatas.InsertOnSubmit(surveyData);
                try
                {
                    dataContext.SubmitChanges();
                }
                catch (SqlException ex)
                {
                    Trace.TraceError(ex.TraceInformation());
                    throw;
                }
            }
        }

        public void Reset(string connectionString, string tenant, string slugName)
        {
            using (var dataContext = new SurveySqlDataContext(connectionString))
            {
                var query = from survey in dataContext.SurveyDatas
                            where survey.Id == string.Format(CultureInfo.InvariantCulture, "{0}_{1}", tenant, slugName)
                            select survey;

                foreach (var surveyData in query)
                {
                    dataContext.SurveyDatas.DeleteOnSubmit(surveyData);
                }

                try
                {
                    dataContext.SubmitChanges();
                }
                catch (SqlException ex)
                {
                    Trace.TraceError(ex.TraceInformation());
                    throw;
                }
            }
        }
    }
}