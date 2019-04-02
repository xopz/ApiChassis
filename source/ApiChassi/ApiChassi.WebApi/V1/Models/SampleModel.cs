namespace ApiChassi.WebApi.V1.Models
{
    using System;
    using ApiChassi.WebApi.Models.Request.Interfaces;
    using ApiChassi.WebApi.Models.Response.Interfaces;

    /// <summary>
    /// Defines a Sample Model.
    /// </summary>
    public class SampleModel : ICreateRequestModel, ICreateResponseModel, IUpdateRequestModel
    {
        /// <summary>
        /// Gets or sets the sample unique identifier.
        /// </summary>
        /// <example>f303a177-a23c-4769-9f63-f65608b4b505</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the descriptions of the sample.
        /// </summary>
        /// <example>Some description.</example>
        public string Description { get; set; }
    }
}
