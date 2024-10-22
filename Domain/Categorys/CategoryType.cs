using Domain.Attributes;

namespace Domain.Categorys
{
    [Auditable]
    public class CategoryType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public int? ParentTypeId { get; set; }
        public CategoryType ParentType { get; set; }
        public ICollection<CategoryType> SubType { get; set; }
    }
}
