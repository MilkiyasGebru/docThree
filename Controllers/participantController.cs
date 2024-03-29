﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Models;

namespace QuizApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public ParticipantController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: api/participant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<participant>>> GetParticipants()
        {
            return await _context.Participants.ToListAsync();
        }

        // GET: api/participant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<participant>> Getparticipant(int id)
        {
            var participant = await _context.Participants.FindAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            return participant;
        }

        // PUT: api/participant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putparticipant(int id, ParticipantResult _participantResult)
        {
            if (id != _participantResult.ParticipantId)
                return BadRequest();

            participant participant = (await _context.Participants.FindAsync(id))!;
            participant!.Score = _participantResult.Score;
            participant.TimeTaken = _participantResult.TimeTaken;
            
            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!participantExists(id))
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

        // POST: api/participant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<participant>> Postparticipant(participant participant)
        {
            var temp = _context.Participants.FirstOrDefault(x => x.Name == participant.Name
                                                                 && x.Email == participant.Email);
            if (temp == null)
            {
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();

            }
            else participant = temp;

            return Ok(participant);
        }

        // DELETE: api/participant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteparticipant(int id)
        {
            var participant = await _context.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool participantExists(int id)
        {
            return (_context.Participants?.Any(e => e.ParticipantId == id)).GetValueOrDefault();
        }
    }
}
