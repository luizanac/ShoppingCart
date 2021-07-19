using System.Collections.Generic;
using System.Linq;
using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart
{
    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> items = new();

        public int UserId { get; }
        public IEnumerable<ShoppingCartItem> Items => items;

        public ShoppingCart(int userId)
        {
            UserId = userId;
        }

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach (var item in shoppingCartItems)
                items.Add(item);

            eventStore.Raise("ShoppingCartItemsAdded", new { UserId, Items = shoppingCartItems });
        }

        public void RemoveItems(int[] productCatalogueIds, IEventStore eventStore)
        {
            items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogueId));
            eventStore.Raise("ShoppingCartItemsRemoved", new
            {
                UserId,
                ProductIds = productCatalogueIds
            });
        }
    }

    public record ShoppingCartItem(
        int ProductCatalogueId,
        string ProductName,
        string Description,
        Money Price)
    {
        public virtual bool Equals(ShoppingCartItem obj) =>
            obj != null &&
            ProductCatalogueId.Equals(obj.ProductCatalogueId);

        public override int GetHashCode() =>
            ProductCatalogueId.GetHashCode();
    }

    public record Money(string Currency, decimal Amount);
}