namespace ThePuppyExchange.Models
{
    public class Checkout
    {
        //Cart info
        public int id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }

        //Puppy info
        public string name { get; set; }
        public string breed { get; set; }
        public int fee { get; set; }
        public string profile_pic { get; set; }
    }
}
