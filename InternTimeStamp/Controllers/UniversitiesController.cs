using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using InternTimeStamp.Models;
using Dapper;

namespace InternTimeStamp.Controllers
{
    public class UniversitiesController : Controller
    {
        SqlDataReader dr;
        private readonly IConfiguration configuration;
        public UniversitiesController(IConfiguration config)
        {
            this.configuration = config;
        }
        List<University> universities = new List<University>();


        // index
        public IActionResult Index()
        {
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            universities = connection.Query<University>("SELECT [Code],[UniversityName] FROM [University] ORDER BY [LastModifyDate] ASC").ToList();

            connection.Close();

            return View(universities);
        }


        // add
        [HttpPost]
        public ActionResult Index(University university)
        {
            List<University>? listUniversity = new List<University>();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);
            bool bIfExist = false;

            connection.Open();

            connection.Execute("INSERT INTO University([Code], [UniversityName], [LastModifyDate]) VALUES(@Code, @UniversityName, getdate())",
                new
                {
                    Code = university.Code,
                    UniversityName = university.UniversityName,
                    LastModifyDate = university.LastModifyDate.AddHours(+7)
                });

            listUniversity = connection.Query<University>("SELECT [Code],[UniversityName] FROM [University] ORDER BY [LastModifyDate] ASC").ToList();

            connection.Close();

            return View(listUniversity);
        }


        // edit
        [HttpPost]
        [Route("Universites/Edit/{code}")]
        public ActionResult Edit(University university)
        {
            var listUniversity = new List<University>();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();

            connection.Execute("UPDATE [University] SET [Code] = @Code ,[UniversityName] = @UniversityName ,[LastModifyDate] = getdate() WHERE Code=@Code",
                new
                {
                    Code = university.Code,
                    UniversityName = university.UniversityName,
                    LastModifyDate = university.LastModifyDate.AddHours(+7)
                });

            connection.Query<University>("SELECT [Code],[UniversityName] FROM [University] ORDER BY [LastModifyDate] ASC").ToList();

            connection.Close();

            return Ok("Successfully Edited!");
        }


        // getid
        [HttpGet]
        [Route("Universities/Get/{code}")]
        public IActionResult GetByCode(string code)
        {
            var result = new University();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand queryCmd = new SqlCommand("select [Code], [UniversityName] FROM [University] WHERE Code=@Code", connection);
            queryCmd.Connection = connection;
            queryCmd.Parameters.AddWithValue("@Code", code);
            dr = queryCmd.ExecuteReader();

            while (dr.Read())
            {
                result.Code = dr.GetString(0);
                result.UniversityName = dr.GetString(1);
            }

            connection.Close();

            return Ok(result);
        }


        // delete
        [HttpGet]
        [Route("Universities/Delete/{code}")]
        public IActionResult Delete(string code)
        {
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            connection.Execute("DELETE FROM [University] WHERE Code=@Code",
                new
                {
                    Code = code
                });

            connection.Close();

            return Ok("Successfully Deleted!");
        }

    }
}
