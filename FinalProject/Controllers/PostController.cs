using AutoMapper;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPost>>> GetPosts(int productId)
        {
            var query = await _postRepository.GetAllPosts(productId);

            if (query == null)
            {
                return NotFound();
            }

            List<GetPost> posts = _mapper.Map<List<GetPost>>(query);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<Status>> Add([FromBody] CreatePost postVM)
        {
            Status status = new Status();

            if (!ModelState.IsValid)
            {
                status.IsSuccess = false;
                status.Message = "invalid model";
                return BadRequest(status);
            }
            else
            {
                Post post = _mapper.Map<Post>(postVM);
                await _postRepository.Add(post);
                status.IsSuccess = true;
                status.Message = "create succesfully";
                return Ok(status);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Status>> Update([FromBody] UpdatePost postVM)
        {
            Status status = new Status();

            if (!ModelState.IsValid)
            {
                status.IsSuccess = false;
                status.Message = "invalid model";
                return BadRequest(status);
            }
            else
            {
                var post = await _postRepository.GetById(postVM.Id);
                if (post == null)
                {
                    status.IsSuccess = false;
                    status.Message = $"this post doesn't exist with id: {postVM.Id}";
                    return BadRequest(status);
                }
                else
                {
                    _mapper.Map(postVM, post);
                    await _postRepository.Update(post);
                    status.IsSuccess = true;
                    status.Message = "update succesfully";
                    return Ok(status);
                }
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery(Name ="id")] int id)
        {
            var post = await _postRepository.GetById(id);
            if(post == null)
            {
                return NotFound();
            }
            await _postRepository.Delete(post);
            return NoContent();
        }

    }
}
