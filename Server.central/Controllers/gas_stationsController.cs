using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Server.central.Models;

namespace Server.central.Controllers
{
    [Route("api")]
    public class GasStationsController : ApiController
    {
        private DB_AGSEntities db = new DB_AGSEntities();

        [HttpGet]
        [Route("getStationinfo/{id}")]
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
        [Route("SetStation/{id}")]
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

        [HttpPost]
        [Route("AddStation")]
        public IHttpActionResult AddStation(gas_stations gas_station)
        {
            if (gas_station == null)
            {
                return BadRequest();
            }

            db.gas_stations.Add(gas_station);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (GasStationExists(gas_station.station_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute(nameof(Get), new { controller = "GasStationsController", id = gas_station.station_id }, gas_station);
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

