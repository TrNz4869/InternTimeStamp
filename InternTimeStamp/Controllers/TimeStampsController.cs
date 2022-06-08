using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using InternTimeStamp.Models;
using Dapper;

namespace InternTimeStamp.Controllers
{
    public class TimeStampsController : Controller
    {
        SqlDataReader dr;

        private readonly IConfiguration configuration;

        public TimeStampsController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult Index()
        {
            var viewModel = new ViewModels.TimeStampList.IndexViewModels();
            viewModel.TimeStampLists = new List<TimeStamp>();
            viewModel.StudentLists = new List<InternStudent>();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            // query timestamp
            connection.Open();
            //connection.Query<TimeStamp>("SELECT [Name],[Time] FROM [TimeStamp] ORDER BY [LastModifyDate] ASC").ToList();

            SqlCommand queryTimeCmd = new SqlCommand("SELECT [Name],[Time] FROM [TimeStamp] ORDER BY [LastModifyDate] ASC", connection);
            dr = queryTimeCmd.ExecuteReader();
            while (dr.Read())
            {
                viewModel.TimeStampLists.Add(new TimeStamp() { Name = dr.GetString(0), Time = dr.GetDateTime(1) });
            }
            connection.Close();

            // query student
            connection.Open();
            //connection.Query<InternStudent>("SELECT [Name], [University_code] FROM [Intern] ORDER BY [LastModifyDate] ASC").ToList();

            SqlCommand queryStdCmd = new SqlCommand("SELECT [Name], [University_code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
            dr = queryStdCmd.ExecuteReader();

            while (dr.Read())
            {
                viewModel.StudentLists.Add(new InternStudent() { Name = dr.GetString(0) });
            }
            connection.Close();

            return View(viewModel);
        }
    }
}
