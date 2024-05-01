using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {

        private readonly AppDbContext _context;
        public StudentsController(AppDbContext context){
            _context = context;
        }



        [HttpGet]
        // Get list of students
        public async Task<IEnumerable<Student>> getStudent(){
            var students = await _context.Students.AsNoTracking().ToListAsync();
            return students;
        }



        [HttpPost]
        // Create student
        public async Task<IActionResult> Create(Student student){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            await _context.AddAsync(student);

            var result = await _context.SaveChangesAsync();

            if(result > 0){
                return Ok();
            }
            return BadRequest();
        }



        [HttpDelete("{id:int}")]
        // Delete Student
        public async Task<IActionResult> Delete(int id){
          var student = await _context.Students.FindAsync(id);
          if(student == null){
            return NotFound();
          }

          _context.Remove(student);

          var result = await _context.SaveChangesAsync();

          if(result > 0 ){
            return Ok("Student was deleted");
          }

          return BadRequest("Unable to delete student");
        }



        [HttpGet("{id:int}")]
        // Get a single student {id}

        // Action Result returns whole model
        public async Task<ActionResult<Student>> GetStudent(int id){
            var student = await _context.Students.FindAsync(id);
            if(student == null){
            return NotFound("Sorry student not found");
          }
          return Ok(student);
        }



        [HttpPut("{id:int}")]
        // Update PUT
        // IActionResult to return object

        public async Task<IActionResult> EditStudent(int id, Student student){
            var studentfromDb = await _context.Students.FindAsync(id);
            if(studentfromDb == null){
            return BadRequest("Sorry student not found");
        }
            studentfromDb.Name = student.Name;
            studentfromDb.Email = student.Email;
            studentfromDb.Address = student.Address;
            studentfromDb.PhoneNumber = student.PhoneNumber;

            var result = await _context.SaveChangesAsync();

            if(result > 0 ){
            return Ok("Student was Edited");
          }
            return BadRequest("Unable to update data");
        }
        
    }
}
