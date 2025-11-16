using EngWordApp.Context;
using EngWordApp.Entity;
using EngWordApp.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class AddNewListController : Controller
{
    private readonly WordContext _context;

    public AddNewListController(WordContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult ImportWords()
    {
        return View(); // Form sayfası
    }

    [HttpPost]
    public async Task<IActionResult> ImportWords(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return BadRequest("Dosya yolu boş olamaz!");
        }

        // Göreli yolu tam yola çevir
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        if (!System.IO.File.Exists(fullPath))
        {
            return BadRequest($"Dosya bulunamadı: {fullPath}");
        }

        var importer = new WordImporter(_context);
        await importer.ImportFromTxtAsync(fullPath);

        return Ok("Kelimeler başarıyla yüklendi!");
    }
}
