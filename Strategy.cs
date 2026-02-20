using SendingApp.Models;
using System.Linq;

namespace SendingApp.Strategies
{
    public interface ICostStrategy
    {
        decimal CalculateCost(Order order);
    }




    public class PromotionalCostStrategy : ICostStrategy
    {
        public decimal CalculateCost(Order order)
        {
            decimal subtotal = order.Items.Sum(i => i.Price);
            decimal discount = subtotal * 0.15m;
            decimal tax = (subtotal - discount) * 0.1m;
            decimal SendingFee = 2m;
            decimal result_promotional = subtotal - discount + tax + SendingFee;
            
            return result_promotional;
        }
    }




    public class standartCostStrategy : ICostStrategy
    {
        public decimal CalculateCost(Order order)
        {
            decimal subtotal = order.Items.Sum(i => i.Price);
            decimal tax = subtotal * 0.1m;
            decimal SendingFee = 3m;
            decimal result_standart = subtotal + tax + SendingFee;

            return result_standart;
        }
    }
}