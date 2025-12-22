using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChromeHistoryAnalyzer.Data;

namespace ChromeHistoryAnalyzer.Controllers;

public class HistoryController(ChromeHistoryDbContext context) : Controller
{
    private const int PageSize = 50;

    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        var query = context.Urls.AsQueryable();

        // Фильтрация по поисковому запросу
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(u => 
                u.Title!.Contains(searchString) || 
                u.Url!.Contains(searchString));
        }

        // Подсчет общего количества записей
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

        // Получение данных с пагинацией
        var urls = await query
            .OrderByDescending(u => u.LastVisitTime)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.SearchString = searchString;
        ViewBag.TotalItems = totalItems;

        return View(urls);
    }

    public async Task<IActionResult> TopSearches()
    {
        // Поисковые запросы (обычно содержат search, query, q= в URL)
        var searchUrls = await context.Urls
            .Where(u => u.Url!.Contains("search") || 
                        u.Url!.Contains("query") || 
                        u.Url!.Contains("?q=") ||
                        u.Url!.Contains("&q="))
            .OrderByDescending(u => u.VisitCount)
            .Take(100)
            .ToListAsync();

        return View(searchUrls);
    }

    public async Task<IActionResult> MostVisited()
    {
        var mostVisited = await context.Urls
            .Where(u => u.Hidden == 0)
            .OrderByDescending(u => u.VisitCount)
            .Take(50)
            .ToListAsync();

        return View(mostVisited);
    }
}