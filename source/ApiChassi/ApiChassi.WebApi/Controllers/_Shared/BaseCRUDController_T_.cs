namespace ApiChassi.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ApiChassi.WebApi.Models.Request.Interfaces;
    using ApiChassi.WebApi.Models.Response.Interfaces;

    public abstract class BaseCRUDController<
        TGetResponseModel, 
        TFindRequestModel, 
        TFindResponseModel, 
        TCreateRequestModel,
        TCreateResponseModel,
        TUpdateRequestModel> : BaseReadController<TGetResponseModel, TFindRequestModel, TFindResponseModel>
        where TGetResponseModel: class
        where TFindRequestModel:  IFindRequestModel
        where TCreateRequestModel: class, ICreateRequestModel
        where TCreateResponseModel: class, ICreateResponseModel
        where TUpdateRequestModel: class, IUpdateRequestModel
    {
        protected abstract Task<bool> ExistsAsync(Guid id);

        protected abstract Task<TCreateResponseModel> CreateAsync(TCreateRequestModel request);

        protected abstract Task UpdateAsync(TUpdateRequestModel request);

        protected abstract Task DeleteAsync(TGetResponseModel request);

        /// <summary>
        /// Creates the specified item.
        /// </summary>
        /// <returns>The created item.</returns>
        /// <param name="apiVersion">API version.</param>
        /// <param name="item">Item.</param>
        [HttpPost(Name = "create-record")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<TCreateResponseModel>> Post(ApiVersion apiVersion, [FromBody] TCreateRequestModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var _item = await CreateAsync(item);
            var _urlString = $"{HttpContext.Request.Path}/{_item.Id}";
            return Created(_urlString, _item);
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <returns>Success of the operation.</returns>
        /// <param name="apiVersion">API version.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="item">Item.</param>
        [HttpPut("{id}", Name = "update-record")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Put(ApiVersion apiVersion, Guid id, [FromBody] TUpdateRequestModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (item.Id != id)
            {
                return BadRequest();
            }
            var _exists = await ExistsAsync(id);
            if (!_exists)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await UpdateAsync(item);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <returns>Success of the operation.</returns>
        /// <param name="apiVersion">API version.</param>
        /// <param name="id">Identifier.</param>
        [HttpDelete("{id}", Name = "delete-record")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Delete(ApiVersion apiVersion, Guid id)
        {
            var _item = await GetAsync(id);
            if (_item == null)
            {
                return NotFound();
            }
            await DeleteAsync(_item);
            return NoContent();
        }
    }

    public abstract class BaseCRUDController<TModel, TFindRequestModel, TFindResponseModel> : BaseCRUDController<TModel, TFindRequestModel, TFindResponseModel, TModel, TModel, TModel>
        where TModel : class, ICreateRequestModel, ICreateResponseModel, IUpdateRequestModel
        where TFindRequestModel : IFindRequestModel
    { }

    public abstract class BaseCRUDController<TModel, TFindRequestModel> : BaseCRUDController<TModel, TFindRequestModel, TModel, TModel, TModel, TModel>
        where TModel : class, ICreateRequestModel, ICreateResponseModel, IUpdateRequestModel
        where TFindRequestModel: IFindRequestModel
    { }
}
