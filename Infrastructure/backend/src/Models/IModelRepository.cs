using Infrastructure.Models;
namespace Infrastructure.Models
{
    public interface IModelRepository
    {
        public ModelRepositorySession OpenSession();
        public ModelRepositoryReadOnlySession OpenReadOnlySession();
    }
}