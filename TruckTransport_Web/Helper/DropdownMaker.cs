using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Helper
{
    public class DropdownMaker
    {
        private TruckTransportDbContext _db = null;

        public List<SelectListItem> GetLocationCategories()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<kategorije> locationCategories = _db.kategorije.AsNoTracking().ToList();
                return locationCategories.Select(x => new SelectListItem { Value = x.kategorija_id.ToString(), Text = x.naziv }).ToList();
            }
        }

        public List<SelectListItem> GetKnownLocations()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<poznatelokacije> knownLocations = _db.poznatelokacije.AsNoTracking().ToList();
                return knownLocations.Select(x => new SelectListItem { Value = x.poznatalokacija_id.ToString(), Text = x.naziv }).ToList();
            }
        }

        public List<SelectListItem> GetDriverStops()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<stajalista> driverStops = _db.stajalista.AsNoTracking().ToList();
                return driverStops.Select(x => new SelectListItem { Value = x.stajaliste_id.ToString(), Text = x.naziv }).ToList();
            }
        }

        public List<SelectListItem> GetUnattachedTasks()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<zadaci> tasks = _db.zadaci.AsNoTracking().Where(x=>x.nalog_id == null).ToList();
                return tasks.Select(x => new SelectListItem { Value = x.zadatak_id.ToString(), Text = x.naziv }).ToList();
            }
        }

        public List<SelectListItem> GetUnattachedAndSpecificOrderAttachedTasks(int orderID)
        {
            using (_db = new TruckTransportDbContext())
            {
                List<zadaci> tasks = _db.zadaci.AsNoTracking().Where(x => x.nalog_id == null || x.nalog_id == orderID).ToList();
                return tasks.Select(x => new SelectListItem { Value = x.zadatak_id.ToString(), Text = x.naziv }).ToList();
            }
        }

        public List<SelectListItem> GetOrderConditions()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<stanja> orderConditions = _db.stanja.AsNoTracking().ToList();
                return orderConditions.Select(x => new SelectListItem { Value = x.stanje_id.ToString(), Text = x.opis }).ToList();
            }
        }

        public List<SelectListItem> GetDrivers()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<vozaci> drivers = _db.vozaci.AsNoTracking().ToList();
                return drivers.Select(x => new SelectListItem { Value = x.vozac_id.ToString(), Text = x.ime + " " + x.prezime }).ToList();
            }
        }

        public List<SelectListItem> GetVehicles()
        {
            using (_db = new TruckTransportDbContext())
            {
                List<vozila> drivers = _db.vozila.AsNoTracking().ToList();
                return drivers.Select(x => new SelectListItem { Value = x.vozilo_id.ToString(), Text = x.naziv + ", " + x.marka }).ToList();
            }
        }
    }
}