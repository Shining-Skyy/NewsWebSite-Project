using Application.Banners;
using Application.Banners.Dto;
using Application.UriComposer;
using Domain.Banners;
using UnitTest.Builders;

namespace UnitTest.Core.Application
{
    public class BannerServiceTest
    {
        [Trait("Service", "Banners")]
        [Fact]
        public void AddBanner_and_GetBanners_ReturnsPaginatedItemsDto()
        {
            // Arrange
            var dataBasebuilder = new DatabaseBuilder(); // Create an instance of DatabaseBuilder.
            var dbContext = dataBasebuilder.GetDbContext(); // Get the in-memory database context.

            var mockUriComposer = new UriComposerService(); // Create a mock service for URI composition.

            // Create an instance of BannersService with the database context and URI composer.
            var service = new BannersService(dbContext, mockUriComposer);

            int pageIndex = 1;
            int pageSize = 5;

            service.AddBanner(new BannerDto
            {
                Id = 1,
                Image = "image1.jpg",
                IsActive = false,
                Link = "http://link1.com",
                Name = "Banner 1",
                Position = Position.Line_1,
                Priority = 1,
            });
            service.AddBanner(new BannerDto
            {
                Id = 2,
                Image = "image2.jpg",
                IsActive = true,
                Link = "http://link2.com",
                Name = "Banner 2",
                Position = Position.Line_2,
                Priority = 2
            });

            // Act
            var result = service.GetBanners(pageIndex, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(2, result.Count);
        }
    }
}