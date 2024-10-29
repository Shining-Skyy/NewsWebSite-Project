using Application.Categorys.CategoryTypes;
using Application.Categorys.CategoryTypes.Dtos;
using Application.Dtos;
using Management.Pages.Category.CategoryType;
using Moq;

namespace UnitTest.Presentation.Management
{
    public class CategoryTypePageTest
    {
        public PaginatedItemsDto<CategoryTypeListDto> Dto;

        [Fact]
        public void OnGet_ShouldCallGetCategory_WithDefaultParameters()
        {
            // Arrange
            var moqCategoryTypeService = new Mock<ICategoryTypeService>(); // Creating a mock of the ICategoryTypeService
            IndexModel model = new IndexModel(moqCategoryTypeService.Object); // Initializing the IndexModel with the mocked service

            int PageIndex = 1;
            int PageSize = 5;
            var expectedBanners = Dto;

            // Setting up the mock to return expectedBanners when GetList is called with specific parameters
            moqCategoryTypeService.Setup(service => service.GetList(null, PageIndex, PageSize)).Returns(expectedBanners);

            // Act
            model.OnGet(null);

            // Assert

            // Verifying that GetList was called exactly once with the specified parameters
            moqCategoryTypeService.Verify(service => service.GetList(null, PageIndex, PageSize), Times.Once);

            // Asserting that the catalogType in the model matches the expected result
            Assert.Equal(expectedBanners, model.catalogType);
        }
    }
}
