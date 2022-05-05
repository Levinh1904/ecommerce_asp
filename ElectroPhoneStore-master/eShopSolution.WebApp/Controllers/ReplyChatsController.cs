using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class ReplyChatsController : Controller
    {
        private readonly EShopDbContext _eShopDbContext;

        public ReplyChatsController(EShopDbContext eShopDbContext)
        {
            _eShopDbContext = eShopDbContext;
        }
        // GET: ReplyChats
        public async Task<IActionResult> Index()
        {
            return View(await _eShopDbContext.ReplyChats.ToListAsync());
        }

        // GET: ReplyChatss/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var replyChat = await _eShopDbContext.ReplyChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (replyChat == null)
            {
                return NotFound();
            }

            return View(replyChat);
        }

        // GET: ReplyChatss/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReplyChatss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Reply,Message")] ReplyChat replyChat)
        {
            if (ModelState.IsValid)
            {
                _eShopDbContext.Add(replyChat);
                await _eShopDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(replyChat);
        }

        // GET: ReplyChatss/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var replyChat = await _eShopDbContext.ReplyChats.FindAsync(id);
            if (replyChat == null)
            {
                return NotFound();
            }
            return View(replyChat);
        }

        // POST: ReplyChatss/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Reply,Message")] ReplyChat replyChat)
        {
            if (id != replyChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eShopDbContext.Update(replyChat);
                    await _eShopDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReplyChatsExists(replyChat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(replyChat);
        }

        // GET: ReplyChatss/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var replyChat = await _eShopDbContext.ReplyChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (replyChat == null)
            {
                return NotFound();
            }

            return View(replyChat);
        }

        // POST: ReplyChatss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var replyChat = await _eShopDbContext.ReplyChats.FindAsync(id);
            _eShopDbContext.ReplyChats.Remove(replyChat);
            await _eShopDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReplyChatsExists(int id)
        {
            return _eShopDbContext.ReplyChats.Any(e => e.Id == id);
        }
        [HttpPost("api/Chat")]
        public async Task<JsonResult> Chat(RequestApi request)
        {
            var replyChat = await _eShopDbContext.ReplyChats.Where(m => m.Message.ToUpper().Contains(request.message.ToUpper())).FirstOrDefaultAsync();

            if (replyChat != null)
            {
                var reply = new ResponseApi { reply = replyChat.Reply };

                return Json(reply);
            }
            else
            {
                var reply = new ResponseApi { reply = "Chúng tôi không hiểu câu hỏi của bạn.Bạn có thể diễn đạt lại được không ? " };
                return Json(reply);
            }
        }
    }
}
