using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentDataAccess;
using System.Threading;

namespace StudentService.Controllers
{
    public class StudentsController : ApiController
    {

        [HttpGet]
     //   [BasicAuthentication]
        public HttpResponseMessage LoadStudents(string gender="All")
        {

            using (StudentDBEntities entities = new StudentDBEntities())
            {
               // string username = Thread.CurrentPrincipal.Identity.Name;
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Students.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, 
                            entities.Students.Where(e=>e.Gender.ToLower()=="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Students.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

              
            }
        }
       [Route("api/sachin/{id}")]
       [HttpGet]
        public HttpResponseMessage LoadStudentById(int id)
        {
            using (StudentDBEntities entities = new StudentDBEntities())
            {
                var entity = entities.Students.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student with id= " + id.ToString() + " not found");
                }
            }
        }
        [HttpPost]
       
        public HttpResponseMessage CreateStudent([FromBody]Student student)
        {
            try
            {
                using (StudentDBEntities entities = new StudentDBEntities())
                {
                    entities.Students.Add(student);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, student);
                    message.Headers.Location = new Uri(Request.RequestUri + student.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [Route("api/students/{id}/courses")]
        public IEnumerable<string> GetStudentCourses(int id)
        {
            if (id == 1)
            {
                return new List<string>() { "C#", "Asp.net", "sql" };
            }
            else
                return new List<string>() { "C++", "Asp.net", "mysql" };
        }

        [HttpDelete]
        public HttpResponseMessage RemoveStudentById(int id)
        {
            try
            {
                using (StudentDBEntities entities = new StudentDBEntities())
                {
                    var entity = entities.Students.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student id =" + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Students.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateStudentById(int id, [FromBody]Student student)
        {
            try
            {
                using (StudentDBEntities entities = new StudentDBEntities())
                {
                    var entity = entities.Students.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student with id = " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.FirstName = student.FirstName;
                        entity.LastName = student.LastName;
                        entity.Gender = student.Gender;
                        entity.NoOfSubjects = student.NoOfSubjects;
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}