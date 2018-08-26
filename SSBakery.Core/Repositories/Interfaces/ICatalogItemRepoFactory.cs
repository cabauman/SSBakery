namespace SSBakery.Repositories.Interfaces
{
    public interface ICatalogItemRepoFactory
    {
        ICatalogItemRepo Create(string catalogCategoryId);
    }
}
