﻿using Application.HomePageService.Dto;
using Application.Interfaces.Contexts;
using Application.Posts.GetPostPLP;
using Application.Posts.GetPostPLP.Dto;
using Application.UriComposer;

namespace Application.HomePageService
{
    public class HomePageService : IHomePageService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposerService;
        private readonly IGetPostPLPService getPostPLPService;
        public HomePageService(IDataBaseContext context, IUriComposerService uriComposerService, IGetPostPLPService getPostPLPService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
            this.getPostPLPService = getPostPLPService;
        }

        public HomePageDto GetData()
        {
            // Retrieve active banners from the database
            var banners = context.Banners
                .Where(p => p.IsActive == true)
                .OrderBy(p => p.Priority)
                .ThenByDescending(p => p.Id)
                .Select(p => new BannerDto
                {
                    Id = p.Id,
                    Image = uriComposerService.ComposeImageUri(p.Image),
                    Link = p.Link,
                    Position = p.Position,
                })
                .ToList();

            // Fetch the most visited posts with pagination
            var MostVisited = getPostPLPService.Execute(new PostPLPRequestDto
            {
                pageIndex = 1,
                pageSize = 20,
                SortType = SortType.MostVisited
            }).Data
            .ToList();

            // Fetch the most popular posts with pagination
            var MostPopular = getPostPLPService.Execute(new PostPLPRequestDto
            {
                pageIndex = 1,
                pageSize = 20,
                SortType = SortType.MostPopular
            }).Data
            .ToList();

            // Fetch the newest posts with pagination
            var newest = getPostPLPService.Execute(new PostPLPRequestDto
            {
                pageIndex = 1,
                pageSize = 20,
                SortType = SortType.newest
            }).Data
            .ToList();

            return new HomePageDto
            {
                Banners = banners,
                MostVisited = MostVisited,
                MostPopular = MostPopular,
                newest = newest
            };
        }
    }
}
