using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParkingRoutes.Data;
using ParkingRoutes.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingRoutes.Controllers
{
    public class RoutesController : Controller
    {
        private readonly AppDbContext _context;

        public RoutesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Route.Include(r => r.Car).Include(r => r.DepartureParking).Include(r => r.DestinationParking).ToListAsync());
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            FillViewBag();
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartureParkingId,DestinationParkingId,CarId")] Route route)
        {
            if (ModelState.IsValid)
            {
                var departure = _context.Parking.Single(x => x.Id == route.DepartureParkingId);
                var destination = _context.Parking.Single(x => x.Id == route.DestinationParkingId);
                var car = _context.Car.Single(x => x.Id == route.CarId);

                route.Distance = CalculateDistance(departure.Latitude,
                    departure.Longitude,
                    destination.Latitude,
                    destination.Longitude);
                route.FuelNeeded = CalculateConsumption(car, route.Distance);
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FillViewBag();
            var route = await _context.Route.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartureParkingId,DestinationParkingId,CarId")] Route route)
        {
            if (id != route.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var departure = _context.Parking.Single(x => x.Id == route.DepartureParkingId);
                    var destination = _context.Parking.Single(x => x.Id == route.DestinationParkingId);
                    var car = _context.Car.Single(x => x.Id == route.CarId);

                    route.Distance = CalculateDistance(departure.Latitude,
                        departure.Longitude,
                        destination.Latitude,
                        destination.Longitude);
                    route.FuelNeeded = CalculateConsumption(car, route.Distance);

                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.Id))
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
            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Route.FindAsync(id);
            _context.Route.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private dynamic FillViewBag()
        {
            ViewBag.ParkingIds = _context.Parking.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.CarIds = _context.Car.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });

            return ViewBag;
        }

        // Wanted to use GoogleApi but they request to create billing account and I'm not willing to do so,
        // so I just simply calculate flight distance
        private double CalculateDistance(double depLatitude, double depLongitude, double desLatitude, double desLongitude)
        {
            var departure = new GeoCoordinate(depLatitude, depLongitude);
            var destination = new GeoCoordinate(desLatitude, desLongitude);

            return Math.Round(departure.GetDistanceTo(destination) / 1000D, 2);
        }

        private double CalculateConsumption(Car car, double distance)
        {
            return Math.Round(((double)(car.AverageConsumption / 100)) * distance + car.EngineStartConsumption, 2);
        }

        private bool RouteExists(int id)
        {
            return _context.Route.Any(e => e.Id == id);
        }
    }
}
