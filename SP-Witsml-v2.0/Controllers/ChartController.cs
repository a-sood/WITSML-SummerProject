using SP_Witsml_v2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using SP_Witsml_v2._0.Services;

namespace SP_Witsml_v2._0.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {

        [HttpGet]
        public ActionResult Plot(int id)
        {
            var db = new ApplicationDbContext();
            LogObject logObject = db.LogObjects.Where(l => l.Id == id).First();
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(logObject.SerializedObject);
            Log log = (Log)formatter.Deserialize(stream);

            ChannelSet cSet = log.ChannelSet.ElementAt(0);
            string[] IndexType = new string[cSet.Index.Count];
            for (int i = 0; i < cSet.Index.Count; i++)
            {
                IndexType[i] = cSet.Index.ElementAt(i).IndexType.ToString();
            }
            ViewBag.Id = id;
            ViewBag.IndexType = IndexType;
            string[] channels = new string[cSet.Channel.Count];
            for (int i = 0; i < cSet.Channel.Count; i++)
            {
                channels[i] = cSet.Channel.ElementAt(i).Aliases.ElementAt(0).Description;
            }
            ViewBag.Channels = channels;
           
            return View();
        }

        [HttpPost]
        public ActionResult Plot(FormCollection form)
        {
            var id = Convert.ToInt32(TempData["id"]);
            var rbSelected = form.GetValue("optradio").AttemptedValue;

            var db = new ApplicationDbContext();
            LogObject logObject = db.LogObjects.Where(l => l.Id == id).First();
            ViewBag.FileName = logObject.Name;
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(logObject.SerializedObject);
            Log log = (Log)formatter.Deserialize(stream);

            ChannelSet cSet = log.ChannelSet.ElementAt(0);
            string[] channelsDescription = new string[cSet.Channel.Count];
            for (int i = 0; i < cSet.Channel.Count; i++)
            {
                channelsDescription[i] = cSet.Channel.ElementAt(i).Aliases.ElementAt(0).Description;
            }

            JArray dataArray = JArray.Parse(cSet.Data.Data);
            int size = dataArray.Count;

            SelectedChannel cSelected = new SelectedChannel() { IndexValues = new List<string>(size), Data = new List<ChannelValue>()};

            int channelIndex = 0;
            foreach (var c in channelsDescription)
            {
                if (form[c] == null)
                {
                    channelIndex++;
                    continue;
                }

                if (form[c].Equals("on"))
                {
                    var temp = new ChannelValue() { Index = channelIndex, Mnemonic = cSet.Channel.ElementAt(channelIndex).Mnemonic.ToString(), Description = channelsDescription.ElementAt(channelIndex), LastDataValue = "0", DataValues = new List<string>(size)};

                    cSelected.Data.Add(temp);
                    channelIndex++;
                }
            }

            foreach (JArray arr in dataArray)
            {
                cSelected.IndexValues.Add(arr.ElementAt(0).ElementAt(0).ToString());
                foreach (var c in cSelected.Data)
                {
                    if (arr.ElementAt(1).ElementAt(c.Index).ToString().Equals("0"))
                    {
                        c.DataValues.Add(c.LastDataValue);
                        continue;
                    }
                    else
                    {
                        c.LastDataValue = arr.ElementAt(1).ElementAt(c.Index).ToString();
                        c.DataValues.Add(arr.ElementAt(1).ElementAt(c.Index).ToString());
                    }
                }
            }

            return View("LineChart", cSelected);
        }
    }
}