namespace ApiChassi.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using ApiChassi.WebApi.Models.Request.Interfaces;

    public abstract class BaseReadController<TGetResponseModel, TFindRequestModel, TFindResponseModel> : BaseController
        where TGetResponseModel : class 
        where TFindRequestModel: IFindRequestModel
    {

        protected abstract Task<TGetResponseModel> GetAsync(Guid id);

        protected abstract Task<TFindResponseModel> FindAsync(TFindRequestModel request);

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]

        public virtual async Task<ActionResult<IEnumerable<TFindResponseModel>>> GetList(ApiVersion version, [FromQuery] TFindRequestModel request)
        {
            return Ok(await FindAsync(request));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<TGetResponseModel>> GetItem(ApiVersion version, Guid id)
        {
            var _result = await GetAsync(id);
            if (_result == null)
            {
                return NotFound();
            }
            return Ok(_result);
        }
    }

    public abstract class BaseReadController<TModel, TFindRequestModel> : BaseReadController<TModel, TFindRequestModel, TModel> 
        where TModel: class
        where TFindRequestModel: IFindRequestModel 
    { }
}
