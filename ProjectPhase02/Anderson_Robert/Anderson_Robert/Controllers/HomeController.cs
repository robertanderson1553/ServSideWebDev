using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Anderson_Robert.Models;
using Microsoft.Extensions.Configuration;
using Anderson_Robert.DAL;

namespace Anderson_Robert.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page2(Person person)
        {
            // send it to the DB
            // do some validation

            //string connStr = configuration.GetConnectionString("MyConnStr");

            DALPerson dp = new DALPerson(this.configuration);
            string uID = dp.AddPerson(person);

            person.PersonID = uID;

            return View(person);
        }


    }
}
