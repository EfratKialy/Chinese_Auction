using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.BLL
{
    public interface IDonorService
    {
        Task<IEnumerable<Donor>> GetDonors();
        Task<Donor> AddDonor(Donor donor);
        Task<Donor> UpdateDonor(Donor donor, int Id);
        Task DeleteDonor(int id);
        Task<Donor> GetDonorByName(string name);
        Task<Donor> GetDonorByEmail(string email);
        Task<Donor> GetDonorByGift(int giftId);
        Task<List<Gift>> GetAllGiftsOfDonor(int Did);

    }
}
