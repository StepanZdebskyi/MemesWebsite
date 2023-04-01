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
    public class UsersRegisterController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        public UsersRegisterController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(UserClass user)
        {
            user._UsersRole = "commonuser";
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
            foreach (DataRow rows in table.Rows)
            {
                if (rows["username"].ToString() == user._UsersName)
                {
                    return new JsonResult("2");//Account with this username is already exists!
                }
                if (rows["login"].ToString() == user._UsersLogin)
                {
                    return new JsonResult("0");//Account with this login is already exists!
                }
            }

            query = @"insert into myfirstdatabase.allusers (role, username, login, pass) values ( @Role, @Username, @Login, @Pass)";

            table = new DataTable();
            sqlDataSource = _Configuration.GetConnectionString("PicturesAppConfig");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Role", "commonuser");
                    myCommand.Parameters.AddWithValue("@Username", user._UsersName);
                    myCommand.Parameters.AddWithValue("@Login", user._UsersLogin);
                    myCommand.Parameters.AddWithValue("@Pass", user._UsersPass);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("1");//New account registered successfully!
        }
    }
}
