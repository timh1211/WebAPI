using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HLLibrarySystemAPI.Models;

namespace HLLibrarySystemAPI.Controllers
{
    public class Rent_DetailsController : ApiController
    {
        private LibraryDBEntities db = new LibraryDBEntities();

        /// <summary>
        /// Print book info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Rent_Details> list = new List<Rent_Details>();
            list = db.Rent_Details.OrderByDescending(s => s.stu_userName).ToList();
            return Ok(list.Select(s => new Rent_Details
            {
                callNumber = s.callNumber,
                stu_userName = s.stu_userName,
                checkOutDate = s.checkOutDate,
                dueDate = s.dueDate,
                issueDate = s.issueDate,
                issueStatus = s.issueStatus,
                lateFees = s.lateFees,
                number_Date_Late = s.number_Date_Late,
                totalFees = s.totalFees,
                unitQuantity = s.unitQuantity
            }));
        }

        /// <summary>
        /// Edit Student Infor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public IHttpActionResult Get(string id, string user)
        {
            IHttpActionResult ret = null;
            Rent_Details borrow = new Rent_Details();
            borrow = db.Rent_Details.SingleOrDefault
                    (s => s.callNumber.Equals(id) 
                    && s.stu_userName.Equals(user));
            if (borrow == null)
            {
                ret = NotFound();
            }
            else
            {
                ret = Ok(borrow);
            }
            return ret;
        }

        // PUT: api/Rent_Details/5
        [HttpPut]
        public IHttpActionResult Put(string id, string user, Rent_Details rent_Details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rent_Details.callNumber && user != rent_Details.stu_userName)
            {
                return BadRequest();
            }
            db.Entry(rent_Details).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Rent_DetailsExists(user))
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

        // POST: api/Rent_Details
        [HttpPost()]
        public IHttpActionResult Post(Rent_Details rent_Details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rent_Details.Add(rent_Details);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Rent_DetailsExists(rent_Details.stu_userName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rent_Details.stu_userName }, rent_Details);
        }

        // DELETE: api/Rent_Details/5
        [ResponseType(typeof(Rent_Details))]
        public IHttpActionResult DeleteRent_Details(string id)
        {
            Rent_Details rent_Details = db.Rent_Details.Find(id);
            if (rent_Details == null)
            {
                return NotFound();
            }

            db.Rent_Details.Remove(rent_Details);
            db.SaveChanges();

            return Ok(rent_Details);
        }

        [HttpGet]
        [Route("api/Rent_Details/LoadingChart")]
        public IHttpActionResult LoadingChart(DateTime sDay, DateTime eDay)
        {
            //List<Rent_Details> listBorrow = new List<Rent_Details>();
            var listBorrow = db.Rent_Details.Where(s => s.checkOutDate >= sDay && s.checkOutDate <= eDay);
            var sumTotal = listBorrow.Sum(s => s.totalFees);
            return Ok(sumTotal);
        }
        private bool Rent_DetailsExists(string user)
        {
            return db.Rent_Details.Count(e => e.stu_userName == user) > 0;
        }
    }
}