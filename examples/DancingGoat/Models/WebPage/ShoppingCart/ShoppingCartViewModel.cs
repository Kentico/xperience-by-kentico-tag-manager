namespace DancingGoat.Models;

public record ShoppingCartViewModel(ICollection<ShoppingCartItemViewModel> Items, decimal TotalPrice);
