using JOBProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JOBProject.Controllers
{
    public class AdminController : Controller
    {
        public List<AdminModel> jobList = new List<AdminModel>();

        public string jobId = "";
        public IActionResult Admin()
        {
            try
            {
                string connectionString = "Data Source=5CG7324TYL;Initial Catalog = JobDB; Encrypt=False; Integrated Security=True";
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                //Viewing
                cmd.CommandText = "SELECT * FROM JOB";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AdminModel obj = new AdminModel();

                        obj.JOBID = (int)reader["JOBID"];
                        obj.ROLE = (string)reader["ROLE"];
                        obj.SALARY = Convert.ToInt32(reader["SALARY"]);
                        //obj.SALARY = reader.GetDecimal((2));
                        obj.COMPANYID = (int)reader["COMPANYID"];
                        //obj.COMPANY = (string)reader["studentName"];
                        jobList.Add(obj);
                    }
                }

                //inserting
                string ROLE = Request.Form["ROLE"];
                string SALARY = Request.Form["SALARY"];
                string COMPANYID = Request.Form["COMPANYID"];

                SqlCommand command = new SqlCommand("INSERT_JOB", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ROLE", SqlDbType.VarChar).Value = ROLE;
                command.Parameters.AddWithValue("@SALARY", SqlDbType.VarChar).Value = SALARY;
                command.Parameters.AddWithValue("@COMPANYID", SqlDbType.VarChar).Value = COMPANYID;

                command.ExecuteNonQuery();

                jobId = Request.Query["JOBID"];
                cmd.CommandText = "";

                //deleting
                jobId = Request.Query["JOBID"];
                cmd.CommandText = $"DELETE FROM JOB WHERE JOBID = {jobId}";

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected);
                if (rowsAffected > 0)
                {
                    Response.Redirect("/Books/Index");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewData["list_of_roles"] = jobList;
            return View();
        }
        IConfiguration configuration;
        public AdminController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            string userEmail = "gopi@gmail.com";

            string connectionString = "Data Source=5CG7324TYL;Initial Catalog = JobDB; Encrypt=False; Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT USERTYPE FROM USERS WHERE EMAIL = '{userEmail}'";

            string isAdmin = (string)cmd.ExecuteScalar();
            if (isAdmin == "ADMIN")
            {
                Response.Redirect("Views\\Admin\\Admin.cshtml");
            }

            //try
            //{
            //    string connectionString = "Data Source=5CG7324TYL;Initial Catalog = JobDB; Encrypt=False; Integrated Security=True";
            //    SqlConnection conn = new SqlConnection(connectionString);

            //    conn.Open();

            //    SqlCommand cmd = conn.CreateCommand();
            //    cmd.CommandText = "SELECT * FROM JOB";

            //    using (var reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            AdminModel obj = new AdminModel();

            //            obj.JOBID = (int)reader["JOBID"];
            //            obj.ROLE = (string)reader["ROLE"];
            //            obj.SALARY = Convert.ToInt32(reader["SALARY"]);
            //            //obj.SALARY = reader.GetDecimal((2));
            //            obj.COMPANYID = (int)reader["COMPANYID"];
            //            //obj.COMPANY = (string)reader["studentName"];
            //            jobList.Add(obj);
            //        }
            //    }

            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //ViewData["list_of_roles"] = jobList;
            return View();
        }
    }
}
