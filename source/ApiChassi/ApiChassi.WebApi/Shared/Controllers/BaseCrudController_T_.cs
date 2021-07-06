using ApiChassi.WebApi.Shared.Models.Request.Interfaces;
using ApiChassi.WebApi.Shared.Models.Response.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ApiChassi.WebApi.Shared.Controllers
{
    /// <summary>
    /// A base controller with `get`, `search`, `post`, `put` and `delete` operations.
    /// </summary>
    /// <typeparam name="TGetResponseModel">The type of the `get` response</typeparam>
    /// <typeparam name="TSearchRequestModel">The type of the `search` resquest</typeparam>
    /// <typeparam name="TSearchResponseModel">The type of the `search` response</typeparam>
    /// <typeparam name="TPostRequestModel">The type of the `post` request</typeparam>
    /// <typeparam name="TPostResponseModel">The type of the `post` response</typeparam>
    /// <typeparam name="TPutRequestModel">The type of the `put` request</typeparam>
    public abstract class BaseCrudController<TGetResponseModel, TSearchRequestModel, TSearchResponseModel, TPostRequestModel, TPostResponseModel, TPutRequestModel>: BaseReadController<TGetResponseModel, TSearchRequestModel, TSearchResponseModel>
        where TGetResponseModel : class
        where TSearchResponseModel : class
        where TSearchRequestModel : ISearchRequestModel
        where TPostResponseModel: IPostResponseModel
        where TPutRequestModel: IPutRequestModel
    {
        /// <summary>
        /// An abstraction of an operation to check the existince of a record
        /// </summary>
        /// <param name="id">The record identification</param>
        /// <returns>The existence of the identified record</returns>
        protected abstract Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// An abstraction of an operation to create a record
        /// </summary>
        /// <param name="request">The record data</param>
        /// <returns>The record created</returns>
        protected abstract Task<TPostResponseModel> CreateAsync(TPostRequestModel request);

        /// <summary>
        /// An abstraction of an operation to update a record
        /// </summary>
        /// <param name="request">The updated record data</param>
        protected abstract Task UpdateAsync(TPutRequestModel request);

        /// <summary>
        /// An abstraction of an operation to delete a record
        /// </summary>
        /// <param name="request">The record reference to be deleted</param>
        protected abstract Task DeleteAsync(TGetResponseModel request);

        /// <summary>
        /// Creates data based on request payload
        /// </summary>
        /// <param name="version">Version of the api</param>
        /// <param name="item">Payload data</param>
        /// <returns>Response payload containing the resulting created record</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<TPostResponseModel>> Post(ApiVersion version, [FromBody]TPostRequestModel item)
        {
            if (item == null) return BadRequest(new ProblemDetails
            {
                Title = "Null item",
                Detail = "Post requests cannot have null payload"
            });

            if (!ModelState.IsValid) return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Detail = "Payload have one or more validation errors"
            });

            var _item = await CreateAsync(item);
            var _urlString = $"{HttpContext?.Request?.Path ?? ""}/{_item.Id}";
            return Created(_urlString, _item);
        }

        /// <summary>
        /// Updates data based on request payload
        /// </summary>
        /// <param name="version">Version of the api</param>
        /// <param name="id">Record identification</param>
        /// <param name="item">Payload of updated data</param>
        /// <returns>Empty response if operation was successful</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Put(ApiVersion version, Guid id, [FromBody]TPutRequestModel item)
        {
            if (item == null) return BadRequest(new ProblemDetails
            {
                Title = "Null item",
                Detail = "Post requests cannot have null payload"
            });

            if (item.Id != id || !ModelState.IsValid) return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Detail = "Payload have one or more validation errors"
            });

            var _exists = await ExistsAsync(id);

            if (!_exists) return NotFound(new ProblemDetails
            {
                Detail = $"Record with id {id} could not be found in app records.",
                Title = "Record not found."
            });

            await UpdateAsync(item);
            return NoContent();
        }

        /// <summary>
        /// Deletes data based on record identification
        /// </summary>
        /// <param name="version">Version of the api</param>
        /// <param name="id">Record identification</param>
        /// <returns>Empty response if operation was successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Delete(ApiVersion version, Guid id)
        {
            var _item = await GetAsync(id);
            if (_item == null) return NotFound(new ProblemDetails
            {
                Detail = $"Record with id {id} could not be found in app records.",
                Title = "Record not found."
            });
            await DeleteAsync(_item);
            return NoContent();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TSearchRequestModel"></typeparam>
    /// <typeparam name="TSearchResponseModel"></typeparam>
    public abstract class BaseCrudController<TModel, TSearchRequestModel, TSearchResponseModel> : BaseCrudController<TModel, TSearchRequestModel, TSearchResponseModel, TModel, TModel, TModel>
        where TModel : class, IPostResponseModel, IPutRequestModel
        where TSearchResponseModel : class
        where TSearchRequestModel : ISearchRequestModel
    { }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TSearchRequestModel"></typeparam>
    public abstract class BaseCrudController<TModel, TSearchRequestModel> : BaseCrudController<TModel, TSearchRequestModel, TModel, TModel, TModel, TModel>
        where TModel : class, IPostResponseModel, IPutRequestModel
        where TSearchRequestModel : ISearchRequestModel
    { }
}
