using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics;
using System.Threading;
using WEBAPI.Authentication;

namespace WEBAPI.Controllers
{
    //[EnableCorsAttribute("*", "*", "*")]
    public class EmployeeController : ApiController
    {
            //now Cors is enabled for all the methods.in case wish to diable for some methods
            //[DisableCors] but for html pages in its own project will be able to consume...
            //[HttpGet]..use this if use custom methods..405 error code..
            // likes can keep AllowGet but use [HttpGet] to access..if used like Getblablabla then not required.
            [BasicAuthentication]
            public HttpResponseMessage Get(string age = "all")
            {
                try
                {
                    //getting the value of name from Principal Thread..
                    //in fiddler, in composer request URI and in body use
                    //Authorization: Basic base64of(username:password)
                    string username = Thread.CurrentPrincipal.Identity.Name;
                    using (Entities model = new Entities())
                    {
                        switch (username.ToString())
                        {
                            case "all":
                            {
                                var all = model.MyHoneys.ToList();
                                var allData = Request.CreateResponse(HttpStatusCode.OK, all);
                                return allData;
                            }
                            case "indra":
                                {
                                    var values = model.MyHoneys.Where(x => x.gender.Equals("m")).ToList();
                                    var message = Request.CreateResponse(HttpStatusCode.OK, values);
                                    return message;
                                }
                            case "harsha":
                                {
                                    var values = model.MyHoneys.Where(x => x.gender == "f").ToList();
                                    return Request.CreateResponse(HttpStatusCode.OK, values);
                                }

                            default:
                                {
                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Datas not present");
                                }
                        }

                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }

            [HttpGet] //custom method..default starts with Get or Get followed by anything..
            public HttpResponseMessage LoadGet(int id)
            {
                try
                {
                    using (Entities mm = new Entities())
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
                    using (Entities m = new Entities())
                    {
                        m.MyHoneys.Add(myhoney);
                        m.SaveChanges();

                        //201 created...
                        var message = Request.CreateResponse(HttpStatusCode.Created, myhoney);
                        message.Headers.Location = new Uri(Request.RequestUri + myhoney.age.ToString());
                        return message;
                    }
                }
                catch (Exception ex)
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
                    using (Entities mn = new Entities())
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
                    using (Entities m = new Entities())
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