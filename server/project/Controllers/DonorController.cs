using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.BLL;
using project.Models;
using project.Models.DTO;
using System.Data;

namespace project.Controllers
{
    [ApiController]
    [Route("donor/api")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService donorService;
        private readonly IMapper mapper;

        public DonorController(IDonorService donorService1, IMapper mapper1)
        {
            donorService = donorService1;
            mapper = mapper1;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Donor>>> GetDonors()
        {
            try
            {
                return  Ok(await donorService.GetDonors());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Donor>> AddDonor(DonorDTO donorDto)
        {
            try
            {
                var d = mapper.Map<Donor>(donorDto);
                return  Ok(await donorService.AddDonor(d));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Donor>> UpdateDonor(DonorDTO donorDto,int Id)
        {
            try
            {
                var d = mapper.Map<Donor>(donorDto);
                return Ok(await donorService.UpdateDonor(d, Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public async Task DeleteDonor(int id)
        {
           await donorService.DeleteDonor(id);
        }
        [HttpGet("donor/api/byName")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Donor>> GetDonorByName([FromQuery] string name)
        {
            try
            {
                return Ok(await donorService.GetDonorByName(name));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("donor/api/byEmail")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetDonorByEmail([FromQuery] string email)
        {
            try
            {
                return Ok(await donorService.GetDonorByEmail(email));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("donor/api/byGift")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetDonorByGift([FromQuery] int giftId)
        {
            try
            {
                return Ok(await donorService.GetDonorByGift(giftId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("donor/api/byDonorId")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<Gift>>> GetAllGiftsOfDonor(int Did)
        {
            try
            {
                List<Gift> lg = await donorService.GetAllGiftsOfDonor(Did);
                return Ok(lg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
