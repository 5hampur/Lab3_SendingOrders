using SendingApp.Models;

namespace SendingApp.Decorators
{
    public class DiscountDecorator : OrderDecorator
    {
        private decimal discountPercent;

        public DiscountDecorator(Order order, decimal discountPercent) : base(order)
        {
            this.discountPercent = discountPercent;
        }

        public override decimal GetTotalCost()
        {
            decimal cost = base.GetTotalCost();
            decimal result_Discount = cost - cost * discountPercent / 100m;
            return result_Discount;
        }
    }



    public class ExpressSendingDecorator : OrderDecorator
    {
        public ExpressSendingDecorator(Order order) : base(order) {}

        public override decimal GetTotalCost()
        {
            return base.GetTotalCost() + 5m;
        }
    }

}