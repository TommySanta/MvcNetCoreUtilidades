﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MvcNetCoreUtilidades.Controllers
{
    public class CachingController : Controller
    {
        private IMemoryCache memoryCache;
        public CachingController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MemoriaPersonalizada(int? tiempo)
        {
            if (tiempo == null)
            {
                tiempo = 60;
            }
            string fecha = DateTime.Now.ToLongDateString() + " -- "
                + DateTime.Now.ToLongTimeString();
            if (this.memoryCache.Get("FECHA") == null)
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(tiempo.Value));

                this.memoryCache.Set("FECHA", fecha, options);
                ViewData["MENSAJE"] = "FECHA almacenada en Cache";
                ViewData["FECHA"] = this.memoryCache.Get("FECHA");

            }
            else
            {
                fecha = this.memoryCache.Get<string>("FECHA");
                ViewData["MENSAJE"] = "Fecha recuperada de Cache";
                ViewData["FECHA"] = fecha;
            }
            return View();
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult MemoriaDistribuida()
        {
            string fecha = DateTime.Now.ToLongDateString()+ "--"+
                DateTime.Now.ToLongTimeString();
            ViewData["FECHA"] = fecha;
            return View();
        }
    }
}
