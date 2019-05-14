using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KeyValueDatabase.libs;

namespace KeyValueDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataBase db = new DataBase();
        private Mailing mailing = new Mailing();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/value
        [HttpPost]
        public async void Post([FromBody] KeyValuePair<String, String> pairKeyValue)
        {
            db.setValue(pairKeyValue.Key, pairKeyValue.Value);
            await mailing.MakeNewsletterAsync(pairKeyValue);
        }

        public void POST([FromBody] String keyValue)
        {
            db.setValue(keyValue.Split(':')[0], keyValue.Split(':')[1]);
        }

        [HttpPost]
        public String Post([FromBody] string key)
        {
            return db.GetValue(key);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
