using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        //Get List of Deparment Records
        public JsonResult Get()
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dbList);

        }

        [HttpPost]
        //Add new Record into Department Table
        public JsonResult Post(Department oParams)
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            int iLastId = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable().Count();
            oParams.DepartmentId = iLastId + 1;

            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").InsertOne(oParams);

            return new JsonResult("New Department Created");


        }

        [HttpPut]
        //Update Record in Departmetnt Table
        public JsonResult Put(Department oParams)
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            //Find Record from Database Table
            var filter = Builders<Department>.Filter.Eq("DepartmentId", oParams.DepartmentId);
            
            //Field that needs updating
            var update = Builders<Department>.Update.Set("DepartmentName", oParams.DepartmentName);

            //Send Record that needs updating and field that will be updated
            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").UpdateOne(filter, update);

            return new JsonResult("Updated Department for " + oParams.DepartmentName);


        }

        [HttpDelete("{Id}")]
        //Update Record in Departmetnt Table
        public JsonResult Delete(int Id)
        {
            //Connect to MongoDB Database
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeApp"));

            //Find Record from Database Table
            var filter = Builders<Department>.Filter.Eq("DepartmentId", Id);            

            //Delete record where the DepartmentId = Id
            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").DeleteOne(filter);

            return new JsonResult("Deleted Department Succesfully");


        }

    }
}
