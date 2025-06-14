using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new();

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll() => _users;

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
            => _users.FirstOrDefault(u => u.Id == id) is User user ? Ok(user) : NotFound();

        [HttpPost]
        public ActionResult<User> Create(User newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Name) ||
                string.IsNullOrWhiteSpace(newUser.Email))
            {
                return BadRequest("Invalid user data.");
            }

            if (!Regex.IsMatch(newUser.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Invalid email format.");
            }

            try
            {
                newUser.Id = _users.Count + 1;
                _users.Add(newUser);
                return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                // Optional: log the exception here
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound();
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound();
            _users.Remove(user);
            return NoContent();
        }
    }
}
