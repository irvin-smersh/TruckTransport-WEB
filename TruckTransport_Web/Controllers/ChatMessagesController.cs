using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;

namespace TruckTransport_Web.Controllers
{
    [Audit]
    [Authorization(isLoggedIn: true)]
    public class ChatMessagesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ChatVM model = new ChatVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var drivers = _db.vozaci.ToList();

                model.FirstDriverID = drivers.First().vozac_id;
                model.ChatUsersMessages = new List<ChatUserMessagesVM>();

                if (drivers != null)
                {
                    foreach (var driver in drivers)
                    {
                        ChatUserMessagesVM userMessagesModel = new ChatUserMessagesVM();
                        userMessagesModel.DriverID = driver.vozac_id;
                        userMessagesModel.DriverFullName = driver.ime + " " + driver.prezime;
                        userMessagesModel.UserMessages = _db.poruke.Where(s => s.vozac_id == driver.vozac_id).Select(s => new ChatMessagesVM
                        {
                            MessageID = s.poruka_id,
                            DriverID = s.vozac_id,
                            UnixCreationTime = s.vrijeme,
                            SByteFromDriver = s.odvozaca,
                            MessageText = s.text
                        }).OrderByDescending(s => s.MessageID).Take(50).ToList();

                        foreach (var userMessage in userMessagesModel.UserMessages)
                        {
                            userMessage.CreationTime = UnixTime.ConvertToDateTimeString(userMessage.UnixCreationTime);
                            userMessage.FromDriver = Convert.ToBoolean(userMessage.SByteFromDriver);
                        }

                        model.ChatUsersMessages.Add(userMessagesModel);
                    }
                }
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult LoadMessagesForSpecificDriver(int driverID)
        {
            ChatUserMessagesVM userMessagesModel = new ChatUserMessagesVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var driver = _db.vozaci.Where(x => x.vozac_id == driverID).FirstOrDefault();

                userMessagesModel.DriverID = driver.vozac_id;
                userMessagesModel.DriverFullName = driver.ime + " " + driver.prezime;
                userMessagesModel.UserMessages = _db.poruke.Where(s => s.vozac_id == driver.vozac_id).Select(s => new ChatMessagesVM
                {
                    MessageID = s.poruka_id,
                    DriverID = s.vozac_id,
                    UnixCreationTime = s.vrijeme,
                    SByteFromDriver = s.odvozaca,
                    MessageText = s.text
                }).OrderByDescending(s => s.MessageID).Take(50).ToList();

                foreach (var userMessage in userMessagesModel.UserMessages)
                {
                    userMessage.CreationTime = UnixTime.ConvertToDateTimeString(userMessage.UnixCreationTime);
                    userMessage.FromDriver = Convert.ToBoolean(userMessage.SByteFromDriver);
                }
            }

            return PartialView(viewName: "_UserMessages", model: userMessagesModel);
        }

        [HttpGet]
        public ActionResult LoadContacts()
        {
            List<ChatUserMessagesVM> userContactsModel = new List<ChatUserMessagesVM>();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var drivers = _db.vozaci.ToList();

                if (drivers != null)
                {
                    foreach (var driver in drivers)
                    {
                        ChatUserMessagesVM userMessagesModel = new ChatUserMessagesVM();
                        userMessagesModel.DriverID = driver.vozac_id;
                        userMessagesModel.DriverFullName = driver.ime + " " + driver.prezime;
                        userMessagesModel.UserMessages = _db.poruke.Where(s => s.vozac_id == driver.vozac_id).Select(s => new ChatMessagesVM
                        {
                            MessageID = s.poruka_id,
                            DriverID = s.vozac_id,
                            UnixCreationTime = s.vrijeme,
                            SByteFromDriver = s.odvozaca,
                            MessageText = s.text
                        }).OrderByDescending(s => s.MessageID).Take(1).ToList();

                        foreach (var userMessage in userMessagesModel.UserMessages)
                        {
                            userMessage.CreationTime = UnixTime.ConvertToDateTimeString(userMessage.UnixCreationTime);
                            userMessage.FromDriver = Convert.ToBoolean(userMessage.SByteFromDriver);
                        }

                        userContactsModel.Add(userMessagesModel);
                    }
                }
            }

            return PartialView(viewName: "_UserContacts", model: userContactsModel);
        }

        [HttpPost]
        public ActionResult SendMessage(int driverID, string messageText)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (messageText != null && messageText != "")
                {
                    poruke messageDB = new poruke();

                    messageDB.vozac_id = driverID;
                    messageDB.vrijeme = UnixTime.GetUnixTimeNow();
                    messageDB.text = messageText;
                    messageDB.odvozaca = 0;

                    _db.poruke.Add(messageDB);
                    _db.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpGet]
        public JsonResult CheckNumberOfMessages()
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var numOfMessages = _db.poruke.Where(x => x.odvozaca == 1 && !x.text.Contains("SOS")).Count();

                if (numOfMessages > Constants.NumberOfMessages)
                {
                    Constants.NumberOfMessages = numOfMessages;
                    return Json(new { setNotif = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { setNotif = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckSOS()
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var numOfSOSMessages = _db.poruke.Where(x => x.odvozaca == 1 && x.text.Contains("SOS")).Count();

                if (numOfSOSMessages > Constants.NumberOfSOSMessages)
                {
                    Constants.NumberOfSOSMessages = numOfSOSMessages;

                    var sosMessage = _db.poruke.Include(x => x.vozaci).Where(x => x.odvozaca == 1 && x.text.Contains("SOS")).OrderByDescending(x => x.poruka_id).First();

                    var splitMessage = sosMessage.text.Split(',');

                    SetSOSVM model = new SetSOSVM();

                    model.DriverID = sosMessage.vozac_id;
                    model.DriverFullName = sosMessage.vozaci.ime + " " + sosMessage.vozaci.prezime;
                    model.SOSMessageTime = UnixTime.ConvertToDateTimeString(sosMessage.vrijeme);
                    model.SOSMessageText = splitMessage[2];

                    var latLongSplitMessage = splitMessage[1].Split(';');

                    model.Latitude = latLongSplitMessage[0];
                    model.Longitude = latLongSplitMessage[1];

                    return Json(new { setNotif = true, sOSMessage = model }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { setNotif = false }, JsonRequestBehavior.AllowGet);
        }
    }
}