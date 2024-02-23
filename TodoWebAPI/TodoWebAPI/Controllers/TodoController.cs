using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace TodoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private IConfiguration _configuration;
        public TodoController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetNotes")]
        public JsonResult GetNotes()
        {
            string query = "select * from dbo.Notes";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("todoApp");
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(sqlDatasource))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddNote")]
        public JsonResult AddNote([FromForm] string newNote)
        {
            var result = string.Empty;
            try
            {
                string query = "insert into dbo.Notes values(@newNotes)";
                DataTable table = new DataTable();
                string sqlDatasource = _configuration.GetConnectionString("todoApp");
                using (SqlConnection conn = new SqlConnection(sqlDatasource))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@newNotes", newNote);
                        command.ExecuteScalar();
                    }
                    result = $"added {newNote} successfully";
                }
            }
            catch (Exception ex) 
            {
                result = $"An error occured {ex.Message}";
            }

            return new JsonResult(result);
        }

        [HttpDelete]
        [Route("DeleteNote")]
        public JsonResult DeleteNote(int id)
        {
            var result = string.Empty;
            try
            {
                string query = "delete from dbo.Notes where id=@id";
                DataTable table = new DataTable();
                string sqlDatasource = _configuration.GetConnectionString("todoApp");
                using (SqlConnection conn = new SqlConnection(sqlDatasource))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteScalar();
                    }
                    result = $"deleted {id} successfully";
                }
            }
            catch (Exception ex)
            {
                result = $"An error occured {ex.Message}";
            }

            return new JsonResult(result);
        }

    }
}
