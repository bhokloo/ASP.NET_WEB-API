using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;

namespace API.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get(string age="all")
        {
            try
            {

                using (Model2 model = new Model2())
                {
                    switch (age.ToString())
                    {
                        case "all":
                            {
                                var values = model.MyHoneys.ToList();
                                var message = Request.CreateResponse(HttpStatusCode.OK, values);
                                return message;
                            }
                        case "1":
                            {
                                var values = model.MyHoneys.Where(x => x.age == 1).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, values);
                            }
                        default:
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Datas not present");
                            }
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
         }
               

        [HttpGet] //custom method..default starts with Get or Get followed by anything..
        public HttpResponseMessage LoadGet(int id)
        {
            try
            {

                using (Model2 mm = new Model2())
                {

                    var s = mm.MyHoneys.Where(x => x.age == id).ToList();

                    if (s != null)
                    {
                        //200 ok
                        return Request.CreateResponse(HttpStatusCode.OK, s);
                    }

                    else
                    {
                        //404 not found
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The age " + id.ToString() + " is not present");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
          }

        //[HttpPost] - if used cutom method..
        public HttpResponseMessage Post([FromBody] MyHoney myhoney)
        {
            try
            {
                using (Model2 m = new Model2())
                {
                    m.MyHoneys.Add(myhoney);
                    m.SaveChanges();

                    //201 created...
                    var message = Request.CreateResponse(HttpStatusCode.Created, myhoney);
                    message.Headers.Location = new Uri(Request.RequestUri + myhoney.age.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                //400 bad request
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //default is [FromBody]...to force it to get values from URI. use [FromUri].
        public HttpResponseMessage Put([FromBody]int id, [FromUri] MyHoney myHoney)
        {
            try
            {
                using (Model2 mn = new Model2())
                {
                    Debug.WriteLine("PUT" + id + "---" + myHoney.name);
                    MyHoney ss = mn.MyHoneys.Where(x => x.age == id).FirstOrDefault();
                    if (ss != null)
                    {
                        ss.name = myHoney.name;
                        ss.age = myHoney.age;
                        mn.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, myHoney);
                        message.Headers.Location = new Uri(Request.RequestUri + myHoney.age.ToString());
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "the age" + id.ToString() + "not present");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (Model2 m = new Model2())
                {
                    var ass = m.MyHoneys.Where(x => x.age == id).FirstOrDefault();
                    if (ass != null)
                    {
                        m.MyHoneys.Remove(ass);
                        m.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, ass);
                        // 200 deleted
                        return message;
                    }
                    else
                    {
                        // 404 not found
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Age" + id.ToString() + "Not found");
                    }
                }
            }
            catch (Exception ex)
            {
                //400 bad request
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
                
            }
        }
    }

