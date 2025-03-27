namespace InventoryManagementWithExpirationDatesSystem.DTOs
{
    public class ItemDTO
    {

        public int ItemId { get; set; } 
        public required string ItemName { get; set; }

       // public  string Category { get; set; }
        public required decimal UnitPrice { get; set; }

    }

    public class ItemInformationtThatTheUserNeedToSeeDTO
    {
        public required string ItemName { get; set; }
        public required decimal UnitPrice { get; set; }

    }
}
