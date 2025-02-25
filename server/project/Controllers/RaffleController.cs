using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using project.BLL;
using project.Models.DTO;
using project.Models;
using project.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace project.Controllers
{
    [ApiController]
    [Route("raffle/api")]
    public class RaffleController : ControllerBase
    {

        private readonly IPurchaseService purchaseService;
        private readonly IMapper _mapper;

        public RaffleController(IPurchaseService parchaseService, IMapper mapper)
        {
            this.purchaseService = parchaseService;
            this._mapper = mapper;
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

    }
}


