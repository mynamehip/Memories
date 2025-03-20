using DiaryService.Data;
using DiaryService.DTO;
using DataAccess.Enum;
using DiaryService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public async Task<IActionResult> ReadAll()
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
        public async Task<IActionResult> ReadByList([FromQuery] List<Guid> listId)
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                await _context.RemoveAsync(id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("deletebylist")]
        public async Task<IActionResult> DeleteByList([FromBody] List<Guid> listId)
        {
            try
            {
                int count = await _context.RemoveAllByList(listId);
                return Ok(new { message = $"Deleted {count} records successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpatePost(Guid id, [FromBody] DiaryInsert diary)
        {
            try
            {
                await _context.EditAsync(id, diary);
                return Ok(new { message = "Updated successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
