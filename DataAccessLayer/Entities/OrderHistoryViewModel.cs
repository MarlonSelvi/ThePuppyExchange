namespace DataAccessLayer.Entities
{
    public class OrderHistoryViewModel
    {
        public int order_id { get; set; }
        public DateTime date { get; set; }
        public List<OrderHistoryItemViewModel> items { get; set; }
        public decimal total => items?.Sum(i => i.fee * i.quantity) ?? 0;
    }

    public class OrderHistoryItemViewModel
    {
        public string name { get; set; }
        public string breed { get; set; }
        public decimal fee { get; set; }
        public int quantity { get; set; }
        public string profile_pic { get; set; }
    }
}