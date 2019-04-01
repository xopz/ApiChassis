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

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Post(ApiVersion apiVersion, [FromBody] TCreateRequestModel item)
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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

    public abstract class BaseCRUDController<T> : BaseCRUDController<T, T, T, T, T, T>
        where T : class, IFindRequestModel, ICreateRequestModel, ICreateResponseModel, IUpdateRequestModel
    { }
}
