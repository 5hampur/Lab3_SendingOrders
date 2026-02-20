using SendingApp.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace SendingApp.Models
{
    public class Order
    {
        public List<MenuItem> Items {get;} = new List<MenuItem>();
        public virtual OrderState State {get; set;}
        public ICostStrategy CostStrategy {get; set;}
        private List<IOrderObserver> observers = new List<IOrderObserver>();

        public virtual void AddItem(MenuItem item)
        {
            Items.Add(item);
        }

        public virtual void AttachObserver(IOrderObserver observer)
        {
            observers.Add(observer);
        }

        public void NotifyObservers()
        {
            observers.ForEach(o => o.Update(this));
        }

        public virtual decimal GetTotalCost()
        {
            if (CostStrategy == null)
            {
                throw new InvalidOperationException("The cost calculation strategy has not been established");
            }

            return CostStrategy.CalculateCost(this);

        }

        public virtual void ChangeState(OrderState newState)
        {
            State = newState;
            NotifyObservers();
        }
    }








    public abstract class OrderState
    {
        public abstract void Next(Order order);
    }

    public class PreparingState : OrderState
    {
        public override void Next(Order order) => order.ChangeState(new DeliveringState());
    }

    public class DeliveringState : OrderState
    {
        public override void Next(Order order) => order.ChangeState(new CompletedState());
    }

    public class CompletedState : OrderState
    {
        public override void Next(Order order)
        {
            Console.WriteLine("Заказ уже выполнен");
        }
    }
}