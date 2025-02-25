using Microsoft.EntityFrameworkCore;
using project.Models;
using System.Drawing;

namespace project.DAL
{
    public class GiftDAL : IGiftDAL
    {
        private readonly Context context;
        public GiftDAL(Context contex)
        {
            this.context = contex;
        }

        public async Task<Gift> AddGift(Gift gift)
        {
            try
            {
                await context.Gifts.AddAsync(gift);
                await context.SaveChangesAsync();
                return gift;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteGift(int id)
        {
            try
            {
                var d = await context.Gifts.FirstOrDefaultAsync(dd => dd.Id == id);
                if (d == null)
                {
                    throw new Exception($"Gift {id} not found");
                }
                var pur = await context.Purchases.FirstOrDefaultAsync(dd => dd.GiftId == d.Id);
                if (pur == null)
                {
                    context.Gifts.Remove(d);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw (new Exception("אי אפשר למחוק מתנה זו"));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Gift> FindByGiftName(string giftName)
        {
            try
            {
                return await context.Gifts.FirstOrDefaultAsync(g => g.Name == giftName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Gift>> FindByName(string name)
        {
            try
            {
                var gifts = await context.Gifts
                                 .Where(g => g.Donor.Name.Contains(name))
                                 .ToListAsync();

                return gifts;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Gift> FindByNumOfPurchase(int num)
        {
            try
            {
                var gift = await context.Gifts
                               .Where(g => g.NumOfPurchases == num)
                               .FirstOrDefaultAsync();

                return gift;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Gift>> GetGifts()
        {
            try
            {
                return await context.Gifts.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Gift> UpdateGift(Gift gift)
        {
            try
            {
                context.Entry(gift).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return gift;
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
    }
}
