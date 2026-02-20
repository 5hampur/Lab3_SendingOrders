using Xunit;
using DeliveryApp.Models;
using DeliveryApp.Builders;
using DeliveryApp.Strategies;
using DeliveryApp.Decorators;

namespace DeliveryApp.Tests
{
    public class OrderTests
    {
        [Fact]
        public void OrderSelection_Test()
        {
            // Создание заказа и выбор блюд
            var burger = new MenuItem("Бургер", 10);
            var pasta = new MenuItem("Паста", 12);

            var orderBuilder = new OrderBuilder().AddMenuItem(burger).AddMenuItem(pasta);

            var order = orderBuilder.Build();

            Assert.Equal(2, order.Items.Count);
            Assert.Contains(burger, order.Items);
            Assert.Contains(pasta, order.Items);
        }

        [Fact]
        public void SpecialConditionsOrder_Test()
        {
            // Заказы с особыми условиями
            var pizza = new MenuItem("Пицца", 15);

            var orderBuilder =
                new OrderBuilder().AddMenuItem(pizza).SetCostStrategy(new StandardCostStrategy());

            Order order = orderBuilder.Build();
            order = new ExpressDeliveryDecorator(order);
            order = new DiscountDecorator(order, 10);

            decimal expectedCost = ((15 + 1.5m + 3 + 5) * 0.9m);
            Assert.Equal(expectedCost, order.GetTotalCost());
        }

        [Fact]
        public void OrderStateTracking_Test()
        {
            // Отслеживание состояний заказа
            var order = new OrderBuilder().Build();
            order.State = new PreparingState();

            Assert.IsType<PreparingState>(order.State);

            order.State.Next(order);
            Assert.IsType<DeliveringState>(order.State);

            order.State.Next(order);
            Assert.IsType<CompletedState>(order.State);
        }

        [Fact]
        public void CostCalculationWithDifferentFactors_Test()
        {
            // Расчёт стоимости с разными факторами
            var pizza = new MenuItem("Пицца", 20);
            var pasta = new MenuItem("Паста", 10);

            var order = new OrderBuilder()
                .AddMenuItem(pizza)
                .AddMenuItem(pasta)
                .SetCostStrategy(new PromotionalCostStrategy())
                .Build();

            order = new ExpressDeliveryDecorator(order);
            order = new DiscountDecorator(order, 20); // 20% скидка

            decimal subtotal = 30;
            decimal discountPromo = subtotal * 0.15m;
            decimal subtotalAfterPromo = subtotal - discountPromo;
            decimal tax = subtotalAfterPromo * 0.1m;
            decimal deliveryPromo = 2m; 
            decimal expressFee = 5m;
            decimal totalBeforeDiscountDecorator = subtotalAfterPromo + tax + deliveryPromo + expressFee;
            decimal totalAfterDiscountDecorator = totalBeforeDiscountDecorator * 0.8m;

            Assert.Equal(totalAfterDiscountDecorator, order.GetTotalCost());
        }

        
        [Fact]
        public void ScalabilityCheck_Test()
        {
            // Расширение новыми условиями и стратегиями
            var sushi = new MenuItem("Суши", 25);
            var order =
                new OrderBuilder()
                .AddMenuItem(sushi).SetCostStrategy(new PromotionalCostStrategy()).Build();

            // Проверка, что можно легко сменить стратегию на другую
            order.CostStrategy = new StandardCostStrategy();
            decimal costStandard = order.GetTotalCost();

            order.CostStrategy = new PromotionalCostStrategy();
            decimal costPromo = order.GetTotalCost();

            Assert.NotEqual(costStandard, costPromo);
        }
    }
}