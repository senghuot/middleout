using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web;
using System.IO;

namespace ProductsApp.Controllers {
    public class UploadController : ApiController {
        
        [HttpPost]
        public async Task<HttpResponseMessage> Upload() {
            // check if the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            // grab the relative path to "App_Data"
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new CustomMultipartFormDataStreamProvider(root);

            try {
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                return Request.CreateResponse(HttpStatusCode.OK);
            } catch (System.Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


        //[HttpPost]
        //public HttpResponseMessage> post() {
        //    // check if the request contains multipart/form-data
        //    if (!Request.Content.IsMimeMultipartContent())
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

        //    // grab the relative path to "App_Data"
        //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new CustomMultipartFormDataStreamProvider(root);
        //    try {
        //        var result = Request.Content.ReadAsMultipartAsync(provider).Result;
        //        //Console.WriteLine(result.FormData[0]);
        //        // This illustrates how to get the file names.
        //        foreach (MultipartFileData fileData in provider.FileData) {
        //            if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName)) {
        //                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //            }
        //            string fileName = fileData.Headers.ContentDisposition.FileName;
        //            if (fileName.StartsWith("\"") && fileName.EndsWith("\"")) {
        //                fileName = fileName.Trim('"');
        //            }
        //            if (fileName.Contains(@"/") || fileName.Contains(@"\")) {
        //                fileName = Path.GetFileName(fileName);
        //            }
        //            File.Move(fileData.LocalFileName, Path.Combine(root, fileName));
        //        }      
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    } 
        //    catch (System.Exception e) 
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }
        //}

        /*public async Task<HttpResponseMessage> PostFormData() {
            
            // check if the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            string path = @"C:/middleout/ProductsApp/App_Data/";
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
        }*/
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider {
	    public CustomMultipartFormDataStreamProvider(string path) : base(path){}
 
        //public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers){
        //    var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
        //    //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        //    return name.Replace("\"",string.Empty);
        //}
    }
}
