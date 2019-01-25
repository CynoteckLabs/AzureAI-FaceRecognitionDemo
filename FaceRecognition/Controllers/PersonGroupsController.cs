using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceRecognition.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceRecognition.Controllers
{
    public class PersonGroupsController : FacesBaseController
    {
        public PersonGroupsController(IConfiguration context)
          : base(context)
        {

        }
        public async Task<IActionResult> Index()
        {
            var groups = await FaceClient.ListPersonGroupsAsync();
            return View(groups);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id not found");
            }
            var model = new PersonGroupDetailsModel();
            model.PersonGroup = await FaceClient.GetPersonGroupAsync(id);
            try
            {
                model.TrainingStatus = await FaceClient.GetPersonGroupTrainingStatusAsync(id);
            }
            catch (FaceAPIException fex)
            {
                ModelState.AddModelError(string.Empty, fex.ErrorMessage);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PersonGroup());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonGroup model)
        {
            try
            {
                await FaceClient.CreatePersonGroupAsync(model.PersonGroupId, model.Name, model.UserData);
            }
            catch (FaceAPIException fex)
            {
                ModelState.AddModelError(string.Empty, fex.ErrorMessage);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id not found");
            }
            var model = await FaceClient.GetPersonGroupAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonGroup model)
        {
            await FaceClient.UpdatePersonGroupAsync(model.PersonGroupId, model.Name, model.UserData);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Train(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id not found");
            }
            await FaceClient.TrainPersonGroupAsync(id);

            return RedirectToAction("Details", new { id = id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await FaceClient.DeletePersonGroupAsync(id);
            }
            catch (FaceAPIException fex)
            {
                ModelState.AddModelError(string.Empty, fex.ErrorMessage);
                return await Index();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return await Index();
            }

            return RedirectToAction("Index");
        }
    }
}