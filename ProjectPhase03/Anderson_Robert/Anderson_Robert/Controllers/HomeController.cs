using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Anderson_Robert.Models;
using Microsoft.Extensions.Configuration;
using Anderson_Robert.DAL;
using Microsoft.AspNetCore.Http;

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
            DALPerson dp = new DALPerson(this.configuration);
            string uID = dp.AddPerson(person);

            person.PersonID = uID;

            //Save the UID to the session
            HttpContext.Session.SetString("uID", uID.ToString());  //Write to the session

            string strUID = HttpContext.Session.GetString("uID");  //Read from the session

            return View(person);
        }

        public IActionResult EditPerson()
        {
            //Get the uID from the session
            int uID = Convert.ToInt32(HttpContext.Session.GetString("uID"));  //Read from the session

            //Get the person object from the DB using the DalPerson class
            DALPerson dp = new DALPerson(configuration);
            Person person = dp.getPerson(uID);

            //Send it to the view
            return View(person);
        }

        public IActionResult UpdatePerson(Person person)
        {
            // get the uid from the session
            string uID = HttpContext.Session.GetString("uID"); // reads from the session
            person.PersonID = uID;

            DALPerson dp = new DALPerson(configuration);
            dp.UpdateUser(person);

            return View("Page2", person);
        }
    }
}
