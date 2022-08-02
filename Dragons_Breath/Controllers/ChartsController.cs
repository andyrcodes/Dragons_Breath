using Dragons_Breath.Data;
using Dragons_Breath.Models.ChartModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Controllers
{
    [Authorize]
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly List<string> _backgroundColors;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
            _backgroundColors = new List<string>
            {
                "#FF0000",
                "#19FF5A",
                "#FFCE19",
                "#0D22FF",
                "#FE7B00",
                "#19D0FF",
                "#D20DFF",
                "#A0FF19",
                "#00FF06",
                "#000000"
            };
        }

        public JsonResult PriorityChart()
        {
            var result = new ChartJSModel();
            int count = 0;
            foreach (var priority in _context.TicketPriorities.ToList())
            {
                result.Labels.Add(priority.Name);
                result.Data.Add(_context.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count());
                if (count < 10)
                {
                    result.BackgroundColor.Add(_backgroundColors[count]);
                }
                else
                {
                    result.BackgroundColor.Add(_backgroundColors[(count % 10)]);
                }
                count++;
            }
            return Json(result);
        }

        public JsonResult StatusChart()
        {
            var result = new ChartJSModel();
            int count = 0;
            foreach (var status in _context.TicketStatuses.ToList())
            {
                result.Labels.Add(status.Name);
                result.Data.Add(_context.Tickets.Where(t => t.TicketStatusId == status.Id).Count());
                if (count < 10)
                {
                    result.BackgroundColor.Add(_backgroundColors[count]);
                }
                else
                {
                    result.BackgroundColor.Add(_backgroundColors[(count % 10)]);
                }
                count++;
            }
            return Json(result);
        }

        public JsonResult TypeChart()
        {
            var result = new ChartJSModel();
            int count = 0;
            foreach (var type in _context.TicketTypes.ToList())
            {
                result.Labels.Add(type.Name);
                result.Data.Add(_context.Tickets.Where(t => t.TicketTypeId == type.Id).Count());
                if (count < 10)
                {
                    result.BackgroundColor.Add(_backgroundColors[count]);
                }
                else
                {
                    result.BackgroundColor.Add(_backgroundColors[(count % 10)]);
                }
                count++;
            }
            return Json(result);

        }

    }
}
