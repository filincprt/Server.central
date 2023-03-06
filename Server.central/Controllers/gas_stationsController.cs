﻿using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Server.central.Models;

namespace Server.central.Controllers
{
    [RoutePrefix("api/gas_stations")]
    public class GasStationsController : ApiController
    {
        private DB_AGSEntities db = new DB_AGSEntities();

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var gas_station = db.gas_stations.Find(id);
            if (gas_station == null)
            {
                return NotFound();
            }
            return Ok(gas_station);
        }

        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult SetStation(gas_stations gas_station)
        {
            if (gas_station == null)
            {
                return BadRequest();
            }

            db.Entry(gas_station).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GasStationExists(gas_station.station_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool GasStationExists(int id)
        {
            return db.gas_stations.Count(e => e.station_id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
