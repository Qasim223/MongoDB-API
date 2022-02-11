using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Department
    {

        #region Departments Fields
        
        public ObjectId Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        #endregion Departments Fields

    }
}
