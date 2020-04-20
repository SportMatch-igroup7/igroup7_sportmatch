using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMatch_1.Models
{
    public class TrainerLanguage
    {
        int lCode;
        int trainerCode;

        public int LCode { get => lCode; set => lCode = value; }
        public int TrainerCode { get => trainerCode; set => trainerCode = value; }

        public TrainerLanguage()
        { }
        public TrainerLanguage(int lCode, int trainerCode)
        {
            this.LCode = lCode;
            this.TrainerCode = trainerCode;
        }
        public int insert(TrainerLanguage[] lan)
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.insertTrainerLanguage(lan);
            return numAffected;
        }
    }
}