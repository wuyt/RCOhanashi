using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCOhanashi.Data;
using RCOhanashi.Models;

namespace RCOhanashi.Controllers
{
    [Produces("application/json")]
    [Route("api/Animes")]
    public class AnimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Animes
        [Authorize]
        [HttpGet]
        public IEnumerable<Anime> GetAnime()
        {
            return _context.Anime;
        }

        // GET: api/Animes/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnime([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anime = await _context.Anime.SingleOrDefaultAsync(m => m.ID == id);

            if (anime == null)
            {
                return NotFound();
            }

            return Ok(anime);
        }

        // PUT: api/Animes/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnime([FromRoute] int id, [FromBody] Anime anime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != anime.ID)
            {
                return BadRequest();
            }

            _context.Entry(anime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Animes
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAnime([FromBody] Anime anime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Anime.Add(anime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnime", new { id = anime.ID }, anime);
        }

        // DELETE: api/Animes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnime([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anime = await _context.Anime.SingleOrDefaultAsync(m => m.ID == id);
            if (anime == null)
            {
                return NotFound();
            }

            _context.Anime.Remove(anime);
            await _context.SaveChangesAsync();

            return Ok(anime);
        }

        private bool AnimeExists(int id)
        {
            return _context.Anime.Any(e => e.ID == id);
        }
    }
}