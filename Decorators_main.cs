using SendingApp.Models;

namespace SendingApp.Decorators
{
    public abstract class OrderDecorator : Order
    {
        protected Order innerOrder;

        protected OrderDecorator(Order order)
        {
            innerOrder = order;
        }

        public override decimal GetTotalCost()
        {
            return innerOrder.GetTotalCost();
        }

        public override void AddItem(MenuItem item)
        {
            innerOrder.AddItem(item);
        }

        public override void AttachObserver(IOrderObserver observer)
        {
            innerOrder.AttachObserver(observer);
        }

        public override void ChangeState(OrderState newState)
        {
            innerOrder.ChangeState(newState);
        }

        public override OrderState State
        {
            get => innerOrder.State;
            set => innerOrder.State = value;
        }
    }
}