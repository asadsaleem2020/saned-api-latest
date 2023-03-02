using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.Accounts.Setup
{

    public class M_Level2
    {

     //   public int ID { get; set; }

        public string Company_Code { get; set; }

        public string Level1Code { get; set; }

        public string Level1Name { get; set; }
       
        public string Code { get; set; }

        public string Name { get; set; }
        public bool Locked { get; set; }
        public int Sort { get; set; }





    }
}
