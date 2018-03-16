using HLLibrarySystemAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace HLLibrarySystemAPI.Controllers
{
    public class StudentsController : BaseController
    {
        /// <summary>
        /// print student
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Student> list = new List<Student>();
            list = db.Students.OrderBy(s => s.stuID).ToList();
            return Ok(list.Select(s => new Student
            {
                stuID = s.stuID,
                stuFirstName = s.stuFirstName,
                stuLastName = s.stuLastName,
                stuAddress = s.stuAddress,
                stuPhone = s.stuPhone,
                majorID = s.majorID,
                Major = s.Major
            }));
        }

        /// <summary>
        /// Get Major into list box (selection)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Students/GetMajor")]
        public IHttpActionResult GetMajor()
        {
            List<Major> listMajor = new List<Major>();
            listMajor = db.Majors.ToList();
            return Ok(listMajor.Select(s=> new Major {
                majorID = s.majorID,
                majorName = s.majorName
            }));
        }

        /// <summary>
        /// Edit Student Infor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public IHttpActionResult Get(string id)
        {
            IHttpActionResult ret = null;
            Student stu = new Student();
            stu = db.Students.SingleOrDefault(s => s.stuID.Equals(id));
            if (stu == null)
            {
                ret = NotFound();
            }
            else
            {
                ret = Ok(stu);
            }
            return ret;
        }

        [HttpPost()]
        public IHttpActionResult Post(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.stuID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = student.stuID }, student);
        }


        [HttpPut()]
        public IHttpActionResult Put(string id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.stuID)
            {
                return BadRequest();
            }
            db.Entry(student).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/Students/GetSession")]
        public IHttpActionResult GetSession()
        {
            List<string> sessionStorage = new List<string>();
            if (HttpContext.Current.Session["acc"] == null)
            {
                sessionStorage.Add(null);
            }
            else
            {
                sessionStorage.Add(HttpContext.Current.Session["acc"].ToString());
                sessionStorage.Add(HttpContext.Current.Session["name"].ToString());
            }
            return Ok(sessionStorage);
        }

        private bool StudentExists(string id)
        {
            return db.Students.Count(e => e.stuID == id) > 0;
        }
    }
}
