namespace DancingGoat.Models;

public record ProductSectionListViewModel(string Title, IEnumerable<ProductListItemViewModel> Items)
{
}
