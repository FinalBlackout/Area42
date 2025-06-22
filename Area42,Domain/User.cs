using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area42.Domain.Entities
{ 
public class User
    {
        //Niet de meest logische maar dit komt overeen met de gedeelde database. Van niet alles maak ik gebruik maar andere domainen wel. 
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public int telephone { get; set; }
        public string job { get; set; }
        public string status { get; set; }
    }
}