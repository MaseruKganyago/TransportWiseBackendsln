using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstProject.Data;
using Microsoft.AspNetCore.Authorization;
using FirstProject.Domain;
using AutoMapper;
using FirstProject.Models.DTOs;
using Microsoft.AspNet.Identity;

namespace FirstProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[System.Web.Http.HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
	public class FuelWisesController : ControllerBase
	{
		private readonly MyDBContext _context;
		private readonly IMapper _mapper;

		public FuelWisesController(MyDBContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// GET: api/FuelWises
		[HttpGet]
		[System.Web.Http.HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
		public IEnumerable<FuelWiseDTO> GetFuelWise()
		{
			var tips = _context.FuelWise.ToList();
			var tipsDTO = _mapper.Map<List<FuelWiseDTO>>(tips);

			return tipsDTO;
		}

		// GET: api/FuelWises/5
		[HttpGet("{id}")]
		[System.Web.Http.HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
		public async Task<ActionResult<FuelWiseDTO>> GetFuelWise([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tip = await _context.FuelWise.FindAsync(id);
			var tipDTO = _mapper.Map<FuelWiseDTO>(tip);

            if (tipDTO == null)
            {
                return NotFound();
            }

            return Ok(tipDTO);
        }

        // PUT: api/FuelWises/5
        [HttpPut("{id}")]
		[System.Web.Http.HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
		public async Task<IActionResult> PutFuelWise([FromRoute] Guid id, [FromBody] FuelWise fuelWise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fuelWise.Id)
            {
                return BadRequest();
            }

            _context.Entry(fuelWise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuelWiseExists(id))
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

        // POST: api/FuelWises
        [HttpPost]
		[Authorize]
        public async Task<IActionResult> PostFuelWise([FromBody] FuelWise fuelWise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FuelWise.Add(fuelWise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFuelWise", new { id = fuelWise.Id }, fuelWise);
        }

        // DELETE: api/FuelWises/5
        [HttpDelete("{id}")]
		[Authorize]
        public async Task<IActionResult> DeleteFuelWise([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fuelWise = await _context.FuelWise.FindAsync(id);
            if (fuelWise == null)
            {
                return NotFound();
            }

            _context.FuelWise.Remove(fuelWise);
            await _context.SaveChangesAsync();

            return Ok(fuelWise);
        }

        private bool FuelWiseExists(Guid id)
        {
            return _context.FuelWise.Any(e => e.Id == id);
        }
    }
}
