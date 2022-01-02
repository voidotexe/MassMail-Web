using Dapper;
using System.Data.SqlClient;
using System.Collections.Generic;
using MassMailWeb.Models;
using System;

namespace MassMailWeb.Helpers
{
    public static class DbHelper
    {
        public static List<EmailDb> emailsDb = new List<EmailDb>();
        public static int emailCounter;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MailHistory;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public static void Insert(string from, string to, string subject, string body, DateTime when)
        {
            /*
             * INSERT INTO Mails table
             * 
             * It's used insertParameters to avoid SQL Injection
             */

            string insertSql = "INSERT INTO Mails ([From], [To], [Subject], [Body], [When]) VALUES (@From, @To, @Subject, @Body, @When)";
            var insertParameters = new { From = from, To = to, Subject = subject, Body = body, When = when };

            using (SqlConnection db = GetConnection())
            {
                db.Execute(insertSql, insertParameters);
            }
        }

        public static void Get()
        {
            string getSql = "SELECT * FROM Mails";
            string getCounterSql = "SELECT COUNT(Id) FROM Mails";

            using (SqlConnection db = GetConnection())
            {
                foreach (var row in db.Query<EmailDb>(getSql))
                {
                    emailsDb.Add(row);
                }

                emailCounter = db.ExecuteScalar<int>(getCounterSql);
            }
        }

        public static void Delete()
        {
            string deleteSql = "TRUNCATE TABLE Mails";
        }
    }
}
