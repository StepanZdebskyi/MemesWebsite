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
    public class PicturesController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        public PicturesController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select * from myfirstdatabase.allpictures";

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
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(PictureClass picture)
        {
            //////////////////////////////
            /*
            PictureClass picture = new PictureClass();
            picture._PictureTitle = "MC Петя Я живий!";
            picture._PictureDescription = "Мем про легендарного українського фріка Петра Моставчука";
            picture._PictureAddingDate = "2022-05-10T20:45";
            picture._PictureKeywordsString = "MC Petya, МС Петя, Мотиватор, Геній думки, Я живий";
            picture._PicureLink = "https://pbs.twimg.com/media/FN-rc2XXsAYWERL.jpg";
            //////////////////////////////
            ///*/
            string query = @"insert into myfirstdatabase.allpictures (keywords, title, description, link, addingdate) values ( @KeyWords, @Title, @Description, @Link, @AddingDate)";

            DataTable table = new DataTable();
            string sqlDataSource = _Configuration.GetConnectionString("PicturesAppConfig");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Title", picture._PictureTitle);
                    myCommand.Parameters.AddWithValue("@KeyWords", picture._PictureKeywordsString);
                    myCommand.Parameters.AddWithValue("@Description", picture._PictureDescription);
                    myCommand.Parameters.AddWithValue("@Link", picture._PicureLink);
                    myCommand.Parameters.AddWithValue("@AddingDate", picture._PictureAddingDate);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Added successfully!");
        }

        [HttpDelete]
        public JsonResult Delete(PictureClass picture)
        {
            //////////////////////////////
            /*
            PictureClass picture = new PictureClass();
            picture._PictureID = 2;
            //////////////////////////////*/
            string query = @"delete from myfirstdatabase.allpictures where id=@ID";

            DataTable table = new DataTable();
            string sqlDataSource = _Configuration.GetConnectionString("PicturesAppConfig");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@ID", picture._PictureID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Deleted successfully!");
        }


        [HttpPut]
        public JsonResult Put(PictureClass picture)
        {
            /*
            //////////////////////////////
            PictureClass picture = new PictureClass();
            picture._PictureTitle = "Sometitle";
            picture._PictureDescription = "SomeDescription3";
            picture._PictureAddingDate = "2022-05-10T20:45";
            picture._PictureKeywordsString = "Some, Words";
            picture._PicureLink = "Some another link";
            picture._PictureID = 6;
            //////////////////////////////*/
            string query = @"update myfirstdatabase.allpictures set keywords=@NewKeywords, title=@Title, description=@NewDescr, link=@NewLink, addingdate=@NewDate where id=@ID;";

            DataTable table = new DataTable();
            string sqlDataSource = _Configuration.GetConnectionString("PicturesAppConfig");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Title", picture._PictureTitle);
                    myCommand.Parameters.AddWithValue("@NewKeywords", picture._PictureKeywordsString);
                    myCommand.Parameters.AddWithValue("@NewDescr", picture._PictureDescription);
                    myCommand.Parameters.AddWithValue("@NewLink", picture._PicureLink);
                    myCommand.Parameters.AddWithValue("@NewDate", picture._PictureAddingDate);
                    myCommand.Parameters.AddWithValue("@ID", picture._PictureID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Updated successfully!");
        }
    }
}
