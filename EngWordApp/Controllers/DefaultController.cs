using EngWordApp.Context;
using EngWordApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngWordApp.Controllers
{
    public class DefaultController : Controller
    {
        private readonly WordContext _context;
        Random rnd = new Random();

        public DefaultController(WordContext context)
        {
            _context = context;
        }

        public enum CategoryValue
        {
            AllList = 1,
            A1 = 2,
            A2 = 3,
            B1 = 4,
            B2 = 5,
            Con = 6,
        };
        

        public IActionResult TakeFilter()
        {
            ViewBag.allListCount = _context.Words.Count();
            ViewBag.a1ListCount = _context.Words.Where(x => x.WordLevel == "A1").Count();
            ViewBag.a2ListCount = _context.Words.Where(x => x.WordLevel == "A2").Count();
            ViewBag.b1ListCount = _context.Words.Where(x => x.WordLevel == "B1").Count();
            ViewBag.b2ListCount = _context.Words.Where(x => x.WordLevel == "B2").Count();
            ViewBag.conListCount = _context.Words.Where(x => x.Connective != null).Count();
            var values = _context.Words.Where(y => y.Weak != null).Select(x => x.Weak).Distinct().ToList();
            return View(values);
        }


        public IActionResult Index(int filter)
        {
            ViewBag.Filter = filter;
            if(filter>6)
            {
                string validWeakValue = "Hafta ";
                validWeakValue += (filter - 5).ToString();
                var values = _context.Words.Where(x => x.Weak == validWeakValue).ToList();
                if(values == null)
                {
                    return RedirectToAction("TakeFilter");
                }
                else
                {
                    
                    int indx = rnd.Next(values.Count);
                    var EngWord = values[indx];
                    var questionId = EngWord.WordId;
                    var questionWord = EngWord.EngWord;
                    HttpContext.Session.SetInt32("QuestionId", questionId);
                    ViewBag.firstWord = questionWord;
                    return View();
                }
                    
            }
            else if(filter < 7 && filter > 1)
            {
                string category = Enum.GetName(typeof(CategoryValue), filter);
                var values = _context.Words.Where(x => x.WordLevel == category).ToList();
                if (values == null || values.Count == 0)
                {
                    return RedirectToAction("TakeFilter");
                }
                else
                {
                    int indx = rnd.Next(values.Count);
                    var EngWord = values[indx];
                    var questionId = EngWord.WordId;
                    var questionWord = EngWord.EngWord;
                    HttpContext.Session.SetInt32("QuestionId", questionId);
                    ViewBag.firstWord = questionWord;
                    return View();
                }
            }
            else if(filter == 6)
            {
                var values = _context.Words.Where(x => x.Connective != null).ToList();
                if(values == null)
                {
                    return RedirectToAction("TakeFilter");
                }
                else
                {
                    int indx = rnd.Next(values.Count);
                    var EngWord = values[indx];
                    var questionId = EngWord.WordId;
                    var questionWord = EngWord.EngWord;
                    HttpContext.Session.SetInt32("QuestionId", questionId);
                    ViewBag.firstWord = questionWord;
                    return View();
                }

            }
            else if(filter == 1)
            {
                var values = _context.Words.Where(x => x.EngWord.Length > 2).ToList();
                if (values == null)
                {
                    return RedirectToAction("TakeFilter");
                }
                else
                {
                    int indx = rnd.Next(values.Count);
                    var EngWord = values[indx];
                    var questionId = EngWord.WordId;
                    var questionWord = EngWord.EngWord;
                    HttpContext.Session.SetInt32("QuestionId", questionId);
                    ViewBag.firstWord = questionWord;
                    ViewBag.wordLevel = _context.Words.Where(x => x.WordId == questionId).Select(y => y.WordLevel).FirstOrDefault();
                    ViewBag.wordWeek = _context.Words.Where(x => x.WordId == questionId).Select(y => y.Weak).FirstOrDefault();
                    return View();
                }
            }
            else
            {
                return RedirectToAction("TakeFilter");
            }





        }

        [HttpPost]
        public IActionResult CheckResult(string result, int filter)
        {
            var questionId = HttpContext.Session.GetInt32("QuestionId");
            var trWord = _context.Means.Where(x => x.WordId == questionId).Select(y => y.TrMean).FirstOrDefault();
            var engWord = _context.Words.Where(x => x.WordId == questionId).Select(y => y.EngWord).FirstOrDefault();
            ViewBag.engWord = engWord;
            var baseWord = _context.Words.Where(x => x.WordId == questionId).FirstOrDefault();
            baseWord.IsAsked = true;
            baseWord.AskCount++;
            ViewBag.Filter = filter;

            if (result == trWord)
            {
                ViewBag.reusltReturn = true;
                
                baseWord.ScAnswerCount++;
                
            }
            else
            {
                ViewBag.reusltReturn = false;
                ViewBag.rightTrWord = trWord; 
                baseWord.WrongAnswerCount++;
            }
            baseWord.SuccessPercent = baseWord.ScAnswerCount / baseWord.AskCount;
            _context.Update(baseWord);
            _context.SaveChanges();
            return View();
        }

        public IActionResult NextQuestion(int filter)
        {
            return RedirectToAction("Index", new { filter = filter});
        }
        public IActionResult Typo(int filter)
        {
            var questionId = HttpContext.Session.GetInt32("QuestionId");
            var baseWord = _context.Words.Where(x => x.WordId == questionId).FirstOrDefault();
            baseWord.WrongAnswerCount--;
            baseWord.ScAnswerCount++;
            baseWord.SuccessPercent = baseWord.ScAnswerCount / baseWord.AskCount;
            _context.Update(baseWord);
            _context.SaveChanges();
            return RedirectToAction("Index", new { filter = filter });
        }
        public IActionResult ItWasClose(int filter)
        {
            var questionId = HttpContext.Session.GetInt32("QuestionId");
            var baseWord = _context.Words.Where(x => x.WordId == questionId).FirstOrDefault();
            baseWord.WrongAnswerCount += (1/2);
            baseWord.ScAnswerCount -= (1/2);
            baseWord.SuccessPercent = baseWord.ScAnswerCount / baseWord.AskCount;
            _context.Update(baseWord);
            _context.SaveChanges();
            return RedirectToAction("Index", new { filter = filter });
        }
    }
}
