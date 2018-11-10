using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDJaya.Service.Helpers;

namespace Artos.Services.Sync.Controllers
{
    /// <summary>
    /// Collection of Ticket API Methods
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncController : Controller
    {
        private readonly RedisDB _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public SyncController(RedisDB context)
        {
            _context = context;
        }


        /*
        [HttpPost("[action]")]
        public ActionResult PushUsedTicket([FromBody]List<UsedTicket> UsedTickets)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                foreach (var item in UsedTickets)
                {
                    item.Id = _context.GetSequence<UsedTicket>();
                    _context.InsertData<UsedTicket>(item);

                }  
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        /// <summary>
        /// Get used ticket list
        /// </summary>
        /// <param name="ticketdate">sample : 2018-05-24T03:09:55.705Z</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetUsedTicket(DateTime ticketdate)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var dateStart = new DateTime(ticketdate.Year, ticketdate.Month, ticketdate.Day, 0, 0, 0);
                var dateEnd = new DateTime(ticketdate.Year, ticketdate.Month, ticketdate.Day, 23, 59, 59);

                var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
               
                var datas = from x in _context.GetAllData<Entities.UsedTicket>()
                            where x.UsedDate >= dateStart && x.UsedDate <= dateEnd
                            orderby x.Id
                            select x;
                hasil.Data = datas.ToList();
            }
            catch(Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }


        /// <summary>
        /// Get generated ticket remember : startdate < enddate
        /// </summary>
        /// <param name="StartDate">sample : 2018-05-24T03:09:55.705Z</param>
        /// <param name="EndDate">sample : 2018-05-24T03:09:55.705Z</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetGeneratedQRTicket(DateTime StartDate, DateTime EndDate)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var dateStart = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0);
                var dateEnd = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
                if (dateStart > dateEnd)
                {
                    hasil.IsSucceed = false;
                    hasil.ErrorMessage = "StartDate must be lower than EndDate";
                    return Ok( hasil);
                }
                var datas = from x in _context.GetAllData<Entities.Ticket>()
                            where x.Created >= dateStart && x.Created <= dateEnd
                            orderby x.Id
                            select x;
                hasil.Data = datas.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }
        */

    }
}