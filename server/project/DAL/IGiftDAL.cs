using project.Models;

namespace project.DAL
{
    public interface IGiftDAL
    {
        Task<IEnumerable<Gift>> GetGifts();
        Task<Gift> AddGift(Gift gift);
        Task<Gift> UpdateGift(Gift gift);
        Task DeleteGift(int id);
        Task<IEnumerable<Gift>> FindByName(string name);
        Task<Gift> FindByGiftName(string giftName);
        Task<Gift> FindByNumOfPurchase(int num);
    }
}
