using System;
using UkrMemesWeb.Models;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace UkrMemesWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersLoginController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        public UsersLoginController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpPut]
        public JsonResult Put(UserClass user)
        {
            string query = @"
                select * from myfirstdatabase.allusers";

            DataTable table = new DataTable();
            string sqlDataSource = _Configuration.GetConnectionString("PicturesAppConfig");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            string currentName = "somename";
            foreach (DataRow rows in table.Rows)
            {
                if (rows["login"].ToString() == user._UsersLogin)
                {
                    currentName = rows["username"].ToString();
                    break;
                }
            }
            return new JsonResult(currentName);
        }

        [HttpPost]
        public JsonResult Post([FromBody] UserClass user)
        {
            string query = @"
                select * from myfirstdatabase.allusers";

            DataTable table = new DataTable();
            string sqlDataSource = _Configuration.GetConnectionString("PicturesAppConfig");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            bool LoginIndex = false;

            foreach (DataRow rows in table.Rows)
            {
                if (rows["login"].ToString() == user._UsersLogin)
                {
                    LoginIndex = true;
                }
            }

            string response = "";
            bool IsAdmin = false;
            if (LoginIndex)
            {
                bool PassIndex = false;
                foreach (DataRow rows in table.Rows)
                {
                    if (rows["login"].ToString() == user._UsersLogin && rows["pass"].ToString() == user._UsersPass)
                    {
                        PassIndex = true;
                        if (rows["role"].ToString() == "admin")
                        {
                            IsAdmin = true;
                        }
                        break;
                    }
                }

                if (PassIndex)
                {
                    response = "1";//User logined successfully
                }
                else 
                {
                    response = "0";//Wrong password
                }
            }
            else
            {
                response = "2";//User with current login not exists
            }

            if (IsAdmin)
            {
                response += "1";//user is admin
            }
            else 
            {
                response += "0";//user is not admin
            }
            return new JsonResult(response);
        }
    }
}
