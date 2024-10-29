using Application.Categorys.CategoryTypes;
using Application.Categorys.CategoryTypes.Dtos;
using AutoMapper;
using Infrastructures.MappingProfile;
using UnitTest.Builders;

namespace UnitTest.Core.Application
{
    public class CategoryTypeServiceTest
    {
        [Trait("Service", "CategoryType")]
        [Fact]
        public void AddCategory_and_GetList_ShouldReturnPaginatedItems_WhenCalled()
        {
            //Arrange

            // Creating an instance of DatabaseBuilder to set up the database context
            var dataBasebuilder = new DatabaseBuilder();
            var dbContext = dataBasebuilder.GetDbContext(); // Getting the database context from the builder

            // Configuring the mapping profile for CategoryType
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoryMappingProfile());
            });
            var mapper = mockMapper.CreateMapper();

            // Creating an instance of the service that will be tested
            var service = new CategoryTypeService(dbContext, mapper);

            int pageIndex = 1;
            int pageSize = 5;

            service.Add(new CategoryTypeDto
            {
                Id = 1,
                Type = "t1"
            });
            service.Add(new CategoryTypeDto
            {
                Id = 2,
                Type = "t2"
            });

            //Act
            var result = service.GetList(null, pageIndex, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(2, result.Count);
        }
    }
}
