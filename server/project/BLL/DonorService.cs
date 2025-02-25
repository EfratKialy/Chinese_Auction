using Microsoft.AspNetCore.Mvc;
using project.DAL;
using project.Models;

namespace project.BLL
{
    public class DonorService : IDonorService
    {
        private readonly IDonorDAL donorDAL;
        private readonly EmailValidator validator;
        public DonorService(IDonorDAL donorDal, EmailValidator validator)
        {
            donorDAL = donorDal;
            this.validator = validator;
        }
        public async Task<Donor> AddDonor(Donor donor)
        {
            if (!validator.IsValidEmail(donor.Email))
            {
                throw new ArgumentException();
            }
            return await donorDAL.AddDonor(donor);
        }

        public async Task DeleteDonor(int id)
        {
           await donorDAL.DeleteDonor(id);
        }

        public async Task<IEnumerable<Donor>> GetDonors()
        {
            return await donorDAL.GetDonors();
        }

        public async Task<Donor> UpdateDonor(Donor donor ,int Id)
        {
            if (!validator.IsValidEmail(donor.Email))
            {
                throw new ArgumentException();
            }
            donor.Id=Id;
            return await donorDAL.UpdateDonor(donor);
        }

        public async Task<Donor> GetDonorByName( string name)
        {
            return await donorDAL.GetDonorByName(name);
        }
        public async Task<Donor> GetDonorByEmail(string email)
        {
            if(!validator.IsValidEmail(email)) 
            {
                throw new ArgumentException();
            }
            return await donorDAL.GetDonorByEmail(email);
        }
        public async Task<Donor> GetDonorByGift(int giftId)
        {
            return await donorDAL.GetDonorByGift(giftId);
        }

        public async Task<List<Gift>> GetAllGiftsOfDonor(int Did)
        {
            return await donorDAL.GetAllGiftsOfDonor(Did);
        }
    }
}
