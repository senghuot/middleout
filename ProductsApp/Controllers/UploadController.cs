using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web;

namespace ProductsApp.Controllers {
    public class UploadController : ApiController {

        public async Task<HttpResponseMessage> PostFormData() {
            // check if the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            string path = @"C:/Users/limse/Documents/Visual Studio 2013/Projects/ProductsApp/ProductsApp/App_Data/";
            try {
                // Read the form data
                await Request.Content.ReadAsMultipartAsync(provider);
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData) {
                    // grab the original filename but needs to remove the first 2 and last characters
                    var filename    = file.Headers.ContentDisposition.FileName;
                    filename        = filename.Substring(1, filename.Length - 2);

                    // need to file the tmp filename to replace with the original filename
                    // this could be dangerous because files could overwrite each other.
                    var tmpFilename = file.LocalFileName.Substring(file.LocalFileName.LastIndexOf("\\") + 1);
                    System.IO.File.Move(path + tmpFilename, path + filename);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            } catch (System.Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e); 
            } 
        }
    }
}
