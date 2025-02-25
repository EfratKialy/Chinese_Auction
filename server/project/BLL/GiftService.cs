using project.DAL;
using project.Models;

namespace project.BLL
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDAL giftDAL;
        public GiftService(IGiftDAL giftDal)
        {
            this.giftDAL = giftDal;
        }

        public async Task<Gift> AddGift(Gift gift)
        {
            return await giftDAL.AddGift(gift);
        }

        public async Task DeleteGift(int id)
        {
            await giftDAL.DeleteGift(id);
        }

        public async Task<Gift> FindByGiftName(string giftName)
        {
            return await giftDAL.FindByGiftName(giftName);
        }

        public async Task<IEnumerable<Gift>> FindByName(string name)
        {
            return await giftDAL.FindByName(name);
        }

        public async Task<Gift> FindByNumOfPurchase(int num)
        {
            return await giftDAL.FindByNumOfPurchase(num);
        }

        public async Task<IEnumerable<Gift>> GetGifts()
        {
            return await giftDAL.GetGifts();
        }

        public async Task<Gift> UpdateGift(Gift gift,int gjiftID)
        {
            gift.Id = gjiftID;
            return await giftDAL.UpdateGift(gift);
        }
    }
}
