using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.Posts.GetPostPLP.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IGetPostPLPService getPostPLPService;
        private readonly IGetPostPDPService getPostPDPService;
        public PostController(IGetPostPLPService getPostPLPService, IGetPostPDPService getPostPDPService)
        {
            this.getPostPLPService = getPostPLPService;
            this.getPostPDPService = getPostPDPService;
        }

        /// <summary>
        /// You can have a list of posts with different types of filters
        /// </summary>
        /// <param name="requestDto">If you want to enter the desired filter</param>
        /// 
        /// <returns></returns>
        // GET: api/<PostController>
        [HttpGet]
        public IActionResult Get([FromQuery] PostPLPRequestDto requestDto)
        {
            return Ok(getPostPLPService.Execute(requestDto));
        }

        /// <summary>
        /// Find the post you want based on the ID
        /// </summary>
        /// <param name="id">Give the id of the desired post</param>
        /// <returns></returns>
        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(getPostPDPService.Execute(id));
        }
    }
}
