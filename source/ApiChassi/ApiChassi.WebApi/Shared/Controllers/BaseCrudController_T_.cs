using ApiChassi.WebApi.Shared.Models.Request.Interfaces;
using ApiChassi.WebApi.Shared.Models.Response.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ApiChassi.WebApi.Shared.Controllers
{
    public abstract class BaseCrudController<TGetResponseModel, TSearchRequestModel, TSearchResponseModel, TPostRequestModel, TPostResponseModel, TPutRequestModel>: BaseReadController<TGetResponseModel, TSearchRequestModel, TSearchResponseModel>
        where TGetResponseModel : class
        where TSearchResponseModel : class
        where TSearchRequestModel : ISearchRequestModel
        where TPostResponseModel: IPostResponseModel
        where TPutRequestModel: IPutRequestModel
    {
        protected abstract Task<bool> ExistsAsync(Guid id);

        protected abstract Task<TPostResponseModel> CreateAsync(TPostRequestModel request);

        protected abstract Task UpdateAsync(TPutRequestModel request);

        protected abstract Task DeleteAsync(TGetResponseModel request);

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
            var _urlString = $"{HttpContext.Request.Path}/{_item.Id}";
            return Created(_urlString, _item);
        }

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

    public abstract class BaseCrudController<TModel, TSearchRequestModel, TSearchResponseModel> : BaseCrudController<TModel, TSearchRequestModel, TSearchResponseModel, TModel, TModel, TModel>
        where TModel : class, IPostRequestModel, IPostResponseModel, IPutRequestModel
        where TSearchResponseModel : class
        where TSearchRequestModel : ISearchRequestModel
    { }

    public abstract class BaseCrudController<TModel, TSearchRequestModel> : BaseCrudController<TModel, TSearchRequestModel, TModel, TModel, TModel, TModel>
        where TModel : class, IPostRequestModel, IPostResponseModel, IPutRequestModel
        where TSearchRequestModel : ISearchRequestModel
    { }
}
