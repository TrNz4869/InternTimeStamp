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

            SqlCommand queryTimeCmd = new SqlCommand("SELECT [Name], [CheckinTime], [CheckoutTime], [Remark] FROM [TimeStamp] ORDER BY [LastModifyDate] ASC", connection);
            dr = queryTimeCmd.ExecuteReader();
            while (dr.Read())
            {
                viewModel.TimeStampLists.Add(new TimeStamp() { Name = dr.GetString(0), CheckinTime = dr.GetDateTime(1), CheckoutTime = dr.GetDateTime(2), Remark = dr.GetString(3) });
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

        // add
        [HttpPost]
        public ActionResult Index(TimeStamp timestamp)
        {
            var listTimeStamp = new List<TimeStamp>();
            string connectionstring = configuration.GetConnectionString("defaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            connection.Execute("INSERT INTO [TimeStamp] ([Name], [CheckinTime], [CheckoutTime], [Remark], [LastModifyDate]) VALUES (@Name, @CheckinTime, @CheckoutTime, @Remark, getdate())",
                new
                {
                    Name = timestamp.Name,
                    CheckinTime = timestamp.CheckinTime.AddHours(+7).ToString("yyyy-MM-ddTHH:mm:ss"),
                    CheckoutTime = timestamp.CheckoutTime.AddHours(+7).ToString("yyyy-MM-ddTHH:mm:ss"),
                    Remark = timestamp.Remark,
                    LastModifyDate = timestamp.LastModifyDate.AddHours(+7)
                });

            //SqlCommand insertCmd = new SqlCommand("INSERT INTO Intern(Name, University_Code, LastModifyDate) VALUES(@Name, @University_Code, getdate())");
            //insertCmd.Connection = connection;
            //insertCmd.Parameters.AddWithValue("@Name", intern_list.Name);
            //insertCmd.Parameters.AddWithValue("@University_Code", intern_list.University_Code);
            //insertCmd.ExecuteNonQuery();

            listTimeStamp = connection.Query<TimeStamp>("SELECT [Name], [CheckinTime], [CheckoutTime], [Remark] FROM [TimeStamp] ORDER BY [LastModifyDate] ASC").ToList();

            //SqlCommand queryCmd = new SqlCommand("SELECT TOP (1000) [Name],[University_Code] FROM [Intern] ORDER BY [LastModifyDate] ASC", connection);
            //dr = queryCmd.ExecuteReader();

            //while (dr.Read())
            //{
            //    listIntern.Add(new Intern() { Name = dr.GetString(0), University_Code = dr.GetString(1) });
            //}

            connection.Close();

            return View(listTimeStamp);
        }
    }
}
