using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energistics.DataAccess.WITSML200;
using System.IO;
using SP_Witsml_v2._0.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace SP_Witsml_v2._0.Services
{
    public class LogService
    {
        private MemoryStream stream;
        private Log log;
        private string name;

        public LogService(Log lg, string nm)
        {
            log = this.QC(lg);
            stream = new MemoryStream();
            name = nm;
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, log);
        }

        public void SaveLogToDB()
        {
            var db = new ApplicationDbContext();
            LogObject logO = new LogObject();
            logO.Name = name;
            logO.SerializedObject = stream.ToArray();
            logO.Date = DateTime.Now;
            logO.ApplicationUserId = HttpContext.Current.User.Identity.GetUserId();
            db.LogObjects.Add(logO);
            db.SaveChanges();
        }

        public Log QC(Log log)
        {
            JArray array = JArray.Parse(log.ChannelSet.ElementAt(0).Data.Data);

            for(int i = 0; i < array.ElementAt(0).ElementAt(1).Count(); i++)
            {
                double lastDataValue = 0.0;
                foreach(JArray arr in array)
                {
                    if (arr.ElementAt(1).ElementAt(i).ToString().Equals("0"))
                    {
                        arr.ElementAt(1).ElementAt(i).Replace(lastDataValue);
                    }
                    else
                    {
                        lastDataValue = Double.Parse(arr.ElementAt(1).ElementAt(i).ToString());
                    }
                }
            }
            log.ChannelSet.ElementAt(0).Data.Data = array.ToString();
            return log;
        }
    }
}