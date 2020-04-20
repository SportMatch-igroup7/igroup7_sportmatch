using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMatch_1.Models
{
    public class TrainerQualification
    {
        int trainerCode;
        int qualificationTypeCode;
        string fromDate;
        int populationCode;
        string documentPath;

        public int TrainerCode { get => trainerCode; set => trainerCode = value; }
        public int QualificationTypeCode { get => qualificationTypeCode; set => qualificationTypeCode = value; }
        public string FromDate { get => fromDate; set => fromDate = value; }
        public int PopulationCode { get => populationCode; set => populationCode = value; }
        public string DocumentPath { get => documentPath; set => documentPath = value; }

        public TrainerQualification()
        { }

        public TrainerQualification(int trainerCode, int qualificationTypeCode, string fromDate, int populationCode, string doc)
        {
            this.TrainerCode = trainerCode;
            this.QualificationTypeCode = qualificationTypeCode;
            this.FromDate = fromDate;
            this.PopulationCode = populationCode;
            this.DocumentPath = doc;
        }
        public int insert(TrainerQualification [] tQ)
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.insertTrainerQualification(tQ);
            return numAffected;
        }
      

    }
}