﻿namespace Application.Posts.GetPostPLP.Dto
{
    public class PostPLPRequestDto
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 2;
        public int? CategoryTypeId { get; set; }
        public string? SearchKey { get; set; }
        public SortType SortType { get; set; }
    }

    public enum SortType
    {
        None = 0,
        MostVisited = 1,
        MostPopular = 2,
        newest = 3,
    }
}
