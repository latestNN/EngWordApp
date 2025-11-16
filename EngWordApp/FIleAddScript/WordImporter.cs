using EngWordApp.Context;
using EngWordApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EngWordApp.Service
{
    public class WordImporter
    {
        private readonly WordContext _context;

        public WordImporter(WordContext context)
        {
            _context = context;
        }

        public async Task ImportFromTxtAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Dosya bulunamadı", filePath);

            var lines = await File.ReadAllLinesAsync(filePath);
            if (lines == null || lines.Length == 0)
                throw new Exception("Dosya boş veya okunamadı.");

            // --- Header'dan type ve hafta(weak) bilgilerini al ---
            var headerLine = lines[0].Trim();
            int typeValue = 0;
            string weakValue = null;

            var mType = Regex.Match(headerLine, @"\btype\s*(\d+)\b", RegexOptions.IgnoreCase);
            if (mType.Success)
                int.TryParse(mType.Groups[1].Value, out typeValue);

            var mHafta = Regex.Match(headerLine, @"\bhafta\s*([^\s,]+)\b", RegexOptions.IgnoreCase);
            if (mHafta.Success)
                weakValue = $"Hafta {mHafta.Groups[1].Value}";

            // --- Satırları işle (ilk satır header olduğu için Skip(1)) ---
            foreach (var rawLine in lines.Skip(1))
            {
                var line = rawLine?.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 🔸 Kategori başlıklarını (B--, C-- vs.) atla
                if (Regex.IsMatch(line, @"^[A-Za-zÇŞĞÜÖİ]{1,3}[-–]{2,3}$"))
                    continue;

                // '*' varsa Connective = true yap ve '*' karakterlerini temizle
                bool isConnective = line.Contains('*');
                if (isConnective)
                    line = line.Replace("*", "").Trim();

                // "EngWord – means / Level" formatını güvenli parse et (– veya - olabilir)
                var match = Regex.Match(line, @"^(?<eng>[^–\-]+)[–\-](?<rest>.+)$");
                if (!match.Success) continue;

                var engWord = match.Groups["eng"].Value.Trim();
                var meansAndLevel = match.Groups["rest"].Value.Trim();

                // means ve level ayrımı: "nefis, parlak / A2" gibi
                var levelSplit = meansAndLevel.Split('/');
                var meansPart = levelSplit[0].Trim();
                var wordLevel = levelSplit.Length > 1 ? levelSplit[1].Trim() : null;

                // Temizlik: wordLevel içindeki olası kalan '*' veya ekstra boşlukları kaldır
                if (!string.IsNullOrEmpty(wordLevel))
                    wordLevel = wordLevel.Replace("*", "").Trim();

                // DB'de varsa getir, yoksa oluştur
                var existingWord = await _context.Words
                    .Include(w => w.Means)
                    .FirstOrDefaultAsync(w => w.EngWord == engWord);

                if (existingWord == null)
                {
                    var word = new Word
                    {
                        EngWord = engWord,
                        WordLevel = wordLevel,
                        SuccessPercent = 0,
                        Type = typeValue,       // header'dan gelen type
                        Weak = weakValue,       // header'dan gelen "Hafta X"
                        Connective = isConnective,
                        Means = new List<Mean>()
                    };

                    foreach (var mean in meansPart.Split(',').Select(m => m.Trim()))
                    {
                        if (!string.IsNullOrWhiteSpace(mean))
                            word.Means.Add(new Mean { TrMean = mean });
                    }

                    _context.Words.Add(word);
                }
                else
                {
                    // Yeni anlamları ekle (case-insensitive kontrol)
                    foreach (var mean in meansPart.Split(',').Select(m => m.Trim()))
                    {
                        if (string.IsNullOrWhiteSpace(mean)) continue;
                        if (!existingWord.Means.Any(m => string.Equals(m.TrMean, mean, StringComparison.OrdinalIgnoreCase)))
                        {
                            existingWord.Means.Add(new Mean
                            {
                                TrMean = mean,
                                WordId = existingWord.WordId
                            });
                        }
                    }

                    // Eksik bilgileri header/line bilgisiyle doldur
                    if (string.IsNullOrEmpty(existingWord.WordLevel) && !string.IsNullOrEmpty(wordLevel))
                        existingWord.WordLevel = wordLevel;

                    if ((existingWord.Type == 0) && (typeValue != 0))
                        existingWord.Type = typeValue;

                    if (string.IsNullOrEmpty(existingWord.Weak) && !string.IsNullOrEmpty(weakValue))
                        existingWord.Weak = weakValue;

                    if (isConnective)
                        existingWord.Connective = true;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
