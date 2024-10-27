using Application.Comments;
using Application.Comments.Dto;
using Application.Interfaces.Contexts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IDataBaseContext dataBaseContext;
        private readonly IIdentityDatabaseContext identityDatabaseContext;
        private readonly IMapper mapper;
        private readonly ICommentsService commentService;

        public CommentsController(IDataBaseContext dataBaseContext
            , IIdentityDatabaseContext identityDatabaseContext, IMapper mapper, ICommentsService commentService)
        {
            this.dataBaseContext = dataBaseContext;
            this.identityDatabaseContext = identityDatabaseContext;
            this.mapper = mapper;
            this.commentService = commentService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = commentService.GetList(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{postId}")]
        public IActionResult Get(int postId)
        {
            var result = commentService.GetListWhithId(postId);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CommentDto commentDto)
        {
            var result = commentService.Add(commentDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(Get), "Comment", new { id = result.Data.Id }, Request.Scheme);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Put([FromBody] CommentDto commentDto)
        {
            var result = commentService.Edit(commentDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete()]
        public IActionResult Delete(int id)
        {
            var result = commentService.Remove(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
    }
}
