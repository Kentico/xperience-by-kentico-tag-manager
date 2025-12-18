using CMS.ContentEngine;

namespace DancingGoat.Models;

public record TaxonomyViewModel(string Name, string CodeName, List<TagViewModel> Tags)
{
    /// <summary>
    /// Maps <see cref="TaxonomyData"/> to a <see cref="TaxonomyViewModel"/>.
    /// </summary>
    public static TaxonomyViewModel GetViewModel(TaxonomyData taxonomy) => new TaxonomyViewModel(taxonomy.Taxonomy.Title, taxonomy.Taxonomy.Name, TagViewModel.GetViewModels(taxonomy.Tags));


    /// <summary>
    /// Gets selected tags.
    /// </summary>
    public async Task<TagCollection> GetSelectedTags()
    {
        if (Tags == null)
        {
            return null;
        }

        return await TagCollection.Create(Tags.Where(tag => tag.IsChecked).Select(tag => tag.Value));
    }
}
