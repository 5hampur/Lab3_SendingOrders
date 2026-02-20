using SendingApp.Models;
using SendingApp.Strategies;

namespace SendingApp.Builders
{
    public class OrderBuilder
    {
        private Order order = new Order();

        public OrderBuilder AddMenuItem(MenuItem item)
        {
            order.AddItem(item);
            return this;
        }

        public OrderBuilder SetCostStrategy(ICostStrategy strategy)
        {
            order.CostStrategy = strategy;
            return this;
        }

        public Order Build()
        {
            order.State = new PreparingState();
            return order;
        }
    }
}