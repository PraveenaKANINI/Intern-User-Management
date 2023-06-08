﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UserIntern.Interfaces;
using UserIntern.Models;
using UserIntern.Models.DTO;

namespace UserIntern.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AngularCORS")]
    public class UserController : ControllerBase
    {
        private readonly IManageUser _manageUser;
        private readonly IRepo<int, Intern> _internRepo;
   

        public UserController(IManageUser manageUser, IRepo<int, Intern> internRepo)
        {
            _manageUser = manageUser;
            _internRepo = internRepo;
           
        }
        [HttpPost("Register User")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register(InternDTO intern)
        {
            var result = await _manageUser.Register(intern);
            if (result != null)
            {
                return Created("Home", result);
            }
            return BadRequest(new Error(2, "Unable to register user at this moment"));
        }

        [HttpPost("Login User")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserDTO userDTO)
        {
            UserDTO user = await _manageUser.Login(userDTO);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest(new Error(2, "Cannot Login user. Password or username may be incorrect or user may be not registered"));


        }
        [Authorize(Roles = "admin")]
        [HttpGet("Get All Intern")]
        [ProducesResponseType(typeof(ICollection<Intern>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Intern>> GetAllInters()
        {
            ICollection<Intern> interns = await _internRepo.GetAll();
            if (interns != null)
            {
                return Ok(interns);
            }
            return NotFound(new Error(1, "No Intern Details Currently"));

        }
      
        [Authorize(Roles = "admin")]
        [HttpGet("Get Single Intern")]
        [ProducesResponseType(typeof(ICollection<Intern>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Intern>> GetSingleintern(int id)
        {
            Intern intern = await _internRepo.Get(id);
            if (intern != null)
            {
                return Ok(intern);
            }
            return NotFound(new Error(1, "No intern Detail with this id"));

        }
        



    


    }
}

