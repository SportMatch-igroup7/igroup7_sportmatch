using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMatch_1.Models
{
    public class TrainerArea
    {
        int trainerCode;
        int areaCode;

        public int TrainerCode { get => trainerCode; set => trainerCode = value; }
        public int AreaCode { get => areaCode; set => areaCode = value; }

        public TrainerArea()
        { }
        public TrainerArea(int trainerCode, int acode)
        {
            this.TrainerCode = trainerCode;
            this.AreaCode = acode;

        }
        public int insert(TrainerArea[] area)
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.insertTrainerArea(area);
            return numAffected;
        }
    }
}