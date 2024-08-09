using System.Collections.Concurrent;

namespace ProductsApi;

internal class ProductsService
{
    internal record Product(string Name, string Description, string Unit, double Price);

    private Product[] allProducts = [
            new Product("Hammer", "Exclusive steel hammer for breaking stuff", "€", 85.5),
            new Product("Screwdriver", "A nice little screwdriver to assemble your IKEA furniture", "€", 4.9),
            new Product("Shovel", "The thing nobody wants to work with", "€", 30),
        ];

    private readonly ConcurrentDictionary<string, Product> _orderedProducts = new ConcurrentDictionary<string, Product>();

    internal Product[] Get()
    {
        return allProducts;
    }

    internal bool Order(string name)
    {
        var product = allProducts.Where(p => p.Name == name).FirstOrDefault();
        if (product == null)
        {
            return false;
        }
        else
        {
            return _orderedProducts.TryAdd("user", product!);
        }
    }

    internal Product[] GetOrderedProducts()
    {
        return _orderedProducts.Values.ToArray();
    }
}