using System;

namespace ApiChassi.WebApi.Shared.Models.Request.Interfaces
{
    /// <summary>
    /// Defines the minimun required fields for request generic Put operations
    /// </summary>
    public interface IPutRequestModel
    {
        /// <summary>
        /// Record identification
        /// </summary>
        Guid Id { get; set; }
    }
}
