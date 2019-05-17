using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KeyValueDatabase.libs;
using System.Net.Http;
using Newtonsoft.Json;

namespace KeyValueDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataBase db = DataBase.GetInstance();
        private Mailing mailing = Mailing.GetInstance();

        // GET api/values
        [HttpGet]
        public void Get()
        {
            db.ToSaveData();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("/setValue")]
        public async void SetValue([FromBody] String pairKeyValue)
        {
            String[] values = pairKeyValue.Split(':');
            db.setValue(values[0], values[1]);
            await mailing.MakeNewsletterAsync(pairKeyValue);
        }

        [HttpPost("/setWithoutSend")]
        public void SetValueWithoutMakeNewsLetter([FromBody] String pairKeyValue)
        {
            String[] values = pairKeyValue.Split(':');
            db.setValue(values[0], values[1]);
        }

        [HttpPost("/getValue")]
        public string GetValue([FromBody]String key)
        {
            return  db.GetValue(key);
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
