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
using _23012018101517_LibraryManagementSystem.Authen;
using HLLibrarySystemAPI.Models;

namespace HLLibrarySystemAPI.Controllers
{
    [AuthenticationAdmin]
    public class AccountStudentsController : BaseController
    {
        /// <summary>
        /// Get student account
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<AccountStudent> list = new List<AccountStudent>();
            list = db.AccountStudents.OrderBy(s => s.stuID).ToList();
            return Ok(list.Select(s => new AccountStudent
            {
                stuID = s.stuID,
                stu_userName = s.stu_userName,
                stu_password = s.stu_password,
                statusStuAcc = s.statusStuAcc,
                email = s.email,
                Student = s.Student
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
            AccountStudent stu = new AccountStudent();
            stu = db.AccountStudents.SingleOrDefault(s => s.stu_userName.Equals(id));
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

        // PUT: api/AccountStudents/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string id, AccountStudent accountStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accountStudent.stu_userName)
            {
                return BadRequest();
            }

            db.Entry(accountStudent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountStudentExists(id))
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

        private bool AccountStudentExists(string id)
        {
            return db.AccountStudents.Count(e => e.stu_userName == id) > 0;
        }
    }
}