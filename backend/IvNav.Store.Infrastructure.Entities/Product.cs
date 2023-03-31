namespace IvNav.Store.Infrastructure.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;


    public Product(string name)
    {
        Name = name;
    }

    private Product()
    {

    }
}
