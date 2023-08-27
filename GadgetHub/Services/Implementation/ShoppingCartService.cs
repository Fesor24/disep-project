using Azure.Core;
using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using GadgetHub.Exceptions;
using GadgetHub.Services.Abstractions;

namespace GadgetHub.Services.Implementation;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ShoppingCart> AddItemToCart(ShoppingCart shoppingCart,int productId,
        string basePath, int quantity = 1)
    {
        string cartId = Guid.NewGuid().ToString();

        if (shoppingCart is null)
        {
            shoppingCart = new ShoppingCart(cartId);
        }

        var product = await _unitOfWork.ProductRepository.GetById(productId);

        if (product is null)
        {
            throw new NotFoundException($"Product with {productId} not found");
        }

        ShoppingCartItem cartItem = MapProductToShoppingCartItem(product, basePath, quantity);

        shoppingCart = AddOrUpdateShoppingCartItem(shoppingCart, cartItem, quantity);

        return shoppingCart;
    }

    public async Task<ShoppingCart> DecrementItemFromCart(ShoppingCart cart,int productId, string basePath)
    {
        var product = await _unitOfWork.ProductRepository.GetById(productId);

        if (product is null)
        {
            throw new NotFoundException($"Product with {productId} not found");
        }

        string cartId = Guid.NewGuid().ToString();

        if (cart is null)
        {
            cart = new ShoppingCart(cartId);
        }

        ShoppingCartItem shoppingCartItem = MapProductToShoppingCartItem(product, basePath);

        cart = DecrementOrRemoveItemFromCart(cart, shoppingCartItem);

        return cart;
    }

    public async Task<ShoppingCart> DeleteItemFromCart(ShoppingCart cart, int productId, string basePath)
    {
        var product = await _unitOfWork.ProductRepository.GetById(productId);

        if (product is null)
        {
            throw new NotFoundException($"Product with {productId} not found");
        }

        if (cart is null)
        {
            cart = new ShoppingCart(Guid.NewGuid().ToString());
        }

        ShoppingCartItem shoppingCartItem = MapProductToShoppingCartItem(product, basePath);

        cart.Items = cart.Items.Where(x => x.Id != shoppingCartItem.Id).ToList();

        return cart;
    }

    public Task DeleteShoppingCart()
    {
        throw new NotImplementedException();
    }

    private ShoppingCart AddOrUpdateShoppingCartItem(ShoppingCart cart, ShoppingCartItem item, int quantity = 1)
    {
        var cartItem = cart.Items.Find(x => x.Id == item.Id);

        if (cartItem is null)
        {
            cart.Items.Add(item);

            return cart;
        }

        cart.Items.First(x => x.Id == item.Id).Quantity += quantity;

        return cart;
    }

    private ShoppingCart DecrementOrRemoveItemFromCart(ShoppingCart cart, ShoppingCartItem item)
    {
        var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);

        if (cartItem is not null)
        {
            if (cartItem.Quantity > 1)
            {
                cart.Items.FirstOrDefault(x => x.Id == item.Id).Quantity--;

                return cart;
            }
            else
            {
                cart.Items.Remove(cartItem);

                return cart;
            }
        }

        return cart;
    }

    private ShoppingCartItem MapProductToShoppingCartItem(Product product, string basePath, int quantity = 1)
    {
        ShoppingCartItem cartItem = new()
        {
            Id = product.Id,
            Name = product.Name,
            Image = basePath + product.Image.TrimStart('~'),
            Price = product.Price,
            CategoryId = product.CategoryId,
            Quantity = quantity
        };

        return cartItem;
    }
}
