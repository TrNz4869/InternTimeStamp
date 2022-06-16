using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using InternTimeStamp.Models;

namespace InternTimeStamp.Controllers
{
    public class InternStudentsController : Controller
    {
        SqlDataReader dr;

        private readonly IConfiguration configuration;

        public InternStudentsController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult Index()
        {
            var viewModel = new ViewModels.InternStudentList.IndexViewModels();
            viewModel.InternStudentLists = new List<InternStudent>();
            viewModel.UniversityLists = new List<University>();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            // query intern
            connection.Open();
            //connection.Query<InternStudent>("SELECT TOP (1000) [Name] ,[University_code] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

            SqlCommand queryInternCmd = new SqlCommand("SELECT [Name], [University_code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
            dr = queryInternCmd.ExecuteReader();
            while (dr.Read())
            {
                viewModel.InternStudentLists.Add(new InternStudent() { Name = dr.GetString(0), University = dr.GetString(1) });
            }
            connection.Close();

            // query university
            connection.Open();
            //connection.Query<University>("SELECT TOP(1000)[Code],[University] FROM[University] ORDER BY[LastModifyDate] ASC").ToList();

            SqlCommand queryUnivCmd = new SqlCommand("SELECT [UniversityName] FROM [University] ORDER BY [LastModifyDate] ASC", connection);
            dr = queryUnivCmd.ExecuteReader();

            while (dr.Read())
            {
                viewModel.UniversityLists.Add(new University() { UniversityName = dr.GetString(0) });
            }
            connection.Close();

            return View(viewModel);
        }

        // add
        [HttpPost]
        public ActionResult Index(InternStudent intern_student)
        {
            var listInternStudent = new List<InternStudent>();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            connection.Execute("INSERT INTO [Intern] ([Name], [University_code], [LastModifyDate]) VALUES (@Name, @University_code, getdate())",
                new
                {
                    Name = intern_student.Name,
                    University_code = intern_student.University,
                    LastModifyDate = intern_student.LastModifyDate.AddHours(+7)
                });

            //SqlCommand insertCmd = new SqlCommand("INSERT INTO Intern(Name, University_Code, LastModifyDate) VALUES(@Name, @University_Code, getdate())");
            //insertCmd.Connection = connection;
            //insertCmd.Parameters.AddWithValue("@Name", intern_list.Name);
            //insertCmd.Parameters.AddWithValue("@University_Code", intern_list.University_Code);
            //insertCmd.ExecuteNonQuery();

            listInternStudent = connection.Query<InternStudent>("SELECT [Name],[University_code] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

            //SqlCommand queryCmd = new SqlCommand("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
            //dr = queryCmd.ExecuteReader();

            //while (dr.Read())
            //{
            //    listIntern.Add(new Intern() { Name = dr.GetString(0), University_Code = dr.GetString(1) });
            //}

            connection.Close();

            return View(listInternStudent);
        }


        // getid
        [HttpGet]
        [Route("InternStudents/Get/{name}")]
        public IActionResult GetByName(string name)
        {
            var result = new InternStudent();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand queryCmd = new SqlCommand("select [Name] FROM [Intern] WHERE Name=@Name", connection);
            queryCmd.Connection = connection;
            queryCmd.Parameters.AddWithValue("@Name", name);
            dr = queryCmd.ExecuteReader();

            while (dr.Read())
            {
                result.Name = dr.GetString(0);
            }

            connection.Close();

            return Ok(result);
        }


        // delete
        [HttpGet]
        [Route("InternStudents/Delete/{name}")]
        public IActionResult delete(string name)
        {
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            connection.Execute("DELETE FROM [Intern] WHERE Name=@Name",
                new
                {
                    name = name
                });

            connection.Close();
            return Ok("successfully deleted!");
        }

        // edit
        [HttpPost]
        [Route("InternStudents/Edit/{name}")]
        public ActionResult Edit(InternStudent intern_student)
        {
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            connection.Execute("UPDATE [Intern] SET[Name] = @Name,[University_code] = @University_code, [LastModifyDate] = getdate() WHERE Name=@Name",
                new
                {
                    Name = intern_student.Name,
                    University_Code = intern_student.University,
                    LastModifyDate = intern_student.LastModifyDate
                });

            //SqlCommand editCommand = new SqlCommand("UPDATE [Intern] SET[Name] = @Name,[University_code] = @University_code, [LastModifyDate] = getdate() WHERE Name=@Name", connection);
            //editCommand.Parameters.AddWithValue("@Name", university.Code);
            //editCommand.Parameters.AddWithValue("@University_code", university.Universities);
            //editCommand.ExecuteNonQuery();

            connection.Query<InternStudent>("SELECT [Name],[University_แode] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

            //SqlCommand queryCmd = new SqlCommand("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
            //dr = queryCmd.ExecuteReader();

            //while (dr.Read())
            //{
            //    listUniversity.Add(new University() { Code = dr.GetString(0), Universities = dr.GetString(1) });
            //}

            connection.Close();

            return Ok("Successfully Edited!");
        }

    }
}
