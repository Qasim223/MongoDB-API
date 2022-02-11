using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _enviroment;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _enviroment = environment;

        }

        [HttpGet]
        //Get List of Employee Records
        public JsonResult Get()
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);

        }

        [HttpPost]
        //Add new Record into Employee Table
        public JsonResult Post(Employee oParams)
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            int iLastId = dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable().Count();
            oParams.EmployeeId = iLastId + 1;

            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").InsertOne(oParams);

            return new JsonResult("New Employee Created");


        }

        [HttpPut]
        //Update Record in Employee Table
        public JsonResult Put(Employee oParams)
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            //Find Record from Database Table
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", oParams.EmployeeId);

            //Field that needs updating
            var update = Builders<Employee>.Update.Set("EmployeeName", oParams.EmployeeName)
                                                    .Set("Department", oParams.Department)
                                                    .Set("DateOfJoining", oParams.DateOfJoining)
                                                    .Set("PhotoFileName", oParams.PhotoFileName);

            //Send Record that needs updating and field that will be updated
            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Updated Employee for " + oParams.EmployeeName);


        }

        [HttpDelete("{Id}")]
        //Update Record in Employee Table
        public JsonResult Delete(int Id)
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            //Find Record from Database Table
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", Id);

            //Delete record where the DepartmentId = Id
            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Deleted Employee Succesfully");


        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {

            try
            {

                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _enviroment.ContentRootPath + "/Photos/" + filename;

                using (var fs = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(fs);
                }

                return new JsonResult(filename);

            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }

        }

    }
}
