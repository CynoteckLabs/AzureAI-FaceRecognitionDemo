using FaceRecognition.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceRecognition.Controllers
{
    public class HomeController : FacesBaseController
    {
        public HomeController(IConfiguration context)
          : base(context)
        {

        }

        public async Task<IActionResult> Index(IFormFile files)
        {
            ViewData["Title"] = "Detect faces";
            if (HttpContext.Request.Method == "GET")
            {
                return View();
            }
            string imageResult = "";
            if (files.Length > 0)
            {
                using (var temporaryStream = new MemoryStream())
                {
                    files.CopyTo(temporaryStream);
                    ResizeImage(temporaryStream, ImageToProcess);
                    await RunOperationOnImage(async stream =>
                    {
                        var faces = await FaceClient.DetectAsync(stream);
                        imageResult = GetInlineImageWithFaces(faces);
                    });
                }
            }

            return View((object)imageResult);
        }


        public async Task<IActionResult> Identify(IFormFile files)
        {
            var model = new IdentifyFacesModel();

            var groups = await FaceClient.ListPersonGroupsAsync();
            model.PersonGroups = groups.Select(g => new SelectListItem
            {
                Value = g.PersonGroupId,
                Text = g.Name
            }).ToList();

            if (HttpContext.Request.Method == "GET")
            {
                return View(model);
            }
            var personGroupId = Request.Form["personGroupId"];

            Face[] faces = new Face[] { };
            Guid[] faceIds = new Guid[] { };
            IdentifyResult[] results = new IdentifyResult[] { };

            if (files.Length > 0)
            {
                using (var temporaryStream = new MemoryStream())
                {
                    files.CopyTo(temporaryStream);
                    ResizeImage(temporaryStream, ImageToProcess);
                    await RunOperationOnImage(async stream =>
                    {
                        faces = await FaceClient.DetectAsync(stream);
                        faceIds = faces.Select(f => f.FaceId).ToArray();

                        if (faceIds.Count() > 0)
                        {
                            results = await FaceClient.IdentifyAsync(personGroupId, faceIds);
                        }
                    });
                }
            }


            if (faceIds.Length == 0)
            {
                model.Error = "No faces detected";
                return View(model);
            }

            foreach (var result in results)
            {
                var identifiedFace = new IdentifiedFace();
                identifiedFace.Face = faces.FirstOrDefault(f => f.FaceId == result.FaceId);

                foreach (var candidate in result.Candidates)
                {
                    var person = await FaceClient.GetPersonAsync(personGroupId, candidate.PersonId);
                    identifiedFace.PersonCandidates.Add(person.PersonId, person.Name);
                }

                identifiedFace.Color = Settings.ImageSquareColors[model.IdentifiedFaces.Count];
                model.IdentifiedFaces.Add(identifiedFace);
            }

            model.ImageDump = GetInlineImageWithFaces(model.IdentifiedFaces.Select(f => f.Face));
            return View(model);
        }

        public async Task<IActionResult> Landmarks(IFormFile files)
        {
            ViewBag.Title = "Face landmarks";

            if (HttpContext.Request.Method == "GET")
            {
                return View("Index");
            }

            var imageResult = "";
            //var file = Request.Form.Files[0];
            Face[] faces = new Face[] { };

            if (files.Length > 0)
            {
                using (var temporaryStream = new MemoryStream())
                {
                    files.CopyTo(temporaryStream);
                    ResizeImage(temporaryStream, ImageToProcess);
                    await RunOperationOnImage(async stream =>
                    {
                        faces = await FaceClient.DetectAsync(stream, returnFaceLandmarks: true);
                    });
                }
            }

            ImageToProcess.Seek(0, SeekOrigin.Begin);
            using (var img = new Bitmap(ImageToProcess))
            // make copy, drawing on indexed pixel format image is not supported
            using (var nonIndexedImg = new Bitmap(img.Width, img.Height))
            using (var g = Graphics.FromImage(nonIndexedImg))
            using (var mem = new MemoryStream())
            {
                g.DrawImage(img, 0, 0, img.Width, img.Height);

                var pen = new Pen(Color.Red, 5);

                foreach (var face in faces)
                {
                    var props = typeof(FaceLandmarks).GetProperties();
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(FeatureCoordinate))
                        {
                            var coordinate = (FeatureCoordinate)prop.GetValue(face.FaceLandmarks);
                            var rect = new Rectangle((int)coordinate.X, (int)coordinate.Y, 2, 2);
                            g.DrawRectangle(pen, rect);
                        }
                    }
                }

                nonIndexedImg.Save(mem, ImageFormat.Png);

                var base64 = Convert.ToBase64String(mem.ToArray());
                imageResult = String.Format("data:image/png;base64,{0}", base64);
            }

            return View("Index", (object)imageResult);
        }

        public async Task<IActionResult> Emotions(IFormFile files)
        {
            var model = new IdentifyFacesModel();

            var groups = await FaceClient.ListPersonGroupsAsync();
            model.PersonGroups = groups.Select(g => new SelectListItem
            {
                Value = g.PersonGroupId,
                Text = g.Name
            }).ToList();

            if (HttpContext.Request.Method == "GET")
            {
                return View(model);
            }
            var personGroupId = Request.Form["PersonGroupId"];
            Face[] faces = new Face[] { };
            Guid[] faceIds = new Guid[] { };
            IdentifyResult[] results = new IdentifyResult[] { };


            if (files.Length > 0)
            {
                using (var temporaryStream = new MemoryStream())
                {
                    files.CopyTo(temporaryStream);
                    ResizeImage(temporaryStream, ImageToProcess);
                    await RunOperationOnImage(async stream =>
                    {
                        var emotionsType = new[] { FaceAttributeType.Emotion };
                        faces = await FaceClient.DetectAsync(stream, returnFaceAttributes: emotionsType);
                        faceIds = faces.Select(f => f.FaceId).ToArray();

                        if (faceIds.Length > 0)
                        {
                            results = await FaceClient.IdentifyAsync(personGroupId, faceIds);
                        }
                    });
                }
            }

            if (faceIds.Length == 0)
            {
                model.Error = "No faces detected";
                return View(model);
            }

            foreach (var result in results)
            {
                var identifiedFace = new IdentifiedFace();
                identifiedFace.Face = faces.FirstOrDefault(f => f.FaceId == result.FaceId);

                foreach (var candidate in result.Candidates)
                {
                    var person = await FaceClient.GetPersonAsync(personGroupId, candidate.PersonId);

                    identifiedFace.PersonCandidates.Add(person.PersonId, person.Name);
                }

                model.IdentifiedFaces.Add(identifiedFace);
                identifiedFace.Color = Settings.ImageSquareColors[model.IdentifiedFaces.Count];
            }

            model.ImageDump = GetInlineImageWithIdentifiedFaces(model.IdentifiedFaces);

            return View(model);
        }

        public async Task<IActionResult> Attributes(IFormFile files)
        {

            var model = new IdentifyFacesModel();

            var groups = await FaceClient.ListPersonGroupsAsync();
            model.PersonGroups = groups.Select(g => new SelectListItem
            {
                Value = g.PersonGroupId,
                Text = g.Name
            }).ToList();

            if (HttpContext.Request.Method == "GET")
            {
                return View(model);
            }
            var personGroupId = Request.Form["PersonGroupId"];
            Face[] faces = new Face[] { };
            Guid[] faceIds = new Guid[] { };
            IdentifyResult[] results = new IdentifyResult[] { };
            var faceAttributeTypes = new[] {
                FaceAttributeType.Accessories, FaceAttributeType.Age, FaceAttributeType.Blur,
                FaceAttributeType.Exposure, FaceAttributeType.FacialHair, FaceAttributeType.Gender,
                FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion,
                FaceAttributeType.Smile
            };

            if (files.Length > 0)
            {
                using (var temporaryStream = new MemoryStream())
                {
                    files.CopyTo(temporaryStream);
                    ResizeImage(temporaryStream, ImageToProcess);
                    await RunOperationOnImage(async stream =>
                    {
                        faces = await FaceClient.DetectAsync(stream, returnFaceAttributes: faceAttributeTypes);
                        faceIds = faces.Select(f => f.FaceId).ToArray();

                        if (faceIds.Length > 0)
                        {
                            results = await FaceClient.IdentifyAsync(personGroupId, faceIds);
                        }
                    });
                }
            }

            if (faceIds.Length == 0)
            {
                model.Error = "No faces detected";
                return View(model);
            }

            foreach (var result in results)
            {
                var identifiedFace = new IdentifiedFace();
                identifiedFace.Face = faces.FirstOrDefault(f => f.FaceId == result.FaceId);

                foreach (var candidate in result.Candidates)
                {
                    var person = await FaceClient.GetPersonAsync(personGroupId, candidate.PersonId);

                    identifiedFace.PersonCandidates.Add(person.PersonId, person.Name);
                }

                identifiedFace.Color = Settings.ImageSquareColors[model.IdentifiedFaces.Count];
                model.IdentifiedFaces.Add(identifiedFace);
            }

            model.ImageDump = GetInlineImageWithIdentifiedFaces(model.IdentifiedFaces);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Login(IFormFile files)
        {
            var model = new IdentifyFacesModel();
            bool imageDetected = false;
            if (HttpContext.Request.Method == "GET")
            {
                return View(model);
            }
            var groups = await FaceClient.ListPersonGroupsAsync();
            Face[] faces = new Face[] { };
            Guid[] faceIds = new Guid[] { };
            IdentifyResult[] results = new IdentifyResult[] { };

            if (files.Length > 0)
            {
                using (var temporaryStream = new MemoryStream())
                {
                    files.CopyTo(temporaryStream);
                    ResizeImage(temporaryStream, ImageToProcess);
                    await RunOperationOnImage(async stream =>
                    {
                        faces = await FaceClient.DetectAsync(stream);
                        faceIds = faces.Select(f => f.FaceId).ToArray();

                        if (faceIds.Count() > 0)
                        {
                            foreach (var item in groups)
                            {
                                results = await FaceClient.IdentifyAsync(item.PersonGroupId, faceIds);
                                if (results.Length > 0)
                                {
                                    foreach (var result in results)
                                    {
                                        if (result.Candidates.Length > 0)
                                        {
                                            imageDetected = true;
                                            break;
                                        }
                                    }
                                        
                                }
                            }
                        }
                    });
                }
            }


            if (faceIds.Length == 0)
            {
                model.Error = "No faces detected";
                return View(model);
            }
            if (imageDetected)
            {
                model.Error = "Login success";
                return RedirectToAction("Index", "PersonGroups");
            }
            else
            {
                model.Error = "Login failed";
                return View(model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
