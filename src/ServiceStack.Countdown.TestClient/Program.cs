//  
// Copyright (c) Trafikselskabet Movia. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//
using System;
using System.Linq;

using ServiceStack.Countdown.TestClient.Services;

namespace ServiceStack.Countdown.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Set up client and get credentials.
                var client = new CountdownServiceClient();

                Console.Write("Enter username: ");
                client.ClientCredentials.UserName.UserName = Console.ReadLine();

                Console.Write("Enter password: ");
                client.ClientCredentials.UserName.Password = Console.ReadLine();

                Console.WriteLine();

                // Test Version Method
                Console.WriteLine("Getting Version ...");
                var ver = client.Version();
                Console.WriteLine($"Received Version '{ver}'");

                Console.WriteLine();

                // Test GetStopPointSet Method
                Console.WriteLine("Getting Stop Points ...");

                var stopPoints = client.GetStopPointSet();

                Console.WriteLine($"Received {stopPoints.stopPoints.Count():n0} stop points. Displaying first 10:");
                Console.WriteLine($"{"Id",5}  {"Name"}");
                stopPoints.stopPoints.Take(10).ToList().ForEach(x => Console.WriteLine($"{x.id,5}  {x.name}"));

                Console.WriteLine();

                //// Test PostStatusReportSet Method
                //Console.WriteLine($"Subscribing to stop point id '{stop}' ...");
                //var sub = client.PostStatusReportSet(new statusReportSet()
                //{
                //    statusReports = new[] { new statusReportSet.statusReport()
                //    {
                //        deviceId = 1,
                //        lastUpdate = DateTime.UtcNow,
                //        stopPointId = stopPoints.stopPoints.First().id
                //    }
                //}
                //});

                // Test PostStatusReportSet Method
                Console.Write("Enter stop number: ");
                var stop = Console.ReadLine();
                Console.Write("Enter device number: ");
                var device = Console.ReadLine();

                Console.WriteLine($"Subscribing to stop point id '{stop}' to device '{device}'.");
                var sub = client.PostStatusReportSet(new statusReportSet()
                {
                    statusReports = new[] { new statusReportSet.statusReport()
                    {
                        deviceId = Int32.Parse(device),
                        lastUpdate = DateTime.UtcNow,
                        stopPointId = Int32.Parse(stop)
                    }
                }
                });
                Console.WriteLine($"Received {sub}");

                Console.WriteLine();

                // Test GetDepartureTimeSet Method
                Console.WriteLine("Getting Departure Times ...");

                var dep1 = client.GetDepartureTimeSet();
                Console.WriteLine($"Received departures for {dep1.stopPoints.Count():n0} stop points. Displaying first 10:");
                Console.WriteLine($"{"Id",5}  {"Name"}");
                dep1.stopPoints.Take(10).ToList().ForEach(x => {
                    Console.WriteLine($"{x.id,5}  {x.name}");
                    Console.WriteLine($"          {"Line",5}  {"Destination",-20}  {"Time",8}");
                    x.lineDestinations.ToList().ForEach(y =>
                    {
                        y.departures.ToList().ForEach(lineDep => Console.WriteLine($"          {y.designation,5}  {y.destination,-20}  {lineDep.time,8:t}"));
                    });
                });

                Console.WriteLine();

                // Test GetDeviationMessageSet Method
                Console.WriteLine("Getting Deviations ...");

                var dev1 = client.GetDeviationMessageSet();
                Console.WriteLine($"Received deviations for {dev1.stopPoints.Count():n0} stop points. Displaying first 10 stops:");
                Console.WriteLine($"{"Id",5}  {"Name"}");
                dev1.stopPoints.Take(10).ToList().ForEach(x => {
                    Console.WriteLine($" {x.id,5}  {x.name}");
                    Console.WriteLine($"                   {"Deviation text",5}    ");
                    x.deviations.Take(1).ToList().ForEach(y =>
                    {
                        y.header.Take(1).ToList().ForEach(stopDev => Console.WriteLine($"                   {y.header,5}    "));
                        Console.WriteLine();
                    });
                });

                Console.WriteLine();

            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    ex = ex.InnerException;
                }
            }

            Console.WriteLine();

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }
    }
}
