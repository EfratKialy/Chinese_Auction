using project.Models;

namespace project.BLL
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> GetGifts();
        Task<Gift> AddGift(Gift gift);
        Task<Gift> UpdateGift(Gift gift,int giftId);
        Task DeleteGift(int id);
        Task<IEnumerable<Gift>> FindByName(string name);
        Task<Gift> FindByGiftName(string giftName);
        Task<Gift> FindByNumOfPurchase(int num);
    }
}
