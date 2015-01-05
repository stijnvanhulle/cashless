﻿using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class DALogs
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<Log> Load(IEnumerable<Claim> claims)
        {
            List<Log> lijst = new List<Log>();
            string sql = "SELECT * FROM tblLogs";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    lijst.Add(Create(data));

                }
                data.NextResult();
            }

            return lijst;

        }


        public static int Insert(Log log, IEnumerable<Claim> claims)
        {
            int id;
            DateTime time = TimeConverter.Controle(log);



            string sql = "INSERT INTO tblLogs VALUES(@RegisterID,@Timestamp,@Message,@StackTrace)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "RegisterID", log.RegisterID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Timestamp", time);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Message", log.Message);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "StackTrace", log.Stacktrace);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3,par4);

            return id;
        }


        private static Log Create(IDataRecord data)
        {

            return new Log()
            {
                RegisterID= (int)data["RegisterID"],
                Message= data["Message"].ToString(),
                Stacktrace=data["Stacktrace"].ToString(),
                Timestamp=(long) TimeConverter.ToUnixTimeStamp((DateTime) data["Timestamp"])

            };
        }


    }
}