using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using Data;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class EmployeeController : ApiController
    {
        //now Cors is enabled for all the methods.in case wish to diable for some methods
        //[DisableCors] but for html pages in its own project will be able to consume...
        public HttpResponseMessage Get(string age="all")
        {
            try
            {

                using (Model1 model = new Model1())
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

                using (Model1 mm = new Model1())
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
                using (Model1 m = new Model1())
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
                using (Model1 mn = new Model1())
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
                using (Model1 m = new Model1())
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

