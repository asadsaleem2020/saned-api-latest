﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
 
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.SpnoserShip;
using CoreInfrastructure.ToolbarItems;

namespace MechSuitsApi.Areas.toolbarController
{
    [Route("api/Candidates_Reservation")]
    [ApiController]
    public class CandidateReservationController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public CandidateReservationController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_CandidateReservation>>> GetList()
        {
            return await _context.Candidates_Reservation.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_CandidateReservation>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Candidates_Reservation
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Candidates_Reservation.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_CandidateReservation>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_CandidateReservation>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.Candidates_Reservation.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Candidates_Reservation.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_CandidateReservation>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{Code}")]
        public async Task<ActionResult<M_CandidateReservation>> Getm(string Code)
        {
            var m = await _context.Candidates_Reservation.FindAsync(Code);
            if (m == null)
            {
                return NotFound();
            }
            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_CandidateReservation m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_CandidateReservation obj = new M_CandidateReservation();
                obj = await _context.Candidates_Reservation.FindAsync(m.Code);

                if (obj != null)
                {
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;
                }
                _context.Entry(obj).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
        [HttpPost]
        public async Task<ActionResult<M_CandidateReservation>> create(M_CandidateReservation data)
        {
            data.Code = getNext(companycode);

            _context.Candidates_Reservation.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { Code = data.Code }, data);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select max(cast(Code as bigint)) +1 as Code from Candidates_Reservation where Company_Code='" + Company_Code + "' ";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1";
            return no;
        }
        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_CandidateReservation>> Deletem(Int64 id)
        {
            var m = await _context.Candidates_Reservation.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.Candidates_Reservation.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}


