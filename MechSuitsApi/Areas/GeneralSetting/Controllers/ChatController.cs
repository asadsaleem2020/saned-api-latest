using CoreInfrastructure.GeneralSetting.Age;
using MechSuitsApi.Classes;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.GeneralSetting.Chat;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using CoreInfrastructure.ToolbarItems;
using System.Linq;
using System.Data;
using Microsoft.OpenApi.Any;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    // [Authorize]
    [Route("api/chat")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ChatController : ControllerBase
    {

        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;

        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";



        public ChatController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            con = c.V_connection;
            this.uriService = uriService;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            //  user = currentUser.FindFirst("User").Value.ToString();


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Chat>>> Get()
        {
            //{id}/{name}

            return await _context.chat.ToListAsync();

        }

        [HttpGet("{senderID}/{recieverID}")]
        public ActionResult<IEnumerable<M_Chat>> GetChat(string senderID, string recieverID)
        {
            if (senderID != null && senderID != "" && recieverID != null && recieverID != "")
            {
                var sql = "SELECT Company_Code, Code, SenderID,RecieverID, MessageText, SendingTime, " +
                "Reply, SeenStatus, SeenTime, Status, Sort, Locked FROM chat " +
                "where SenderID=" + senderID + " AND RecieverID=" + recieverID + " OR SenderID=" + recieverID + " AND RecieverID=" + senderID + "ORDER BY CONVERT(DATETIME,SendingTime) DESC";

                var m = _context.chat.FromSqlRaw(sql).ToList();

                if (m == null)
                {
                    // return NotFound();
                }

                return m;
            }
            return null;
        }

        [HttpGet("{senderID}")]
        public ActionResult<IEnumerable<M_Chat>> GetChat_inNotification(string senderID)
        {
            var sql = "SELECT Company_Code, Code, SenderID,RecieverID, MessageText, SendingTime, " +
                "Reply, SeenStatus, SeenTime, Status, Sort, Locked FROM chat " +
                "where SenderID!="+ senderID+" AND RecieverID="+ senderID +" AND SeenStatus = 0 ORDER BY CONVERT(DATETIME, SendingTime) DESC"; //ORDER BY CONVERT(DATETIME,SendingTime) DESC

            var m = _context.chat.FromSqlRaw(sql).ToList();

            if (m == null)
            {
                // return NotFound();
            }

            return m;
        }

        [HttpPost]
        public async Task<ActionResult<M_Chat>> create(M_Chat chatData)
        {
            //DateTime now = DateTime.Now;
            chatData.Company_Code = "C001";
            chatData.Code = getNext();
            chatData.SendingTime = DateTime.Now.ToString();
            chatData.Reply = "0";
            chatData.SeenStatus = "0";
            chatData.SeenTime = "1999, 12, 12, 0, 0, 0";
            //chatData.Status = "0";
            //chatData.Sort = "0";
            //chatData.Locked = "0";
            Console.WriteLine(chatData);
            _context.chat.Add(chatData);
            


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChat", new { senderID = chatData.SenderID, recieverID = chatData.RecieverID }, chatData);
        }

        [HttpPut]
        [Route("seenStatus")]
        public ActionResult<IEnumerable<M_Chat>> update(M_Chat xyz)
            {
            SqlDataReader sqldr;
            string strsql = @"UPDATE chat SET SeenStatus = 1 where RecieverID=" + xyz.RecieverID + "AND SeenStatus= 0";
              if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return Ok("Updated !!!");
        }

        public string getNext()
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT))  ,1) +1  AS code FROM Chat";
            //strsql = "select  max(  cast(code as bigint)) +1  as code from Agents where    Company_Code='" + Company_Code + "' ";

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

    }
}
