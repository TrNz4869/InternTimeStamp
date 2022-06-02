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
            //connection.Query<Intern>("SELECT TOP (1000) [Name] ,[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

            SqlCommand queryInternCmd = new SqlCommand("SELECT [Name] ,[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
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
        //[HttpPost]
        //public ActionResult Index(Intern intern_list)
        //{
        //    var listIntern = new List<Intern>();
        //    string connectionstring = configuration.GetConnectionString("defaultConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionstring);

        //    connection.Open();
        //    connection.Execute("INSERT INTO Intern(Name, University_Code, LastModifyDate) VALUES(@Name, @University_Code, getdate())",
        //        new
        //        {
        //            Name = intern_list.Name,
        //            University_Code = intern_list.University_Code,
        //            LastModifyDate = intern_list.LastModifyDate
        //        });

        //    //SqlCommand insertCmd = new SqlCommand("INSERT INTO Intern(Name, University_Code, LastModifyDate) VALUES(@Name, @University_Code, getdate())");
        //    //insertCmd.Connection = connection;
        //    //insertCmd.Parameters.AddWithValue("@Name", intern_list.Name);
        //    //insertCmd.Parameters.AddWithValue("@University_Code", intern_list.University_Code);
        //    //insertCmd.ExecuteNonQuery();

        //    listIntern = connection.Query<Intern>("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

        //    //SqlCommand queryCmd = new SqlCommand("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
        //    //dr = queryCmd.ExecuteReader();

        //    //while (dr.Read())
        //    //{
        //    //    listIntern.Add(new Intern() { Name = dr.GetString(0), University_Code = dr.GetString(1) });
        //    //}

        //    connection.Close();

        //    return View(listIntern);
        //}


        ////// delete
        //[HttpGet]
        //[Route("InternList/Delete/{name}")]
        //public IActionResult Delete(string name)
        //{
        //    string connectionstring = configuration.GetConnectionString("defaultConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionstring);

        //    connection.Open();
        //    connection.Execute("DELETE FROM [Intern] WHERE Name=@Name",
        //        new
        //        {
        //            Name = name
        //        });

        //    //SqlCommand delCommand = new SqlCommand("DELETE FROM [Intern] WHERE Name=@Name", connection);
        //    //delCommand.Connection = connection;
        //    //delCommand.Parameters.AddWithValue("@Name", name);
        //    //delCommand.ExecuteNonQuery();
        //    connection.Close();
        //    return Ok("Successfully Deleted!");
        //}

        //// edit
        //[HttpPost]
        //public ActionResult Edit(Intern intern_list)
        //{
        //    string connectionstring = configuration.GetConnectionString("defaultConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionstring);

        //    connection.Open();
        //    connection.Execute("UPDATE [Intern] SET[Name] = @Name,[University_code] = @University_code, [LastModifyDate] = getdate() WHERE Name=@Name",
        //        new
        //        {
        //            Name = intern_list.Name,
        //            University_Code = intern_list.University_Code,
        //            LastModifyDate = intern_list.LastModifyDate
        //        });

        //    //SqlCommand editCommand = new SqlCommand("UPDATE [Intern] SET[Name] = @Name,[University_code] = @University_code, [LastModifyDate] = getdate() WHERE Name=@Name", connection);
        //    //editCommand.Parameters.AddWithValue("@Name", university.Code);
        //    //editCommand.Parameters.AddWithValue("@University_code", university.Universities);
        //    //editCommand.ExecuteNonQuery();

        //    connection.Query<Intern>("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

        //    //SqlCommand queryCmd = new SqlCommand("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
        //    //dr = queryCmd.ExecuteReader();

        //    //while (dr.Read())
        //    //{
        //    //    listUniversity.Add(new University() { Code = dr.GetString(0), Universities = dr.GetString(1) });
        //    //}

        //    connection.Close();

        //    return Ok("Successfully Edited!");
        //}

    }
}
