using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace IsPrimeNumber
{
    public class RateLimitFillter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly IMemoryCache _memoryCache;

        public RateLimitFillter()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions() { }); ;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return RateLimitCheck(context, next);
        }

        private Task RateLimitCheck(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ip = GetClientIpAddress(context.HttpContext.Request);
            var ipVal = _memoryCache.Get(ip);

            var rec = new RateLimitMemRec
            {
                time = DateTime.Now,
                count = 0
            };
            if (ipVal == null)
            {
                _memoryCache.Set(ip, rec, DateTime.Now.AddMinutes(10));
            }
            else
            {
                rec = (RateLimitMemRec)ipVal;
                rec.count++;
                _memoryCache.Set(ip, rec, DateTime.Now.AddMinutes(10));
            }
            if (rec.count > 10)
            {
                context.HttpContext.Items.Add("RateLimit", rec.time);
            }
            else
            {
                context.HttpContext.Items.Add("RateLimit", null);
            }
            return next();
        }

        private string GetClientIpAddress(HttpRequest request)
        {
            return request.Host.Value;
        }
    }
}
