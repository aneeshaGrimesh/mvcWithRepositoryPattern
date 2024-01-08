using CrudWithRepository_Core;
using CrudWithRepository_Infrastructure.Implementations;
using CrudWithRepository_Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CrudWithRepository_UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var product = await _productRepo.GetAll();
            return View(product);
        }

        // it will create empty form for insert data
        [HttpGet]
        public  async Task<IActionResult> CreateOrEdit(int id =0)
        {
            if(id == 0)
            {
                return View(new Product());
            }
            else
            {
                try
                {
                    Product product = await _productRepo.GetById(id);
                    if (product != null)
                    {
                        return View(product);
                    }
                   
                }
                catch (Exception ex)
                {

                    TempData["errorMessage"] = ex.Message;
                    return RedirectToAction("Index");
                }
                TempData["errorMessage"] = $"Product datails not avialble with id : {id}";
                return RedirectToAction("Index");
            }
            
        }

        // it will insert data
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(Product model )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Id == 0)
                    {
                        await _productRepo.Add(model);
                        //these  Messages will set in layout pages
                        TempData["successMessage"] = "Product Created Succesfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        await _productRepo.Update(model);
                        TempData["successMessage"] = "Product Updated Succesfully";
                        return RedirectToAction(nameof(Index));
                    }
                   
                }
                else
                {
                    TempData["errorMessage"] = "Model State is Invalid";
                    return View();
                }
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            
        }

        [HttpGet]
        public async Task< IActionResult> Delete( int id)
        {
            try
            {
                Product product = await _productRepo.GetById(id);
                if (product != null)
                {
                    return View(product);
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] =ex.Message;
                return RedirectToAction("Index");
            }
            TempData["errorMessage"] = $"Product datails not avialble with id : {id}";
            return RedirectToAction("Index");
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConform(int id)
        {
            try
            {
                await _productRepo.DeleteById(id);
                TempData["successMessage"] = "Product Deleted Succesfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex )
            {

                TempData["successMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
