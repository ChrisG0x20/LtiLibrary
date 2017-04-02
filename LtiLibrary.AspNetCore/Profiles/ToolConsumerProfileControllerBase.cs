﻿using System;
using System.Threading.Tasks;
using LtiLibrary.NetCore.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LtiLibrary.AspNetCore.Profiles
{
    /// <summary>
    /// Implements the LTI Tool Consumer Profile API.
    /// </summary>
    [Route("profiles/toolconsumerprofile", Name = "ToolConsumerProfileApi")]
    [Consumes(LtiConstants.ToolConsumerProfileMediaType)]
    [Produces(LtiConstants.ToolConsumerProfileMediaType)]
    public abstract class ToolConsumerProfileControllerBase : Controller
    {
        protected ToolConsumerProfileControllerBase()
        {
            OnGetToolConsumerProfile = context => { throw new NotImplementedException(); };
        }

        public Func<GetToolConsumerProfileContext, Task> OnGetToolConsumerProfile { get; set; }

        [HttpGet]
// ReSharper disable InconsistentNaming
        public async Task<IActionResult> GetAsync(string lti_version = "LTI-1p0")
// ReSharper restore InconsistentNaming
        {
            try
            {
                var context = new GetToolConsumerProfileContext(lti_version);

                await OnGetToolConsumerProfile(context);

                if (context.StatusCode == StatusCodes.Status200OK)
                {
                    // Set the Content-Type of the ObjectResult
                    return new ToolConsumerProfileResult(context.ToolConsumerProfile);
                }
                return StatusCode(context.StatusCode);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
