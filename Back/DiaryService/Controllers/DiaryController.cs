using DiaryService.Data;
using DiaryService.DTO;
using DiaryService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace DiaryService.Controllers
{
    [Route("api/diary")]
    [ApiController]
    public class DiaryController : ControllerBase
    {
        private readonly DiaryDbContext _context;
        public DiaryController(DiaryDbContext context)
        {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.GetAllPostsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getbylist")]
        public async Task<IActionResult> GetByList([FromQuery] List<Guid> listId)
        {
            try
            {
                var list = await _context.GetAllByListAsync(listId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] DiaryInsert diary)
        {
            try
            {
                await _context.AddAsync(diary);
                return Ok(new { message = "Bài viết đã được lưu!" });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
