﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreInfrastructure.GeneralSetting.M_BarCodeType
{
   public class M_BarCodeType
    {
       
        public string COMPANY_CODE { get; set; } 
        public string Code { get; set; } 
        public string Name { get; set; }
        public bool Locked { get; set; }
        public int Sort { get; set; }
      
    }
}