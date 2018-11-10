using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDJaya.Models;
using PDJaya.Service.Helpers;

namespace PDJaya.Service.Controllers
{
    /// <summary>
    /// Reporting API
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly PDJayaDB _context;
        public ReportsController(PDJayaDB context)
        {
            _context = context;
        }
        /// <summary>
        /// Report Pembayaran by Date Interval
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetPaymentByDate([FromQuery]DateTime StartDate,[FromQuery] DateTime EndDate)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.Payments
                            where x.PaymentDate >= StartDate && x.PaymentDate <= EndDate
                            orderby x.PaymentDate ascending
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
    }
}