/*=====================================================================
  File:     TopologyData.cs

  Summary:  Demonstrates retrieving information about Active Directory  
            topology components - sitelinks, sitelink bridges, sites, 
            adjacent sites, domains, servers, subnets and 
            bridgehead servers.
=======================================================================
                       
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]


namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{

    class ADTopology
    {
        public static void GetData(string targetForestName)
        {
            try
            {
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Forest,targetForestName);


                // Bind to an inter-site transport object using the FindByTransportType
                // method to view details of site links within a particular transport 
                // type, either RPC over IP or SMTP. 
                ActiveDirectoryInterSiteTransport transport = ActiveDirectoryInterSiteTransport.FindByTransportType(context,ActiveDirectoryTransportType.Rpc);

                Console.WriteLine("\tBridge all site links {0}",transport.BridgeAllSiteLinks);

                Console.WriteLine("\tIgnore replication schedule" + "for this inter-site transport? {0}", transport.IgnoreReplicationSchedule);

                // get all the site links within a particular transport type
                Console.WriteLine("\nSite links:\n");
                foreach (ActiveDirectorySiteLink link in transport.SiteLinks)
                {
                    Console.WriteLine("\tSitelink \"{0}\"", link);
                }

                // get all the site link bridges within a particular transport type
                Console.WriteLine("\nSite link bridges:\n");
                foreach (ActiveDirectorySiteLinkBridge bridge in transport.SiteLinkBridges)
                {
                    Console.WriteLine("\tSitelinkBridge \"{0}\"", bridge);
                }

                // bind to a forest object using the GetForest method
                Forest forest = Forest.GetForest(context);

                // get all the sites in the forest
                foreach (ActiveDirectorySite site in forest.Sites)
                {
                    Console.WriteLine("\nSite \"{0}\"", site.Name);
                    Console.WriteLine("\tcontains the following domain(s):");
                    foreach (Domain domain in site.Domains)
                    {
                        Console.WriteLine("\t\t{0}", domain.Name);
                    }
                    Console.WriteLine("\tcontains the following server(s):");
                    foreach (DirectoryServer server in site.Servers)
                    {
                        Console.WriteLine("\t\t{0}", server.Name);
                    }
                    Console.WriteLine("\tcontains the following subnet(s):");
                    foreach (ActiveDirectorySubnet subnet in site.Subnets)
                    {
                        Console.WriteLine("\t\tSubnet: {0} location: {1}",
                                                        subnet.Name,
                                                        subnet.Location);
                    }
                    Console.WriteLine("\nInterSiteTopologyGenerator is {0}",
                                                        site.InterSiteTopologyGenerator);

                    Console.WriteLine("\nBridgehead servers:");
                    foreach (DirectoryServer server in site.BridgeheadServers)
                    {
                        Console.WriteLine("\t\t{0}", server.Name);
                    }

                    Console.WriteLine("\nPreferred Bridgehead servers:");
                    foreach (DirectoryServer server in site.PreferredRpcBridgeheadServers)
                    {
                        Console.WriteLine("\t\t{0}", server.Name);
                    }

                    Console.WriteLine("\nAdjacent sites are:");
                    foreach (ActiveDirectorySite adjSite in site.AdjacentSites)
                    {
                        Console.WriteLine("\t\t{0}", adjSite.Name);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }
    }
}
