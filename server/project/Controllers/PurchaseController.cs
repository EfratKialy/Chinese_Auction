using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using project.BLL;
using project.Models.DTO;
using project.Models;
using project.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace project.Controllers
{
    [ApiController]
    [Route("purchase/api")]
    public class PurchaseController : ControllerBase
    {

        private readonly IPurchaseService purchaseService;
        private readonly IMapper _mapper;
        private readonly Context context;


        public PurchaseController(IPurchaseService parchaseService, IMapper mapper , Context context)
        {
            this.purchaseService = parchaseService;
            this._mapper = mapper;
            this.context = context; 
        }
        [HttpGet("purchase/api/GetPurchaseByGift")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchaseByGift([FromQuery] int id)
        {
            try
            {
                return Ok(await purchaseService.GetPurchaseByGift(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("purchase/api/GetPurchaseByCustomer")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchaseByCustomer([FromQuery] int id)
        {
            try
            {
                return Ok(await purchaseService.GetPurchaseByCustomer(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetAllPurchase()
        {
            try
            {
                return Ok(await purchaseService.GetAllPurchase());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Purchase>> AddPurchase(PurchaseDTO purchaseDto)
        {
            try
            {
                var p = _mapper.Map<Purchase>(purchaseDto);
                return await purchaseService.AddPurchase(p);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }


        }
        [HttpDelete]
        [AllowAnonymous]
        public async Task DeletePurchaseFromCart(int id)
        {
            await purchaseService.DeletePurchaseFromCart(id);
        }


        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<Gift>> Updatepurchase(PurchaseDTO purchaseDTO, int purchaseId)
        {
            try
            {
                var purchase = _mapper.Map<Purchase>(purchaseDTO);
                return Ok(await purchaseService.Updatepurchase(purchase, purchaseId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("purchase/api/MakeRaffleByGift")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Customer>> MakeRaffleByGift([FromQuery] int id)
        {
            try
            {
                return Ok(await purchaseService.MakeRaffleByGift(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpGet("download-winners")]
        //[Authorize(Roles = "Manager")]
        //public async Task<IActionResult> DownloadWinners()
        //{
        //    purchaseService.DownloadWinners();
        //}

        [HttpGet("download-gifts-with-winners")]
        [Authorize(Roles = "Manager")]

        public async Task<IActionResult> DownloadGiftsWithWinners()
        {
            var gifts = await context.Gifts
                .Select(g => new
                {
                    GiftName = g.Name,
                    WinnerId = g.WinnerId
                })
                .ToListAsync();

            var winnerIds = gifts.Select(g => g.WinnerId).Where(id => id != null).ToList();
            var winners = await context.Customers
                .Where(c => winnerIds.Contains(c.Id))
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .ToListAsync();

            var giftWithWinnerNames = gifts.Select(g => new
            {
                g.GiftName,
                WinnerName = winners.FirstOrDefault(w => w.Id == g.WinnerId)?.Name ?? "No winner"
            }).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("GiftName,WinnerName");
            foreach (var item in giftWithWinnerNames)
            {
                sb.AppendLine($"{item.GiftName},{item.WinnerName}");
            }

            var fileName = "gifts_with_winners.csv";
            var fileContent = Encoding.UTF8.GetBytes(sb.ToString());

            return File(fileContent, "text/csv", fileName);
        }



    }
}


