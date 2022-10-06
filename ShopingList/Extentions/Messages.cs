namespace ShopingList.Extentions
{
    public static class Messages
    {
        public static string ProductExists = "Product {0} already exists in the same category.";
        public static string ProductEdited = "Product {0} was edited.";
        public static string ProductAdded = "Product {0} was added.";
        public static string ProductNotFound = "Product do not exists. ID: {0}";
        public static string ProductDeleted = "Product {0} was deleted.";

        public static string CategoryExists = "Category {0} already exists.";
        public static string CategoryNotFound = "Category do not exists. ID: {0}";
        public static string CategoryEdited = "Category {0} was edeted.";
        public static string CategoryCantDeleted = "Тhe category cannot be deleted. There are some products with this category.";
        public static string CategoryDeleted = "Category {0} was deleted.";
        
        public static string ShopingListNotFound = "Shoping list do not exists. ID: {0}";
        public static string ShopingListCreated = "Shoping list {0} was created.";
        public static string ShopingListEdited = "Shoping list {0} was edited.";
        public static string ShopingListDeleted = "Shoping list  {0} was deleted.";
        public static string ProductNotExistsInShopingList = "There is not such product in shoping list. ID: {0}";
        
        public static string NotAuthorize = "You are not authorize for this operation.";
        
        public static string ExeptionMessage = "ExeptionMessage";
        public static string ExeptionInnerMessage = "ExeptionInnerMessage";
        
        public static string AlertMsg = "AlertMsg";
        public static string AlertMsgEdit = "AlertMsgEdit";
        public static string AlertMsgError = "AlertMsgError";
        public static string AlertMsgRow = "AlertMsgRow";
        
        public static string CategoryId = "CategoryId";
        public static string ChooseProductCategory = "Choose product category...";
        public static string LogUsername = "Username: {0}";
    }
}
