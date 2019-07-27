using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace SbAdmintrial.Models
{
    public class meterial
    {
        public int idmaterial { get; set; }
        [DisplayName("File Name")]
        public string materialname { get; set; }
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }
        public HttpPostedFileBase imagefile { get; set; }
    }
}