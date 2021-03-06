﻿using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ProductsApp.Controllers {
    public class UpdatesController : ApiController {
        static readonly Dictionary<Guid, Update> updates = new Dictionary<Guid, Update>();

        [HttpPost]
        public HttpResponseMessage PostComplexion(Update update) {
            if (ModelState.IsValid && update != null) {
                update.Status = HttpUtility.HtmlEncode(update.Status);

                var id = Guid.NewGuid();
                updates[id] = update;

                var response = new HttpResponseMessage(HttpStatusCode.Created) {
                    Content = new StringContent(update.Status + " appending new content from the server")
                };
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { action = "status", id = id }));
                return response;
            } else
                return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public Update Status(Guid id) {
            Update update;
            if (updates.TryGetValue(id, out update)) {
                return update;
            } else {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
