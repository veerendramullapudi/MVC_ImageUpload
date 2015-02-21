using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvcimageupload.Controllers
{
    public class userController : Controller
    {
        //s
        // GET: /user/

        public ActionResult Index()
        
        {
            Database1Entities de = new Database1Entities();
            ViewBag.UsersList = de.Tables.ToList();
            return View();
        }
        public async Task<ActionResult> Upload(Table user)
        {
            Database1Entities ve = new Database1Entities();
            if (ModelState.IsValid)
            {
                ve.Tables.Add(user);
                if (Request.Files.Count > 0)
                {
                    Stream fis = Request.Files[0].InputStream;
                    byte[] data = new Byte[Request.Files[0].ContentLength];
                    await fis.ReadAsync(data, 0, data.Length);
                    user.image = data;
                }
                await ve.SaveChangesAsync();
            }
            ViewBag.UsersList = ve.Tables.ToList();
            return View("Index", user);
        }
        [HttpGet]
        public FileStreamResult GetImage(int id)
        {
            Database1Entities ve = new Database1Entities();
            Table founduser = ve.Tables.Find(id);
            if (founduser != null)
            {
                byte[] data = founduser.image;
                System.IO.MemoryStream ms = new MemoryStream(data);
                FileStreamResult res = new FileStreamResult(ms, "image/jpg");
                return res;
            }
            return null;
        }


    }
}
