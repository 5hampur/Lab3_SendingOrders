namespace SendingApp.Models
{
    public interface IOrderObserver
    {
        void Update(Order order);
    }

    public class UserNotifier : IOrderObserver
    {
        public void Update(Order order)
        {
            Console.WriteLine($"Status update: {order.State.GetType().Name}");
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

}