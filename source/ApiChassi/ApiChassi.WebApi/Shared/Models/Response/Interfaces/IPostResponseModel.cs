using System;

namespace ApiChassi.WebApi.Shared.Models.Response.Interfaces
{
    /// <summary>
    /// Defines the minimun required fields for responses of generic Post operations 
    /// </summary>
    public interface IPostResponseModel
    {
        /// <summary>
        /// Record identification
        /// </summary>
        Guid Id { get; set; }
    }
}
