using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.Models;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStudentService" in both code and config file together.
    [ServiceContract]
    public interface IStudentService
    {
        [OperationContract]
        void AddStudents(student obj);
        [OperationContract]
        List<student> GetStudents();
        //[OperationContract]
        //StudentContract GetStudentByID(string Name);

        //[OperationContract]
        //void UpdateStudent(StudentContract studentcontractor);
        //[OperationContract]
        //void DeleteStudent(string Name);

    }
    //[DataContract]
    //public class StudentContract
    //{

    //    [DataMember]
    //    public string Name { get; set; }
    //    [DataMember]
    //    public int Age { get; set; }
    //}

}
