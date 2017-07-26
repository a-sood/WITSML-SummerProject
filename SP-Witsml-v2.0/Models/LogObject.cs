using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SP_Witsml_v2._0.Models
{
    public class LogObject
    {
        public int Id { get; set; }
        [Display(Name="File Name")]
        public string Name { get; set; }
        public byte[] SerializedObject { get; set; }

        [Display(Name="Upload Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMMM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string ApplicationUserId { get; set; }
    }
}