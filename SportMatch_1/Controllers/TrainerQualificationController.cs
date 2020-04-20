using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SportMatch_1.Models;

namespace SportMatch_1.Controllers
{
    public class TrainerQualificationController : ApiController
    {
        // GET: api/TrainerQualification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TrainerQualification/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TrainerQualification
        public int Post([FromBody]TrainerQualification [] tQ)
        {
            TrainerQualification t = new TrainerQualification();
            return t.insert(tQ);
        }
               // PUT: api/TrainerQualification/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TrainerQualification/5
        public void Delete(int id)
        {
        }
    }
}
