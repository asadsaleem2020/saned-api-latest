﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.GeneralSetting.WorkStatus
{
    public class M_WorkStatus
    {
        public string COMPANY_CODE { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }
        public int Sort { get; set; }
    }
}
