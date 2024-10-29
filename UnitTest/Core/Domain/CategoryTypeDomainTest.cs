using Domain.Categorys;

namespace UnitTest.Core.Domain
{
    public class CategoryTypeDomainTest
    {
        [Fact]
        public void CategoryType_Should_Have_Properties()
        {
            // Arrange
            var categoryType = new CategoryType();

            // Act
            categoryType.Id = 1;
            categoryType.Type = "Sample Type";
            categoryType.ParentTypeId = null;
            categoryType.ParentType = null; 
            categoryType.SubType = new List<CategoryType>();

            // Assert
            Assert.Equal(1, categoryType.Id);
            Assert.Equal("Sample Type", categoryType.Type);
            Assert.Null(categoryType.ParentTypeId);
            Assert.Null(categoryType.ParentType);
            Assert.NotNull(categoryType.SubType);
        }

        [Fact]
        public void CategoryType_SubType_Should_Be_Initialized()
        {
            // Arrange
            var categoryType = new CategoryType();

            // Act
            categoryType.SubType = new List<CategoryType>();

            // Assert
            Assert.NotNull(categoryType.SubType);
            Assert.Empty(categoryType.SubType);
        }

        [Fact]
        public void CategoryType_Should_Allow_ParentType()
        {
            // Arrange
            var parentCategory = new CategoryType { Id = 2, Type = "Parent Type" };
            var childCategory = new CategoryType { Id = 1, Type = "Child Type", ParentType = parentCategory };

            // Act & Assert
            Assert.Equal(parentCategory, childCategory.ParentType);
        }
    }
}
