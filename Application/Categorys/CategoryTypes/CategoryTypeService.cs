﻿using Application.Categorys.CategoryTypes.Dtos;
using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Common;
using Domain.Categorys;

namespace Application.Categorys.CategoryTypes
{
    public class CategoryTypeService : ICategoryTypeService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public CategoryTypeService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public BaseDto<CategoryTypeDto> Add(CategoryTypeDto categoryType)
        {
            // Mapping the incoming DTO to the entity model
            var model = mapper.Map<CategoryType>(categoryType);

            context.CategoryTypes.Add(model);
            context.SaveChanges();

            return new BaseDto<CategoryTypeDto>(mapper.Map<CategoryTypeDto>(model), new List<string> { $"Category added successfully" }, true);
        }

        public BaseDto<CategoryTypeDto> Edit(CategoryTypeDto categoryType)
        {
            // Retrieving the existing category type from the database using its ID
            var data = context.CategoryTypes
                .SingleOrDefault(p => p.Id == categoryType.Id);

            mapper.Map(categoryType, data);
            context.SaveChanges();

            return new BaseDto<CategoryTypeDto>(mapper.Map<CategoryTypeDto>(data), new List<string> { $"Category edited successfully" }, true);
        }

        public BaseDto<CategoryTypeDto> FindById(int Id)
        {
            var data = context.CategoryTypes.Find(Id);

            // Mapping the found entity to the DTO
            var result = mapper.Map<CategoryTypeDto>(data);

            return new BaseDto<CategoryTypeDto>(result, null, true);
        }

        public PaginatedItemsDto<CategoryTypeListDto> GetList(int? parentId, int page, int pageSize)
        {
            int totalCount = 0;

            // Querying the category types based on the parent ID and applying pagination
            var data = context.CategoryTypes
                .Where(p => p.ParentTypeId == parentId)
                .PagedResult(page, pageSize, out totalCount);

            // Projecting the data to a list of DTOs
            var result = mapper.ProjectTo<CategoryTypeListDto>(data).ToList();

            return new PaginatedItemsDto<CategoryTypeListDto>(page, pageSize, totalCount, result);
        }

        public BaseDto Remove(int Id)
        {
            var data = context.CategoryTypes.Find(Id);

            context.CategoryTypes.Remove(data);
            context.SaveChanges();

            return new BaseDto(new List<string> { $"Category removed successfully" }, true);
        }
    }
}
