namespace NetEnhancements.EntityFramework.Query
{
    public enum OrderDirection
    {
        Ascending,
        Descending,
    }

    public class OrderExpression
    {
        public string Order { get; }

        public OrderDirection Direction { get; }

        public OrderExpression(string order, OrderDirection direction = OrderDirection.Ascending)
        {
            Order = order;
            Direction = direction;
        }

        public static implicit operator OrderExpression(string order)
        {
            return new OrderExpression(order);
        }
    }
}
