using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using project.BLL;
using project.Models;
using project.Models.DTO;
using System.IO;
using Microsoft.Extensions.Configuration;
using project.DAL;
using Microsoft.AspNetCore.Authorization;

namespace project.Controllers
{
    [ApiController]
    [Route("gift/api")]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService giftService;
        private readonly IMapper mapper;
        private readonly string storedFilesPath;

        public GiftController(IGiftService giftService, IMapper mapper1, IConfiguration config)
        {
            this.giftService = giftService;
            mapper = mapper1;
            storedFilesPath = config["StoredFilesPath"];
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Gift>> AddGiftApload([FromForm] GiftDTO giftDto, IFormFile image2)
        {
            try
            {
                var gift = mapper.Map<Gift>(giftDto);

                if (image2 != null && image2.Length > 0)
                {
                    var imagePath = Path.Combine("wwwroot/images", image2.FileName);
                    var imageDirectory = Path.GetDirectoryName(imagePath);

                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image2.CopyToAsync(stream);
                    }

                    // Use relative path for client access
                    gift.Image = $"\\images\\{image2.FileName}";
                }

                var addedGift = await giftService.AddGift(gift);

                if (addedGift == null)
                {
                    throw new Exception("Failed to add the gift to the database.");
                }

                return Ok(addedGift);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Gift>>> GetGifts()
        {
            try
            {
                return Ok(await giftService.GetGifts());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Gift>> AddGift([FromForm] GiftDTO giftDto, IFormFile image)
        {
            try
            {
                string imagePath = null;

                if (image != null)
                {
                    var filePath = Path.Combine(storedFilesPath, $"{DateTime.Now.Ticks}_{image.FileName}");

                    if (!Directory.Exists(storedFilesPath))
                    {
                        Directory.CreateDirectory(storedFilesPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imagePath = $"./{filePath}";
                }

                var gift = mapper.Map<Gift>(giftDto);
                gift.Image = imagePath; // Assuming you have ImagePath property in Gift model
                return Ok(await giftService.AddGift(gift));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<Gift>> UpdateGift(int id, [FromForm] GiftDTO giftDto, IFormFile image)
        //{
        //    try
        //    {
        //        var existingGift = await giftService.GetGiftById(id);
        //        if (existingGift == null)
        //        {
        //            return NotFound();
        //        }

        //        // Update the existing gift properties
        //        existingGift.Name = giftDto.Name;
        //        existingGift.Price = giftDto.Price;
        //        // Update other properties as needed

        //        // Handle image update if provided
        //        if (image != null)
        //        {
        //            var imagePath = Path.Combine(storedFilesPath, $"{DateTime.Now.Ticks}_{image.FileName}");

        //            if (!Directory.Exists(storedFilesPath))
        //            {
        //                Directory.CreateDirectory(storedFilesPath);
        //            }

        //            using (var stream = new FileStream(imagePath, FileMode.Create))
        //            {
        //                await image.CopyToAsync(stream);
        //            }

        //            existingGift.Image = $"./{imagePath}";
        //        }

        //        // Update the gift in the database
        //        var updatedGift = await giftService.UpdateGift(existingGift);
        //        return Ok(updatedGift);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpPut("/gift/api/giftId")]
        [AllowAnonymous]
        public async Task<ActionResult<Gift>> UpdateGift(GiftDTO giftDTO,int giftId)
        {
            try
            {
                var gift = mapper.Map<Gift>(giftDTO);
                return Ok(await giftService.UpdateGift(gift,giftId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public async Task DeleteGift(int id)
        {
            await giftService.DeleteGift(id);
        }

        [HttpGet("GetByDonorName")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Gift>>> FindByName([FromQuery] string name)
        {
            try
            {
                return Ok(await giftService.FindByName(name));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/GetByGiftName")]
        [AllowAnonymous]
        public async Task<ActionResult<Gift>> FindByGiftName([FromQuery] string giftName)
        {
            try
            {
                return Ok(await giftService.FindByGiftName(giftName));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/GetByNumOfPurchases")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Gift>> FindByNumOfPurchase([FromQuery] int num)
        {
            try
            {
                return Ok(await giftService.FindByNumOfPurchase(num));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
