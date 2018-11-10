using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PDJaya.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PDJaya.Service.Controllers
{
    /// <summary>
    /// Collection of Ticket API Methods
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly RedisDB _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public TicketsController(RedisDB context)
        {
            _context = context;
        }
       
        /*
        /// <summary>
        /// Generate ticket Ragunan
        /// </summary>
        /// <param name="input">please provide all inputs in order to get ticket</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ActionResult GenerateTicketRagunan([FromBody]TicketInput input)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var hasil = new OutputData() { IsSucceed = true };
            if (input.TicketDate < currentDate)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "Ticket date must be equal or higher than today";
            }
            if (string.IsNullOrEmpty( input.UserName))
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "Please input username";
            }
            if (input.PartnerID<0)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "Please input partner id";
            }
            if (input.TicketType < 0 || input.TicketType>1)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "Please input correct TicketType = 0 - Anak-anak, 1 - Dewasa";
            }
            if (hasil.IsSucceed)
            {
                var NewTicket = new PDJaya.Entities.Ticket() { Created = DateTime.Now, CreatedUTC = DateTime.UtcNow, CreatedBy = input.UserName, ExpiryDate = new DateTime(input.TicketDate.Year, input.TicketDate.Month, input.TicketDate.Day,23,59,59), QRTypeID = 0, TicketUID = Guid.NewGuid().ToString(), Location = input.Location, PartnerID = input.PartnerID, Id = _context.GetSequence<PDJaya.Entities.Ticket>(), Price=input.Price, TicketType = input.TicketType };
                var gen = new QRCodeTicket();
                var GeneratedTicket = gen.GenerateQR(NewTicket.TicketUID, NewTicket.CreatedBy, NewTicket.ExpiryDate,NewTicket.TicketType);
                NewTicket.QRCode = GeneratedTicket.QRCodePlain;
                NewTicket.QRCodeEncrypted = GeneratedTicket.QRCodeEncrypted;
                NewTicket.TicketProvider = "Ragunan";
                var res = _context.InsertData<Entities.Ticket>(NewTicket);
                if (res)
                {
                    hasil.Data = new { QR = GeneratedTicket.QRCodeEncrypted };
                }
                else
                {
                    hasil.IsSucceed = false;
                    hasil.ErrorMessage = "Cannot insert to db";
                }

            }
            return Ok(hasil);
        }

        /// <summary>
        /// Get Tariff Ragunan
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetTarifRagunan()
        {
             var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var datas = from x in _context.GetAllData<Entities.BankDKI.Tarif>()
                            where currentDate>=x.BerlakuDari && currentDate <= x.BerlakuSampai
                            select x;
                hasil.Data = datas.ToList();
            }
            catch(Exception ex) {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
        
            return Ok(hasil);
        }
        */
    }
}