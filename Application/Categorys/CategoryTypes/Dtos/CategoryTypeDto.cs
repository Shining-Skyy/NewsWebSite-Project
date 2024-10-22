namespace Application.Categorys.CategoryTypes.Dtos
{
    public class CategoryTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentTypeId { get; set; }
    }
}
