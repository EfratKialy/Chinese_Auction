using Microsoft.AspNetCore.Mvc;
using project.DAL;
using project.Models;
using project.Models.DTO;

namespace project.BLL
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseDAL purchaseDal;
        public PurchaseService(IPurchaseDAL purchaseDal) 
        {
            this.purchaseDal = purchaseDal;
        }

        public  async Task<Purchase> AddPurchase(Purchase purchase)
        {
            return await purchaseDal.AddPurchase(purchase);
        }

        public async  Task DeletePurchaseFromCart(int id)
        {
            await purchaseDal.DeletePurchaseFromCart(id);
        }

        public async Task DownloadWinners()
        {
            await purchaseDal.DownloadWinners();
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchase()
        {
            return await purchaseDal.GetAllPurchase();
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseByCustomer(int id)
        {
            return await purchaseDal.GetPurchaseByCustomer(id);
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseByGift(int id)
        {
            return await purchaseDal.GetPurchaseByGift(id);
        }

        public async Task<Customer> MakeRaffleByGift(int id)
        {
            return await purchaseDal.MakeRaffleByGift(id);
        }

        public async Task<Purchase> Updatepurchase(Purchase purchase, int purchaseId)
        {
            purchase.Id = purchaseId;
            return await purchaseDal.Updatepurchase(purchase);
        }
    }
}
