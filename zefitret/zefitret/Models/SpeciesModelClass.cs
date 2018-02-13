using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using zefitret.Models;

namespace zefitret.Models
{
    public class SpeciesModelClass
    {
        [Key]
        public int MId { get; set; }
        public string MCategory { get; set; }
        public string MSName { get; set; }
        public string MLocalName { get; set; }
        public string MCommonName { get; set; }
        public string MDescription { get; set; }
        public string MPicTakenBy { get; set; }
        public string MContentBy { get; set; }
        public string MEditedBy { get; set; }

        /// <summary>
        ///  these 4 lines of byte[] below are for index view 
        /// </summary>
        public byte[] MMainPic { get; set; }
        public byte[] MSecPic { get; set; }
        public byte[] MThirdPic { get; set; }
        public byte[] MFourthPic { get; set; }

        public HttpPostedFileBase MainImg { set; get; }
        public HttpPostedFileBase SecondImg { set; get; }
        public HttpPostedFileBase ThirdImg { set; get; }
        public HttpPostedFileBase FourthImg { set; get; }

        public string MainSrc { set; get; }
        public string SecondSrc { set; get; }
        public string ThirdSrc { set; get; }
        public string FourthSrc { set; get; }
    }
}