using LicenseRentalAPI.Extensions;
using LicenseRentalAPI.Models;
using LicenseRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseRentalAPI.Controllers
{
    [Route("api/license")]
    [ApiController]
    [Authorize]
    public class LicenseController : ControllerBase
    {
        private readonly LicenseService _licenseService;
        private readonly string _renter;

        public LicenseController(LicenseService licenseService, IHttpContextAccessor httpContextAccessor) 
        {
            _licenseService = licenseService;
            _renter = httpContextAccessor.HttpContext.GetRenter();
        }

        /// <summary>
        /// Rent a license if available
        /// </summary>
        /// <returns></returns>
        [HttpGet("rent", Name = "RentLicense")]
        public async Task<ActionResult<License>> RentLicense()
        {
            // Fetch license
            var license = await _licenseService.RentLicense(_renter);
            if (license == null)
            {
                return NotFound("No license available at the moment.");
            }
            return Ok(license);
        }

        /// <summary>
        /// Add license if it does not already exist
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        [HttpPut("{licenseId}", Name = "AddLicense")]
        public async Task<ActionResult> AddLicense([FromRoute] string licenseId)
        {
            var licenseAdded = await _licenseService.AddLicense(licenseId);
            if (!licenseAdded)
            {
                return StatusCode(403, "License not added as it already exists.");
            }
            return Ok("License added");
        }

        /// <summary>
        /// Get all licenses
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllLicenses")]
        public async Task<ActionResult<IEnumerable<License>>> GetAllLicenses()
        {
            // Fetch license
            var licenses = await _licenseService.GetAllLicenses();
            if (!licenses.Any())
            {
                return NotFound("No licenses exist.");
            }
            return Ok(licenses);
        }
    }
}
