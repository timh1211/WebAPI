using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using HLLibrarySystemAPI.Models;

namespace HLLibrarySystemAPI.Controllers
{
    public class BooksController : BaseController
    {
        /// <summary>
        /// Print book info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Book> list = new List<Book>();
            list = db.Books.OrderByDescending(s => s.callNumber).ToList();
            return Ok(list.Select(s => new Book
            {
                callNumber = s.callNumber,
                authorName = s.authorName,
                Category = s.Category,
                categoryID = s.categoryID,
                image = s.image,
                ISBN_Number = s.ISBN_Number,
                lasted = s.lasted,
                quantity = s.quantity,
                Rent_Details = s.Rent_Details,
                special = s.special,
                statusBook = s.statusBook,
                title = s.title,
                views = s.views
            }));
        }

        /// <summary>
        /// Get Category into list box
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Books/GetCate")]
        public IHttpActionResult GetCate()
        {
            List<Category> listCate = new List<Category>();
            listCate = db.Categories.ToList();
            return Ok(listCate.Select(s => new Category
            {
                cateID = s.cateID,
                cateName = s.cateName
            }));
        }

        [HttpGet]
        [Route("api/Books/FindingBook")]
        public IHttpActionResult FindingBook(string id)
        {
            List<Book> listBook = new List<Book>();
            listBook = db.Books.Where(s => s.title.Contains(id)
                                        || s.callNumber.Contains(id)
                                        || s.authorName.Contains(id)
                                        || s.callNumber.Contains(id)
                                        || s.Category.cateName.Contains(id)).ToList();
            return Ok(listBook.Select(s => new Book
            {
                callNumber = s.callNumber,
                authorName = s.authorName,
                Category = s.Category,
                categoryID = s.categoryID,
                image = s.image,
                ISBN_Number = s.ISBN_Number,
                lasted = s.lasted,
                quantity = s.quantity,
                Rent_Details = s.Rent_Details,
                special = s.special,
                statusBook = s.statusBook,
                title = s.title,
                views = s.views
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
            Book book = new Book();
            book = db.Books.SingleOrDefault(s => s.callNumber.Equals(id));
            if (book == null)
            {
                ret = NotFound();
            }
            else
            {
                HttpContext.Current.Session["callNumber"] = book.callNumber;
                ret = Ok(book);
            }
            return ret;
        }

        // PUT: api/Books/5
        [HttpPut]
        public IHttpActionResult Put(string id, Book book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != book.callNumber) return BadRequest();
            db.Entry(book).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [HttpPost]
        public IHttpActionResult Post(Book book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Books.Add(book);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookExists(book.callNumber)) return Conflict();
            }
            return CreatedAtRoute("DefaultApi", new { id = book.callNumber }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            Book book = db.Books.Find(id);
            if (book == null) return NotFound();
            db.Books.Remove(book);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookBorrowed(id)) return Conflict();
            }
            return CreatedAtRoute("DefaultApi", new { id = book.callNumber }, book);
        }
        private bool BookExists(string id)
        {
            return db.Books.Count(e => e.callNumber == id) > 0;
        }
        private bool BookBorrowed(string id)
        {
            return db.Rent_Details.Count(e => e.callNumber == id) > 0;
        }
    }
}