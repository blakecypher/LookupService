using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using model;
using service;

namespace backend;

/// <summary>
/// Agregates and returns credit data for a given SSN
/// </summary>
[ApiController]
[Route("credit-data")]
[ApiExplorerSettings(GroupName = "v1")]
public class CreditDataController(ILookupService lookupService) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ssn"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(CreditData), 200)][HttpGet("{ssn}")]
    public async Task<ActionResult<CreditData>> GetCreditData(string ssn)
    {
        var creditData  = await lookupService.GetCreditData(ssn);

        if (creditData == null)
        {
            return NotFound();
        }

        return Ok(creditData);
    }
}