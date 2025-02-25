//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using project.Models;
//using System.Drawing;
//using System.Linq;

//namespace project.DAL
//{
//    public class PurchaseDAL : IPurchaseDAL
//    {
//        private readonly Context context;
//        public PurchaseDAL(Context context)
//        {
//            this.context = context;
//        }

//        public async Task<Purchase> AddPurchase(Purchase purchase)
//        {
//            try
//            {
//                await context.Purchases.AddAsync(purchase);
//                await context.SaveChangesAsync();
//                return purchase;
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public async Task DeletePurchaseFromCart(int id)
//        {
//            try
//            {
//                var d = await context.Purchases.FirstOrDefaultAsync(dd => dd.Id == id);
//                if (d == null)
//                {
//                    throw new Exception($"Purchase {id} not found");
//                }
//                context.Purchases.Remove(d);
//                await context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public async Task<IEnumerable<Purchase>> GetAllPurchase()
//        {
//            //לא הצלחתי
//            try
//            {
//                return context.Purchases.Select(x => x).Where(x => x.Status == true).ToList();
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//        }

//        public async Task<IEnumerable<Purchase>> GetPurchaseByCustomer(int id)
//        {
//            try
//            {
//                return context.Purchases
//                          .Where(purchase => purchase.CustomerId == id)
//                          .ToList();
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//        }

//        public async Task<IEnumerable<Purchase>> GetPurchaseByGift(int id)
//        {
//            try
//            {
//                return context.Purchases
//                          .Where(purchase => purchase.GiftId == id)
//                          .ToList();
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//        }

//        public async Task<Purchase> Updatepurchase(Purchase purchase)
//        {
//            try
//            {
//                context.Purchases.Update(purchase);

//                await context.SaveChangesAsync();

//                return purchase;
//            }
//            catch (DbUpdateConcurrencyException ex)
//            {
//                throw new Exception("גרסה מיושנת של התורם. אנא נסה שוב.");
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("שגיאה בעדכון התורם: " + ex.Message);
//            }
//        }
//        public async Task<IEnumerable<Customer>> MakeRaffleByGift(int id)
//        {
//            var p = await GetPurchaseByGift(id);


//        }


//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL
{
    public class PurchaseDAL :
        IPurchaseDAL
    {
        private readonly Context context;
        public PurchaseDAL(Context context)
        {
            this.context = context;
        }

        public async Task<Purchase> AddPurchase(Purchase purchase)
        {
            try
            {
                await context.Purchases.AddAsync(purchase);
                await context.SaveChangesAsync();
                return purchase;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeletePurchaseFromCart(int id)
        {
            try
            {
                var d = await context.Purchases.FirstOrDefaultAsync(dd => dd.Id == id);
                if (d == null)
                {
                    throw new Exception($"Purchase {id} not found");
                }
                context.Purchases.Remove(d);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchase()
        {
            try
            {
                return await context.Purchases.Where(x => x.Status == true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseByCustomer(int id)
        {
            try
            {
                return await context.Purchases
                          .Where(purchase => purchase.CustomerId == id)
                          .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseByGift(int id)
        {
            try
            {
                return await context.Purchases
                          .Where(purchase => purchase.GiftId == id)
                          .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Purchase> Updatepurchase(Purchase purchase)
        {
            try
            {
                context.Purchases.Update(purchase);
                await context.SaveChangesAsync();
                return purchase;
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

        public async Task<Customer> MakeRaffleByGift(int giftId)
        {
            var purchases = await GetPurchaseByGift(giftId);

            // Filter purchases where the status is true
            var validPurchases = purchases.Where(p => p.Status == true).ToList();

            if (validPurchases == null || !validPurchases.Any())
            {
                throw new Exception("No valid purchases found for this gift.");
            }

            var random = new Random();
            var winnerIndex = random.Next(validPurchases.Count);
            var winningPurchase = validPurchases[winnerIndex];

            // Update the WinnerId in the gift
            var gift = await context.Gifts.FirstOrDefaultAsync(g => g.Id == giftId);
            if (gift == null)
            {
                throw new Exception("Gift not found.");
            }

            gift.WinnerId = winningPurchase.CustomerId;
            context.Gifts.Update(gift);
            await context.SaveChangesAsync();

            // Return the winning customer
            var winningCustomer = await context.Customers.FirstOrDefaultAsync(c => c.Id == winningPurchase.CustomerId);
            if (winningCustomer == null)
            {
                throw new Exception("Winning customer not found.");
            }

            return winningCustomer;
        }

        public Task DownloadWinners()
        {
            throw new NotImplementedException();
        }

        //public async Task DownloadWinners()
        //{
        //    // שליפת המתנות עם מזהי הזוכים
        //    var gifts = await context.Gifts
        //        .Select(g => new
        //        {
        //            GiftName = g.Name,
        //            WinnerId = g.WinnerId
        //        })
        //        .ToListAsync();

        //    // שליפת שמות הזוכים
        //    var winnerIds = gifts.Select(g => g.WinnerId).Where(id => id != null).ToList();
        //    var winners = await context.Customers
        //        .Where(c => winnerIds.Contains(c.Id))
        //        .Select(c => new
        //        {
        //            c.Id,
        //            c.Name
        //        })
        //        .ToListAsync();

        //    // שילוב הנתונים
        //    var giftWithWinnerNames = gifts.Select(g => new
        //    {
        //        g.GiftName,
        //        WinnerName = winners.FirstOrDefault(w => w.Id == g.WinnerId)?.Name ?? "No winner"
        //    }).ToList();

        //    // יצירת הקובץ והורדתו
        //    var sb = new StringBuilder();
        //    sb.AppendLine("GiftName,WinnerName");
        //    foreach (var item in giftWithWinnerNames)
        //    {
        //        sb.AppendLine($"{item.GiftName},{item.WinnerName}");
        //    }

        //    var fileName = "gifts_with_winners.csv";
        //    var filePath = Path.Combine("path_to_save", fileName);
        //    await File.WriteAllTextAsync(filePath, sb.ToString());

        //    // החזרת התוצאה כקובץ להורדה
        //    return File(System.IO.File.ReadAllBytes(filePath), "text/csv", fileName);

        //}
    }
}

