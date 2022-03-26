using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICET_BKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_ValuesController : ControllerBase
    {


        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "Value 1", "Value 2", "Value 3" };

        }


        [HttpGet("{id}")]
        public string Get(int id)
        {

            return "The value is " + id;
        }



    }
}
