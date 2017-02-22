using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HackathonAwesomo.Controllers
{
    public class PatientsController : ApiController
    {
        [Route("api/Patients")][HttpGet]
        public IHttpActionResult GetPatients()
        {
            //var Patients = RelayrBot.GetAllPatients();
            var Patients = DBAccess.GetPatients();
            return Ok(Patients);
        }

        [Route("api/Patients/Details")][HttpGet]
        public IHttpActionResult GetPatientDetails()
        {
            var parameters = Request.GetQueryNameValuePairs().ToList();
            if (parameters.Count > 0 && parameters[0].Key == "device")
            {
                Patient patient = DBAccess.GetPatientDetails(parameters[0].Value);
                return Ok(patient);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("api/Patients")][HttpPost]
        public IHttpActionResult PostPatients([FromBody] Patient newPatient)
        {
            if (newPatient.id == null)
            {
                Patient patient = RelayrBot.RegisterPatient(newPatient.name);

                if (patient.id == null)
                {
                    return BadRequest("Invalid Data Recieved");
                }
                else
                {
                    newPatient.id = patient.id;
                    newPatient.created = patient.created;
                    DBAccess.InsertPatient(newPatient);
                    return Ok(newPatient);
                }
            }
            else
            {
                DBAccess.UpdatePatient(newPatient);
                return Ok(DBAccess.GetPatientDetails(newPatient.id));
            }
        }

        [Route("api/Patients/Readings")][HttpGet]
        public IHttpActionResult GetReadings()
        {
            var parameters = Request.GetQueryNameValuePairs().ToList();
            if (parameters.Count > 0 && parameters[0].Key == "device")
            {
                var reading = RelayrBot.GetPatientReading(parameters[0].Value);
                return Ok(reading);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
