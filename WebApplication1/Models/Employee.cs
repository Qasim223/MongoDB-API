using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Employee
    {

        #region Employee Fields

        public ObjectId Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }

        #endregion Employee Fields

    }
}
