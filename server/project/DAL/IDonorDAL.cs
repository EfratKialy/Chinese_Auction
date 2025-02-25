using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.DAL
{
    public interface IDonorDAL
    {
        Task<IEnumerable<Donor>> GetDonors();
        Task<List<Gift>> GetAllGiftsOfDonor(int Did);
        Task<Donor> AddDonor(Donor donor);
        Task<Donor> UpdateDonor(Donor donor);
        Task DeleteDonor(int id);
        Task<Donor> GetDonorByName(string name);
        Task<Donor> GetDonorByEmail(string email);
        Task<Donor> GetDonorByGift(int giftId);
    }
}
