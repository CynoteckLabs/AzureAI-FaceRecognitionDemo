using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceRecognition.Controllers
{
    public class PeopleController : FacesBaseController
    {
        public PeopleController(IConfiguration context)
            : base(context)
        {

        }
        public async  Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Person group ID is missing");
            }

            var model = await FaceClient.ListPersonsAsync(id);
            ViewBag.PersonGroupId = id;

            return View(model);
        }

        public async Task<IActionResult> Details(string id,Guid? personId)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id Not Found");
            }
            if (personId==null)
            {
                return NotFound("PersonId Not Found");
            }
            var model = await FaceClient.GetPersonAsync(id, personId.Value);
            ViewBag.PersonGroupId = id;

            return View(model);
        }

        public IActionResult Create(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id not found");
            }
            ViewBag.PersonGroupId = id;
            return View("Edit", new Person());
        }

        [HttpPost]
        public async Task<IActionResult> Create (Person person)
        {
            return await Edit(person);
        }

        public async Task<IActionResult> Edit(string id,Guid personId)
        {
            ViewBag.PersonGroupId = id;
            var model = await FaceClient.GetPersonAsync(id, personId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Person person)
        {
            var personGroupId = Request.Form["PersonGroupId"];
            if (string.IsNullOrEmpty(personGroupId))
            {
                return NotFound("PersongroupId not found");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.PersonGroupId = personGroupId;
                return View(person);
            }
            try
            {
                if (person.PersonId == Guid.Empty)
                {
                    await FaceClient.CreatePersonAsync(personGroupId, person.Name, person.UserData);
                }
                else
                {
                    await FaceClient.UpdatePersonAsync(personGroupId, person.PersonId, person.Name, person.UserData);
                }
                return RedirectToAction("Index", new { id = personGroupId });
            }
            catch (FaceAPIException fex)
            {
                ModelState.AddModelError(string.Empty, fex.ErrorMessage);
            }
            return View(person);
        }

        [HttpGet]
        public ActionResult AddFace(string id, string personId)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddFace(string id,Guid personId)
        {
            try
            {
                if (Request.Form.Files[0].Length > 0)
                {
                    using (var temporaryStream = new MemoryStream())
                    {
                        Request.Form.Files[0].CopyTo(temporaryStream);
                        ResizeImage(temporaryStream, ImageToProcess);
                        await RunOperationOnImage(async stream =>
                        {
                            await FaceClient.AddPersonFaceAsync(id, personId, stream);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

            return RedirectToAction("Index", new { id = id });
        }

        public async Task<IActionResult> Delete(string id, Guid personId)
        {
            try
            {
                await FaceClient.DeletePersonAsync(id,personId);
            }
            catch (FaceAPIException fex)
            {
                ModelState.AddModelError(string.Empty, fex.ErrorMessage);
                return RedirectToAction("Index", new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index", new { id = id });
            }

            return RedirectToAction("Index", new { id = id });
        }
    }
}