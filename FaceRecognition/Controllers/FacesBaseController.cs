using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceRecognition.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceRecognition.Controllers
{
    public abstract class FacesBaseController : ImageUsingBaseController
    {
        protected FaceServiceClient FaceClient { get; private set; }
        IConfiguration _iconfiguration;
        public FacesBaseController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var apiKey = _iconfiguration["CognitiveServicesFaceApiKey"];
            var apiRoot = _iconfiguration["CognitiveServicesFaceApiUrl"];
            FaceClient = new FaceServiceClient(apiKey, apiRoot);
        }

        protected string GetInlineImageWithFaces(IEnumerable<Face> faces)
        {
            ImageToProcess.Seek(0, SeekOrigin.Begin);

            using (var img = new Bitmap(ImageToProcess))
            // make copy, drawing on indexed pixel format image is not supported
            using (var nonIndexedImg = new Bitmap(img.Width, img.Height))
            using (var g = Graphics.FromImage(nonIndexedImg))
            using (var mem = new MemoryStream())
            {
                g.DrawImage(img, 0, 0, img.Width, img.Height);

                var i = 0;

                foreach (var face in faces)
                {
                    var pen = new Pen(Settings.ImageSquareColors[i], 5);
                    var faceRectangle = face.FaceRectangle;
                    var rectangle = new Rectangle(faceRectangle.Left,
                                                  faceRectangle.Top,
                                                  faceRectangle.Width,
                                                  faceRectangle.Height);

                    g.DrawRectangle(pen, rectangle);
                    i++;
                }

                nonIndexedImg.Save(mem, ImageFormat.Png);

                var base64 = Convert.ToBase64String(mem.ToArray());
                return String.Format("data:image/png;base64,{0}", base64);
            }
        }

        protected string GetInlineImageWithIdentifiedFaces(IEnumerable<IdentifiedFace> faces)
        {
            ImageToProcess.Seek(0, SeekOrigin.Begin);

            using (var img = new Bitmap(ImageToProcess))
            // make copy, drawing on indexed pixel format image is not supported
            using (var nonIndexedImg = new Bitmap(img.Width, img.Height))
            using (var g = Graphics.FromImage(nonIndexedImg))
            using (var mem = new MemoryStream())
            {
                g.DrawImage(img, 0, 0, img.Width, img.Height);

                var i = 0;

                foreach (var face in faces)
                {
                    var pen = new Pen(face.Color, 5);
                    var faceRectangle = face.Face.FaceRectangle;
                    var rectangle = new Rectangle(faceRectangle.Left,
                                                  faceRectangle.Top,
                                                  faceRectangle.Width,
                                                  faceRectangle.Height);

                    g.DrawRectangle(pen, rectangle);
                    i++;
                }

                nonIndexedImg.Save(mem, ImageFormat.Png);

                var base64 = Convert.ToBase64String(mem.ToArray());
                return String.Format("data:image/png;base64,{0}", base64);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewData["LeftMenu"] = "_FacesMenu";
        }

       
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (FaceClient != null)
            {
                FaceClient.Dispose();
                FaceClient = null;
            }
        }
    }
}