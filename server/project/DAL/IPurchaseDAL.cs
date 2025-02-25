using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Models.DTO;

namespace project.DAL
{
    public interface IPurchaseDAL
    {
        Task<IEnumerable<Purchase>> GetPurchaseByGift(int id);
        Task<Purchase> AddPurchase(Purchase purchase);
        Task<IEnumerable<Purchase>> GetAllPurchase();
        Task<IEnumerable<Purchase>> GetPurchaseByCustomer(int id);
        Task DeletePurchaseFromCart(int id);
        Task<Purchase> Updatepurchase(Purchase purchase);
        Task<Customer> MakeRaffleByGift(int id);
        Task DownloadWinners();



    }
}
