using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtcdNet;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new string[] { "value1", "value2" };
            var __options = new EtcdClientOpitions {
                Urls = new string[] { "http://etcd:2379" }
            };
            EtcdClient etcdClient = new EtcdClient(__options);
            await etcdClient.CreateNodeAsync("/key", "my value");
            var _value = await etcdClient.GetNodeValueAsync("/key");
            return Ok(_value);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
