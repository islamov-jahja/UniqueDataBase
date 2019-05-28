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
        [HttpGet("/leader")]
        public string GetLeader()
        {
            return db._portOfLeader;
        }

        [HttpPost("/setLeader")]
        public void SetPortOfLeader([FromBody]string portOfLeader)
        {
            Console.WriteLine($"leader is set: {portOfLeader}");
            db._portOfLeader = portOfLeader;
            db.SendDataBase();

        }

        [HttpPost("/toVote")]
        public void ToVote([FromBody]string port)
        {
            Console.WriteLine("To Vote to:" + port);
            db.ToVote(port);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("/setValue")]
        public async Task<string> SetValue([FromBody] String pairKeyValue)
        {
            String[] values = pairKeyValue.Split(':');
            bool wasSaved = db.SetValue(values[0], values[1]);
            await mailing.MakeNewsletterAsync(pairKeyValue);
            return JsonConvert.SerializeObject(wasSaved);
        }

        [HttpPost("/setWithoutSend")]
        public void SetValueWithoutMakeNewsLetter([FromBody] String pairKeyValue)
        {
            String[] values = pairKeyValue.Split(':');
            db.SetValue(values[0], values[1]);
        }

        [HttpPost("/getDict")]
        public string GetDict()
        {
            return db.GetDict();
        }

        [HttpPost("/sendDict")]
        public void AddDataToDict([FromBody] String dictionaryWithData)
        {
            Dictionary<String, String> tempDict = JsonConvert.DeserializeObject<Dictionary<String, String>>(dictionaryWithData);
            db.UploadData(tempDict);
            //проверить работоспособность
        }

        [HttpPost("/getValue")]
        public string GetValue([FromBody]String key)
        {
            Console.WriteLine("au");
            Console.WriteLine(db.GetValue(key));
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
