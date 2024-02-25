using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
                private readonly PaymentService _paymentService;


        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [Authorize(Roles="Admin")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(long userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        // [Authorize(Roles="Admin,Student")]

        [HttpPost("student")]
        public async Task<IActionResult> CreateUser(User user)
        {
            await _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
        }

        // [Authorize(Roles="Admin,Student")]

        // [HttpPost("student")]
        // public async Task<IActionResult> CreateStudent(Student student)
        // {
        //     await _userService.CreateStudent(student);
        //     return CreatedAtAction(nameof(GetUserById), new { userId = student.UserId }, student);
        // }

        [Authorize(Roles="Student")]

        [HttpPut("student/{id}")]
        public async Task<IActionResult> UpdateUser(long studentId, User user)
        {
            if (studentId != user.UserId)
            {
                return BadRequest();
            }

            var existingUser = await _userService.GetUserById(studentId);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userService.UpdateUser(user);

            return NoContent();
        }
        
        [Authorize(Roles="Admin")]

        [HttpDelete("student/{id}")]
        public async Task<IActionResult> DeleteUser(long studentId)
        {
            var existingUser = await _userService.GetUserById(studentId);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(studentId);

            return NoContent();
        }
        // [Authorize(Roles="Student")]

        // [HttpPost("student/payment")]
        // public async Task<IActionResult> PostStudentPayment(long studentId, Payment payment)
        // {
        //     payment.StudentId = studentId;
        //     await _userService.AddPaymentToStudent(payment);
        //     return CreatedAtAction(nameof(PostStudentPayment), new { studentId }, payment);
        // }


        //  [Authorize]

    //    [HttpPost("student/payment")]
    //     public async Task<IActionResult> PostStudentPayment(long userId, Payment payment)
    //     {
    //         payment.UserId = userId;
    //         await _userService.AddPaymentToStudent(payment);
    //         return CreatedAtAction(nameof(PostStudentPayment), new { userId }, payment);
    //     }
    [HttpPost("student/payment")]
public async Task<IActionResult> PostStudentPayment(long userId, Payment payment)
{
    try
    {
        payment.UserId = userId;
        await _userService.AddPaymentToStudent(payment);
        return CreatedAtAction(nameof(PostStudentPayment), new { userId }, payment);
    }
    catch (Exception ex)
    {
        // Log the exception for debugging
        Console.WriteLine(ex);
        return StatusCode(500, "Internal server error");
    }
}



    //     [Authorize(Roles="Student")]
    // [HttpPost("student/payment")]
    // public async Task<IActionResult> PostStudentPayment(string userId, Payment payment)
    // {
    //     try
    //     {
    //         // Find the student associated with the userId
    //         var student = await _userService.GetStudentByUserId(userId);
    //         if (student == null)
    //         {
    //             return BadRequest(new { Status = "Error", Message = "Student not found" });
    //         }

    //         // Assign the studentId to the payment
    //         payment.StudentId = student.StudentId;

    //         // Add the payment to the student
    //         await _userService.AddPaymentToStudent(payment);

    //         return CreatedAtAction(nameof(PostStudentPayment), new { studentId = student.StudentId }, payment);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    //     }
    // }

// [Authorize(Roles="Student")]
// [HttpPost("student/payment")]
// public async Task<IActionResult> CreatePayment(Payment payment)
// {
//     try
//     {
//         // Create the payment
//         await _paymentService.CreatePayment(payment);

//         // Return the created payment with its ID in the response
//         return CreatedAtAction(nameof(CreatePayment), new { id = payment.PaymentID }, payment);
//     }
//     catch (Exception ex)
//     {
//         // Handle exceptions
//         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//     }
// }

        [Authorize(Roles="Admin,Student")]

        [HttpGet("student/user/{userId}")]
        public async Task<IActionResult> GetStudentByUserId(long userId)
        {
            var student = await _userService.GetStudentByUserId(userId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        
        [Authorize(Roles="Admin")]

        [HttpGet("admin/payment")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _userService.GetAllPayments();
            return Ok(payments);
        }
    }
}
