using project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace project.DAL
{
    public class DonorDAL : IDonorDAL
    {
        private readonly Context context;
        public DonorDAL(Context contex)
        {
            this.context = contex;
        }
        public async Task<Donor> AddDonor(Donor donor)
        {
            try
            {
                await context.Donors.AddAsync(donor);
                await context.SaveChangesAsync();
                return donor;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task DeleteDonor(int id)
        {
            try
            {
                var d = await context.Donors.FirstOrDefaultAsync(dd => dd.Id == id);
                if (d == null)
                {
                    throw new Exception($"Donor {id} not found");
                }
                context.Donors.Remove(d);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Donor> GetDonorByName(string name)
        {
            try
            {
                return await context.Donors.FirstOrDefaultAsync(d => d.Name == name);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Donor> GetDonorByEmail(string email)
        {
            try
            {
                return await context.Donors.FirstOrDefaultAsync(d => d.Email == email);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Donor> GetDonorByGift(int giftId)
        {
            try
            {
                var query = from gift in context.Gifts
                            join donor in context.Donors on gift.DonorId equals donor.Id
                            where gift.Id == giftId
                            select donor;

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<Donor>> GetDonors()
        {
            try
            {
                return await context.Donors.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<Donor> UpdateDonor(Donor donor)
        {
            try
            {
                context.Donors.Update(donor);

                await context.SaveChangesAsync();

                return donor;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("גרסה מיושנת של התורם. אנא נסה שוב.");
            }
            catch (Exception ex)
            {
                throw new Exception("שגיאה בעדכון התורם: " + ex.Message);
            }
        }

        public async Task<List<Gift>> GetAllGiftsOfDonor(int Did)
        {
            try
            {
                return await context.Gifts.Where(d => d.DonorId == Did).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
