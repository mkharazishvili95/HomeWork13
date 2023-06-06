using HomeWork13.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class AppointmentController : Controller
{
    private const string jsonFilePath = "appointments.json";

    public IActionResult Index()
    {
        List<Appointment> appointments = LoadAppointments();

        return View(appointments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Appointment appointment)
    {
        if (!IsValidTime(appointment.Time))
        {
            ModelState.AddModelError("Time", "The visit time must be between 10:00 and 19:00.");
            return View(appointment);
        }

        List<Appointment> appointments = LoadAppointments();
        appointments.Add(appointment);
        SaveAppointments(appointments);

        return RedirectToAction("Index");
    }

    private List<Appointment> LoadAppointments()
    {
        if (System.IO.File.Exists(jsonFilePath))
        {
            string json = System.IO.File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<Appointment>>(json);
        }

        return new List<Appointment>();
    }

    private void SaveAppointments(List<Appointment> appointments)
    {
        string json = JsonSerializer.Serialize(appointments);
        System.IO.File.WriteAllText(jsonFilePath, json);
    }

    private bool IsValidTime(string time)
    {
        // Custom validation logic to check if time is between 10:00 and 19:00
        // You can use DateTime.TryParse or other methods to validate the time format
        // Here's an example using TimeSpan:
        if (TimeSpan.TryParse(time, out TimeSpan visitTime))
        {
            TimeSpan startTime = TimeSpan.Parse("10:00");
            TimeSpan endTime = TimeSpan.Parse("19:00");
            return visitTime >= startTime && visitTime <= endTime;
        }

        return false;
    }
}
