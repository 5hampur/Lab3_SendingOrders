using SendingApp.Builders;
using SendingApp.Decorators;
using SendingApp.Models;
using SendingApp.Strategies;
using System;

namespace SendingApp
{
    class Program
    {
        static void Main()
        {
            // создание заказа и выбор блюд
            var burger = new MenuItem("Burger", 12);
            var pasta = new MenuItem("Pasta", 15);
            var pizza = new MenuItem("Pizza", 20);

            var builder = new OrderBuilder()
                .AddMenuItem(burger)
                .AddMenuItem(pasta)
                .SetCostStrategy(new standartCostStrategy());

            Order standartOrder = builder.Build();
            standartOrder.AttachObserver(new UserNotifier());

            Console.WriteLine("Standart");
            Console.WriteLine($"Total amount to be paid: {standartOrder.GetTotalCost()} dollars");
            standartOrder.State.Next(standartOrder);
            standartOrder.State.Next(standartOrder);
            standartOrder.State.Next(standartOrder);
            Console.WriteLine();

            // особые условия
            var specialBuilder = new OrderBuilder()
                .AddMenuItem(pizza)
                .SetCostStrategy(new PromotionalCostStrategy());

            Order specialOrder = specialBuilder.Build();
            specialOrder.AttachObserver(new UserNotifier());

            // декораторы
            specialOrder = new ExpressSendingDecorator(specialOrder);
            specialOrder = new DiscountDecorator(specialOrder, 10); // скидка 10 проц

            Console.WriteLine("special order (fast sending and -10%)");
            Console.WriteLine($"Total amount to be paid: {specialOrder.GetTotalCost()} dollars");
            specialOrder.State.Next(specialOrder);
            specialOrder.State.Next(specialOrder);
            specialOrder.State.Next(specialOrder);
            Console.WriteLine();

            // Отслеживание состояния
            var trackingBuilder = new OrderBuilder()
                .AddMenuItem(burger)
                .SetCostStrategy(new standartCostStrategy());

            Order trackingOrder = trackingBuilder.Build();
            trackingOrder.AttachObserver(new UserNotifier());

            Console.WriteLine("checking the status tracking");
            Console.WriteLine($"current tracking: {trackingOrder.State.GetType().Name}");
            trackingOrder.State.Next(trackingOrder);
            trackingOrder.State.Next(trackingOrder);
            Console.WriteLine();

            // скидки, налоги, доставка
            var priceTestBuilder = new OrderBuilder()
                .AddMenuItem(pasta)
                .AddMenuItem(pizza)
                .SetCostStrategy(new PromotionalCostStrategy());

            Order priceTestOrder = priceTestBuilder.Build();
            priceTestOrder = new DiscountDecorator(priceTestOrder, 15); // скидка 15 проц
            priceTestOrder = new ExpressSendingDecorator(priceTestOrder);

            Console.WriteLine("Checking the cost calculation with different factors");
            Console.WriteLine($"Total amount to be paid: {priceTestOrder.GetTotalCost()} dollars ");
            Console.WriteLine();
        }
    }
}