using Application.Banners;
using Moq;
using Management.Pages.Banners;
using Application.Dtos;
using Application.Banners.Dto;

namespace UnitTest.Presentation.Management
{
    public class BannerPageTest
    {
        public PaginatedItemsDto<BannerDto> Dto;

        [Fact]
        public void OnGet_ShouldCallGetBanners_WithDefaultParameters()
        {
            // Arrange
            var mockBannerService = new Mock<IBannersService>(); // Creating a mock of the IBannersService
            IndexModel model = new IndexModel(mockBannerService.Object); // Initializing the IndexModel with the mocked service

            int PageIndex = 1;
            int PageSize = 5;
            var expectedBanners = Dto;

            // Setting up the mock to return expected banners when GetBanners is called with specific parameters
            mockBannerService.Setup(service => service.GetBanners(PageIndex, PageSize)).Returns(expectedBanners);

            // Act
            model.OnGet();

            // Assert

            // Ensuring that GetBanners was called exactly once with the specified parameters
            mockBannerService.Verify(service => service.GetBanners(PageIndex, PageSize), Times.Once);

            // Asserting that the Dto property of the model contains the expected banners
            Assert.Equal(expectedBanners, model.Dto);
        }
    }
}
